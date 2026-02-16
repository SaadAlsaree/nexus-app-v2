
'use client';

import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { 
  InterrogationSession, 
  InterrogationType, 
  InterrogationTypeLabels, 
  InterrogationOutcome, 
  InterrogationOutcomeLabels 
} from '../types/interrogation-session';
import { zodResolver } from '@hookform/resolvers/zod';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { createInterrogationSession, updateInterrogationSession } from '../api/interrogation-session.service';
import { useState } from 'react';
import { format } from 'date-fns';
import { toast } from 'sonner';
import { Spinner } from '@/components/ui/spinner';
import { Checkbox } from '@/components/ui/checkbox';
import { FormField, FormItem, FormLabel, FormControl } from '@/components/ui/form';
import { Suspect } from '../../suspect/types/suspect';
import { CaseSummary } from '../../case/types/case';
import { AudioRecorder } from './audio-recorder';
import { Data as AIData, Choice } from '../api/ai.service';


const formSchema = z.object({
  suspectId: z.string().min(1, { message: 'معرف المشتبه به مطلوب' }),
  caseId: z.string().min(1, { message: 'معرف القضية مطلوب' }),
  sessionDate: z.date({ message: 'تاريخ الجلسة مطلوب' }),
  interrogatorName: z.string().min(3, { message: 'اسم المحقق يجب أن يكون 3 أحرف على الأقل' }),
  location: z.string().min(1, { message: 'الموقع مطلوب' }),
  sessionType: z.string(),
  content: z.string().min(10, { message: 'محتوى الجلسة يجب أن يكون 10 أحرف على الأقل' }),
  summaryContent: z.string().min(10, { message: 'ملخص الجلسة يجب أن يكون 10 أحرف على الأقل' }),
  outcome: z.string(),
  investigatorNotes: z.string().optional(),
  isRatifiedByJudge: z.boolean(),
});

interface InterrogationSessionFormProps {
  initialData: InterrogationSession | null;
  pageTitle: string;
  suspectId?: string;
  caseId?: string;
  suspects?: Suspect[];
  cases?: CaseSummary[];
}

