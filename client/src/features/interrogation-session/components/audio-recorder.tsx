'use client';

import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Mic, Square, Trash2, Send, Loader2, Volume2, FileDown, CheckCircle2, BookOpenText } from 'lucide-react';
import { useAudioRecorder } from '@/hooks/use-audio-recorder';
import { transcribeDiarize, Data, LLMQuery, Choice } from '../api/ai.service';
import { createEvidence } from '../../evidence/api/evidence.service';
import { toast } from 'sonner';
import { Badge } from '@/components/ui/badge';
import { ScrollArea } from '@/components/ui/scroll-area';
import { set, del, getMany, keys, get } from 'idb-keyval';

interface AudioRecorderProps {
  onResult?: (data: Data) => void;
  caseId?: string;
  suspectId?: string;
  setAiSummary?: (summary: Choice[]) => void;
  aiSummerArg?: string;
}

interface Recording {
  id: string;
  blob: Blob;
  time: number;
  isSaved: boolean;
  transcription?: Data;
  isSending?: boolean;
}

export function AudioRecorder({ onResult, caseId, suspectId, setAiSummary, aiSummerArg }: AudioRecorderProps) {
  const {
    isRecording,
    audioBlob: latestBlob,
    recordingTime,
    startRecording,
    stopRecording,
    resetRecording,
  } = useAudioRecorder();

  const MAX_TIME = 600; // 10 minutes in seconds
  const [recordings, setRecordings] = useState<Recording[]>([]);
  const [isSavingInProgress, setIsSavingInProgress] = useState<string | null>(null);
  const [isAiLoading, setIsAiLoading] = useState(false);

  React.useEffect(() => {
    
// get all recordings from local storage
keys().then((keys) => {
  getMany(keys).then((values) => {
    if(!suspectId || !caseId) return;
    const recordings = values.filter((v: any) => v.suspectId === suspectId && v.caseId === caseId);
    setRecordings(recordings.map((v: any) => ({id: v.id, blob: v.file, time: v.time, isSaved: true, transcription: v.transcription})));
  });
});
  }, [suspectId, caseId]);
  // Effect to automatically stop recording at 10 minutes
  React.useEffect(() => {
    if (isRecording && recordingTime >= MAX_TIME) {
      stopRecording();
      toast.info('تم الوصول للحد الأقصى للتسجيل (10 دقائق)');
    }
  }, [isRecording, recordingTime, stopRecording]);

  // Handle addition of new recording when one finishes
  React.useEffect(() => {
    if (!isRecording && latestBlob) {
      const newRecording: Recording = {
        id: crypto.randomUUID(),
        blob: latestBlob,
        time: recordingTime,
        isSaved: false,
      };
      setRecordings(prev => [...prev, newRecording]);
      resetRecording();
    }
  }, [isRecording, latestBlob, recordingTime, resetRecording]);

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  const handleStart = async () => {
    try {
      await startRecording();
    } catch (err) {
      toast.error('تعذر الوصول إلى الميكروفون');
    }
  };

  const handleSend = async (recordingId: string) => {
    const recording = recordings.find(r => r.id === recordingId);
    if (!recording) return;

    try {
      await handleSaveAsEvidence(recordingId);
      setRecordings(prev => prev.map(r => r.id === recordingId ? { ...r, isSending: true } : r));
      const data = await transcribeDiarize(recording.blob, 'ar', 2);
      // const query = data.segments.map((segment) => `${segment.speaker}: ${segment.text}`).join("\n");
      // const llmResponse = await LLMQuery(query);
      // if (setAiSummary && llmResponse.choices) setAiSummary(llmResponse.choices);
      
      
      setRecordings(prev => prev.map(r => r.id === recordingId ? { ...r, transcription: data, isSending: false } : r));
      
      if (onResult) onResult(data);
      toast.success('تمت المعالجة بنجاح');
    } catch (err) {
      console.error(err);
      toast.error('فشلت عملية المعالجة');
      setRecordings(prev => prev.map(r => r.id === recordingId ? { ...r, isSending: false } : r));
    }
  };

  const handleSaveAsEvidence = async (recordingId: string) => {
    const recording = recordings.find(r => r.id === recordingId);
    if (!recording || recording.isSaved) return;
    try {
      // setIsSavingInProgress(recordingId);
      
      const fileName = `Recording_${new Date().toISOString().replace(/[:.]/g, '-')}.wav`;
      
      const file = new File([recording.blob], fileName, { type: 'audio/wav' });

        await set(recordingId , {file: recording.blob, suspectId, caseId, id: recording.id});

      
      const res = await createEvidence({
        file,
        caseId,
        suspectId,
        description: `تسجيل صوتي استجواب #${recordings.indexOf(recording) + 1} - ${new Date().toLocaleString('ar-EG')}`
      });

      if (res) {
        await del(recordingId);
        toast.success('تم حفظ التسجيل في المرفقات بنجاح');
        setRecordings(prev => prev.map(r => r.id === recordingId ? { ...r, isSaved: true } : r));
      } else {
        toast.error(res || 'فشل حفظ التسجيل');
      }
    } catch (err) {
      console.error(err);
      toast.error('حدث خطأ أثناء حفظ التسجيل');
    } 
  };
const handleSummary = async () => {
 try { 
  setIsAiLoading(true);
  if (!aiSummerArg){
    toast.error('يجب تحديد النص. يجب الضغط على تحليل اولاً');
    return
  };
      const llmResponse = await LLMQuery(aiSummerArg.replace("--- [تفريغ صوتي جديد] ---", ""));
      if (setAiSummary && llmResponse.choices) setAiSummary(llmResponse.choices);
      toast.success('تم تحليل النص بنجاح');
    } catch (err) {
      console.error(err);
      toast.error('حدث خطأ أثناء تحليل النص');
    } finally {
      setIsAiLoading(false);
    }
}
  const playAudio = (blob: Blob) => {
    const url = URL.createObjectURL(blob);
    const audio = new Audio(url);
    audio.play();
  };

  const deleteRecording = (id: string) => {
    setRecordings(prev => prev.filter(r => r.id !== id));
  };

  return (
    <Card className="w-full overflow-hidden  border-primary/10 transition-all duration-300">
      <CardContent className="p-6">
        <div className="flex flex-col items-center space-y-6">
          {/* Active Recording State */}
          {isRecording && (
            <div className="flex flex-col items-center space-y-2">
              <div className="relative animate-pulse">
                <div className="h-16 w-16 rounded-full flex items-center justify-center bg-destructive text-white">
                  <Square className="h-8 w-8" />
                </div>
                <span className="absolute -top-1 -right-1 flex h-4 w-4">
                  <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-destructive opacity-75"></span>
                  <span className="relative inline-flex rounded-full h-4 w-4 bg-destructive"></span>
                </span>
              </div>
              <span className="text-3xl font-mono font-bold text-foreground">
                {formatTime(recordingTime)}
              </span>
              <Badge variant="destructive" className="animate-pulse">جاري التسجيل...</Badge>
              <p className="text-xs text-muted-foreground mt-1">الحد الأقصى: 10 دقائق</p>
            </div>
          )}

          {/* Controls */}
          <div className="flex flex-col items-center gap-4">
            {!isRecording && (
              <div className="flex flex-col items-center gap-2">
                <Button 
                  type="button"
                  onClick={handleStart} 
                  disabled={!caseId || !suspectId}
                  size="lg" 
                  className="rounded-full px-8 gap-2"
                >
                  <Mic className="h-5 w-5" />
                  تسجيل جديد
                </Button>
                {(!caseId || !suspectId) && (
                  <p className="text-xs text-destructive font-medium">
                    يجب اختيار القضية والمشتبه به قبل بدء التسجيل
                  </p>
                )}
              </div>
            )}

            {isRecording && (
              <Button type="button" onClick={stopRecording} variant="destructive" size="lg" className="rounded-full px-8 gap-2">
                <Square className="h-5 w-5" />
                إيقاف التسجيل
              </Button>
            )}
          </div>

          {/* Recordings List */}
          {recordings.length > 0 && (
            <div className="w-full space-y-4 mt-4">
              <h3 className="text-lg font-semibold text-right border-b pb-2">قائمة التسجيلات ({recordings.length})</h3>
              <div className="grid gap-4">
                {recordings.map((rec, index) => (
                  <div key={rec.id} className="bg-card border rounded-xl p-4 shadow-sm transition-all hover:shadow-md">
                    <div className="flex items-center justify-between gap-4">
                      {/* Left: Info & Play */}
                      <div className="flex items-center gap-3">
                         <div className="h-10 w-10 rounded-full bg-primary/10 flex items-center justify-center text-primary font-bold">
                           {index + 1}
                         </div>
                         <div className="flex flex-col">
                           <span className="font-medium">تسجيل استجواب #{index + 1}</span>
                           <span className="text-xs text-muted-foreground font-mono">{formatTime(rec.time)}</span>
                         </div>
                      </div>

                      {/* Right: Actions */}
                      <div className="flex items-center gap-2">
                        <Button 
                          type="button"
                          onClick={() => playAudio(rec.blob)} 
                          variant="ghost" 
                          size="icon" 
                          className="h-9 w-9 rounded-full"
                          title="تشغيل"
                        >
                          <Volume2 className="h-4 w-4" />
                        </Button>
                       

                        {/* <Button 
                          type="button"
                          onClick={() => handleSaveAsEvidence(rec.id)} 
                          disabled={rec.isSaved || isSavingInProgress === rec.id}
                          variant={rec.isSaved ? "secondary" : "outline"}
                          size="sm"
                          className={`rounded-full gap-1 ${rec.isSaved ? 'text-green-600' : ''}`}
                        >
                          {isSavingInProgress === rec.id ? (
                            <Loader2 className="h-4 w-4 animate-spin" />
                          ) : rec.isSaved ? (
                            <CheckCircle2 className="h-4 w-4" />
                          ) : (
                            <FileDown className="h-4 w-4" />
                          )}
                          {rec.isSaved ? 'تم الحفظ' : 'حفظ'}
                        </Button> */}

                        <Button 
                          type="button"
                          onClick={() => handleSend(rec.id)} 
                          disabled={rec.isSending}
                          variant="default"
                          size="sm"
                          className="rounded-full gap-1"
                        >
                          {rec.isSending ? (
                            <Loader2 className="h-4 w-4 animate-spin" />
                          ) : (
                            <Send className="h-4 w-4" />
                          )}
                          تحليل
                        </Button>
                         <Button 
                          type="button"
                          onClick={handleSummary} 
                          variant="default" 
                          size="icon" 
                          className="h-9 w-9 rounded-full"
                          title="تلخيص"
                          disabled={!aiSummerArg || isAiLoading}
                        >
                          {isAiLoading ? (
                            <Loader2 className="h-4 w-4 animate-spin" />
                          ) : (
                            <BookOpenText className="h-4 w-4" />
                          )}
                        </Button>

                        <Button 
                          type="button"
                          onClick={() => deleteRecording(rec.id)} 
                          variant="ghost" 
                          size="icon" 
                          className="h-9 w-9 rounded-full text-destructive hover:bg-destructive/10"
                        >
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </div>
                    </div>

                    {/* Result for this specific recording */}
                    {rec.transcription && (
                      <div className="mt-4 pt-4 border-t space-y-2">
                        <Badge variant="outline" className="mb-2">نتائج التحليل</Badge>
                        <ScrollArea className="h-[150px] w-full rounded-md border p-3 bg-muted/20">
                          <div className="space-y-3" dir="rtl">
                            {rec.transcription.segments.map((segment, sIndex) => (
                              <div key={sIndex} className="text-xs border-r-2 border-primary/30 pr-2">
                                <span className="font-bold text-primary">
                                  {segment.speaker === '0' ? 'المحقق' : 'المشتبه به'}:
                                </span>
                                <p className="text-muted-foreground mt-0.5">{segment.text}</p>
                              </div>
                            ))}
                          </div>
                        </ScrollArea>
                      </div>
                    )}
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
}