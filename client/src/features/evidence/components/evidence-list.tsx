'use client';

import { useEffect, useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { FileIcon, Download, Trash2Icon, FileText, Loader2, Play } from 'lucide-react';
import { Evidence } from '../types/evidence';
import { getEvidenceByCaseId, getEvidenceBySuspectId, deleteEvidence, getEvidenceList } from '../api/evidence.service';
import { toast } from 'sonner';

interface EvidenceListProps {
    caseId?: string;
    suspectId?: string;
    onAddEvidence?: () => void;
}

export function EvidenceList({ caseId, suspectId, onAddEvidence }: EvidenceListProps) {
    const [evidence, setEvidence] = useState<Evidence[]>([]);
    const [loading, setLoading] = useState(true);
    const [playingEvidence, setPlayingEvidence] = useState<Evidence | null>(null);

    const isAudio = (fileType: string, fileName: string) => {
        return fileType?.toLowerCase().includes('audio') || 
               fileName?.toLowerCase().endsWith('.wav') || 
               fileName?.toLowerCase().endsWith('.mp3') ||
               fileName?.toLowerCase().endsWith('.m4a');
    };

    const loadEvidence = async () => {
        setLoading(true);
        try {
            let data: Evidence[] = [];
            if (caseId) {
                data = await getEvidenceByCaseId(caseId);
            } else if (suspectId) {
                data = await getEvidenceBySuspectId(suspectId);
            } else {
                data = await getEvidenceList({});
            }
            setEvidence(data);
        } catch (error) {
            toast.error('فشل تحميل المرفقات الأدلة');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadEvidence();
    }, [caseId, suspectId]);

    const handleDelete = async (id: string) => {
        if (!confirm('هل أنت متأكد من حذف هذا الدليل؟')) return;

        try {
            const result = await deleteEvidence(id);
            if (result.succeeded) {
                toast.success('تم حذف الدليل بنجاح');
                loadEvidence();
            } else {
                toast.error(result.message || 'فشل حذف الدليل');
            }
        } catch (error) {
            toast.error('حدث خطأ أثناء الحذف');
        }
    };

    if (loading) {
        return (
            <div className="flex justify-center items-center py-12">
                <Loader2 className="h-8 w-8 animate-spin text-primary" />
            </div>
        );
    }

    return (
        <Card>
            <CardHeader>
                <div className="flex items-center justify-between">
                    <CardTitle className="flex items-center gap-2">
                        <FileText className="h-5 w-5" />
                        الأدلة والمرفقات
                    </CardTitle>
                    {onAddEvidence && (
                        <Button variant="outline" onClick={onAddEvidence}>
                            إضافة دليل
                        </Button>
                    )}
                </div>
            </CardHeader>
            <CardContent>
                {evidence.length > 0 ? (
                    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
                        {evidence.map((item) => (
                            <div key={item.id} className="relative group rounded-lg border p-4 bg-gray-50 dark:bg-zinc-900 transition-all hover:shadow-md">
                                <div className="flex items-start justify-between mb-3">
                                    <div className="p-2 bg-blue-100 dark:bg-blue-900/30 rounded-lg">
                                        <FileIcon className="h-6 w-6 text-blue-600 dark:text-blue-400" />
                                    </div>
                                    <div className="flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
                                        <Button variant="ghost" size="icon" className="h-8 w-8" asChild>
                                            <a href={item.downloadUrl} download target="_blank" rel="noopener noreferrer">
                                                <Download className="h-4 w-4" />
                                            </a>
                                        </Button>
                                        {isAudio(item.fileType, item.fileName) && (
                                            <Button 
                                                variant="ghost" 
                                                size="icon" 
                                                className="h-8 w-8 text-green-600 hover:text-green-700 hover:bg-green-100"
                                                onClick={() => setPlayingEvidence(item)}
                                            >
                                                <Play className="h-4 w-4" />
                                            </Button>
                                        )}
                                        <Button 
                                            variant="ghost" 
                                            size="icon" 
                                            className="h-8 w-8 text-destructive hover:text-destructive hover:bg-destructive/10"
                                            onClick={() => handleDelete(item.id)}
                                        >
                                            <Trash2Icon className="h-4 w-4" />
                                        </Button>
                                    </div>
                                </div>
                                <div className="space-y-1">
                                    <h4 className="font-semibold truncate" title={item.fileName}>
                                        {item.fileName}
                                    </h4>
                                    <div className="flex items-center gap-2 text-xs text-muted-foreground">
                                        <Badge variant="secondary" className="text-[10px] px-1 py-0 h-4">
                                            {item.fileType}
                                        </Badge>
                                        <span>{new Date(item.createdAt).toLocaleDateString('ar-SA')}</span>
                                    </div>
                                    {item.description && (
                                        <p className="text-sm text-muted-foreground line-clamp-2 mt-2" title={item.description}>
                                            {item.description}
                                        </p>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-12 border-2 border-dashed rounded-xl">
                        <FileText className="h-12 w-12 text-muted-foreground mx-auto mb-4 opacity-20" />
                        <p className="text-muted-foreground">لا توجد أدلة مرفوعة حالياً</p>
                    </div>
                )}
            </CardContent>

            <Dialog open={!!playingEvidence} onOpenChange={(open) => !open && setPlayingEvidence(null)}>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>استماع للتسجيل: {playingEvidence?.fileName}</DialogTitle>
                    </DialogHeader>
                    <div className="flex justify-center p-4">
                        {playingEvidence && (
                            <audio controls src={playingEvidence.downloadUrl} className="w-full" autoPlay>
                                متصفحك لا يدعم تشغيل الملفات الصوتية.
                            </audio>
                        )}
                    </div>
                </DialogContent>
            </Dialog>
        </Card>
    );
}