export default function InterrogationSessionForm({
  initialData,
  pageTitle,
  suspectId,
  caseId,
  suspects = [],
  cases = []
}: InterrogationSessionFormProps) {
  const defaultValues = {
    suspectId: initialData?.suspectId || suspectId || '',
    caseId: initialData?.caseId || caseId || '',
    sessionDate: initialData?.sessionDate ? new Date(initialData.sessionDate) : new Date(),
    interrogatorName: initialData?.interrogatorName || '',
    location: initialData?.location || '',
    sessionType: String(initialData?.sessionType ?? InterrogationType.InitialInquiry),
    content: initialData?.content || '',
    summaryContent: initialData?.summaryContent || '',
    outcome: String(initialData?.outcome ?? InterrogationOutcome.Denial),
    investigatorNotes: initialData?.investigatorNotes || '',
    isRatifiedByJudge: initialData?.isRatifiedByJudge ?? false,
  };


  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: defaultValues
  });

  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);

  async function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      setIsLoading(true);
      const safeDate = format(values.sessionDate, "yyyy-MM-dd");
      const formattedValues = {
        ...values,
        sessionType: Number(values.sessionType) as InterrogationType,
        outcome: Number(values.outcome) as InterrogationOutcome,
        sessionDate: safeDate,
      };

      if (initialData) {
        const res = await updateInterrogationSession({ ...formattedValues, id: initialData.id } as any);
        if (res.succeeded) {
          toast.success('تم تحديث جلسة الاستجواب بنجاح');
          setIsLoading(false);
          router.push(`/suspect/${values.suspectId}`);
        }
      } else {
        const res = await createInterrogationSession(formattedValues as any);
        if (res.succeeded) {
          toast.success('تم إنشاء جلسة الاستجواب بنجاح');
          setIsLoading(false);
          router.push(`/suspect/${values.suspectId}`);
        }
      }
    } catch (error) {
      toast.error(`حدث خطأ: ${(error as Error).message}`);
      setIsLoading(false);
    }
  }

  const handleTranscription = (result: AIData) => {
    const formattedText = result.segments
      .map(segment => {
        const speakerRole = segment.speaker === '0' ? 'المحقق' : 'المشتبه به';
        return `${speakerRole}: ${segment.text}`;
      })
      .join('\n');
    
    const currentContent = form.getValues('content');
    const newContent = currentContent 
      ? `${currentContent}\n\n--- [تفريغ صوتي جديد] ---\n${formattedText}`
      : formattedText;
      
    form.setValue('content', newContent);
    toast.success('تمت إضافة التفريغ الصوتي بنجاح');
  };

  const handleSummary = (summary: Choice[]) => {
    const formattedText = summary
      .map(choice => {
        return `${choice?.message?.content}`;
      })
      .join('\n');
    
    const currentContent = form.getValues('summaryContent');
    const newContent = currentContent 
      ? `${currentContent}\n\n--- [ملخص جديد] ---\n${formattedText}`
      : formattedText;
      
    form.setValue('summaryContent', newContent);
    toast.success('تمت إضافة الملخص بنجاح');
  };

  const sessionTypeOptions = Object.entries(InterrogationTypeLabels).map(([value, label]) => ({
    label: label,
    value: value
  }));

  const outcomeOptions = Object.entries(InterrogationOutcomeLabels).map(([value, label]) => ({
    label: label,
    value: value
  }));

  const suspectOptions = suspects.map(s => ({
    label: s.fullName,
    value: s.id
  }));

  const caseOptions = cases.map(c => ({
    label: `${c.title} (${c.caseFileNumber})`,
    value: c.id
  }));

  return (
    <Card className='mx-auto w-full' dir="rtl">
      <CardHeader>
        <CardTitle className='text-right text-2xl font-bold'>
          {pageTitle}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <Form
          form={form}
          onSubmit={form.handleSubmit(onSubmit)}
          className='space-y-8'
        >
          <div className='grid grid-cols-1 gap-6 md:grid-cols-2'>
            <FormSelect
               control={form.control}
               name='suspectId'
               label='المشتبه به'
               placeholder='اختر المشتبه به'
               required
               options={suspectOptions}
              //  disabled={!!suspectId && !initialData}
            />
            <FormSelect
               control={form.control}
               name='caseId'
               label='القضية'
               placeholder='اختر القضية'
               required
               options={caseOptions}
               disabled={!!caseId && !initialData}
            />
            <FormDatePicker
              control={form.control}
              name='sessionDate'
              label='تاريخ الجلسة'
            />
            <FormInput
              control={form.control}
              name='interrogatorName'
              label='اسم المحقق'
              placeholder='أدخل اسم المحقق'
              required
            />
            <FormInput
              control={form.control}
              name='location'
              label='الموقع'
              placeholder='أدخل موقع الجلسة'
              required
            />
            <FormSelect
              control={form.control}
              className='w-full'
              name='sessionType'
              label='نوع الجلسة'
              placeholder='اختر نوع الجلسة'
              required
              options={sessionTypeOptions}
            />
            <FormSelect
              control={form.control}
              className='w-full'
              name='outcome'
              label='النتيجة'
              placeholder='اختر نتيجة الجلسة'
              required
              options={outcomeOptions}
            />
            <FormField
              control={form.control}
              name='isRatifiedByJudge'
              render={({ field }) => (
                <FormItem className='flex flex-row items-start space-x-3 space-x-reverse space-y-0 rounded-md border p-4'>
                  <FormControl>
                    <Checkbox
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <div className='space-y-1 leading-none'>
                    <FormLabel>مصدق من القاضي</FormLabel>
                  </div>
                </FormItem>
              )}
            />
          </div>

          <div className='grid grid-cols-1 gap-6'>
            <div className="space-y-2">
              <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 text-right block">
                تسجيل وتحليل الصوت (اختياري)
              </label>
              <AudioRecorder 
                onResult={handleTranscription} 
                caseId={form.watch('caseId')}
                suspectId={form.watch('suspectId')}
                setAiSummary={handleSummary}
                aiSummerArg={form.watch('content')}
              />
            </div>

            <FormTextarea
              control={form.control}
              name='content'
              label='محتوى الجلسة'
              placeholder='أدخل محتوى الجلسة...'
              config={{ rows: 6 }}
            />
            <FormTextarea
              control={form.control}
              name='summaryContent'
              label='ملخص الجلسة'
              placeholder='أدخل ملخص الجلسة...'
              config={{ rows: 4 }}
            />
            <FormTextarea
              control={form.control}
              name='investigatorNotes'
              label='ملاحظات المحقق'
              placeholder='أدخل ملاحظات المحقق (اختياري)...'
              config={{ rows: 3 }}
            />
          </div>

          <div className="flex justify-start">
            <Button type='submit' disabled={isLoading}>
              {isLoading ? <Spinner className="mr-2 h-4 w-4" /> : 'حفظ'}
            </Button>
          </div>
        </Form>
      </CardContent>
    </Card>
  );
}
