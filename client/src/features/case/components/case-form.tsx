
'use client';

import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Form } from '@/components/ui/form';
import { CaseDetails, CaseStatus, CaseStatusLabels, PriorityLevel, PriorityLevelLabels, UpdateCaseRequest } from '../types/case';
import { zodResolver } from '@hookform/resolvers/zod';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { createCase, updateCase } from '../api/case.service';
import { useState } from 'react';
import { format } from 'date-fns';
import { toast } from 'sonner';
import { Spinner } from '@/components/ui/spinner';

const formSchema = z.object({
  caseFileNumber: z.string().min(1, { message: 'رقم القضية مطلوب' }),
  title: z.string().min(3, { message: 'عنوان القضية يجب أن يكون 3 أحرف على الأقل' }),
  investigatingOfficer: z.string().min(3, { message: 'اسم ضابط التحقيق مطلوب' }),
  openDate: z.date({ message: 'تاريخ الفتح مطلوب' }),
  status: z.string(),
  priority: z.string(),
});

export default function CaseForm({
  initialData,
  pageTitle
}: {
  initialData: UpdateCaseRequest | null;
  pageTitle: string;
}) {
  const defaultValues = {
    caseFileNumber: initialData?.caseFileNumber || '',
    title: initialData?.title || '',
    investigatingOfficer: initialData?.investigatingOfficer || '',
    openDate: initialData?.openDate ? new Date(initialData.openDate) : new Date(),
    status: String(initialData?.status ?? CaseStatus.UnderInvestigation),
    priority: String(initialData?.priority ?? PriorityLevel.Medium),
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
      const safeDate = format(values.openDate, 'yyyy-MM-dd');
      const formattedValues = {
        ...values,
        status: Number(values.status) as CaseStatus,
        priority: Number(values.priority) as PriorityLevel,
        openDate: safeDate,
      };

      if (initialData) {
        const payload = { ...formattedValues, caseId: initialData.caseId, openDate: safeDate } as any;
        console.log(payload);
        const res = await updateCase(payload);
         if (res.succeeded) {
          toast.success('تم تحديث القضية بنجاح');
          setIsLoading(false);
          router.push(`/case/${initialData.caseId}`); 
        }
      } else {
        const res = await createCase({ ...formattedValues, openDate: safeDate } as any);
         if (res.succeeded) {
          toast.success('تم إنشاء القضية بنجاح');
          setIsLoading(false);
          router.push(`/cases`);
        }
      }
    } catch (error) {
      toast.error(`حدث خطأ: ${(error as Error).message}`);
      setIsLoading(false);
    }
  }

  const statusOptions = Object.entries(CaseStatusLabels).map(([value, label]) => ({
    label: label,
    value: value
  }));

  const priorityOptions = Object.entries(PriorityLevelLabels).map(([value, label]) => ({
     label: label,
     value: value
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
            <FormInput
              control={form.control}
              name='caseFileNumber'
              label='رقم القضية'
              placeholder='أدخل رقم القضية'
              required
            />
            <FormInput
              control={form.control}
              name='title'
              label='عنوان القضية'
              placeholder='أدخل عنوان القضية'
              required
            />
            <FormDatePicker
              control={form.control}
              name='openDate'
              label='تاريخ الفتح'
            />
            <FormInput
              control={form.control}
              name='investigatingOfficer'
              label='ضابط التحقيق'
              placeholder='أدخل اسم ضابط التحقيق'
              required
            />
            <FormSelect
              control={form.control}
              className='w-full'
              name='status'
              label='الحالة'
              placeholder='اختر الحالة'
              required
              options={statusOptions}
            />
             <FormSelect
              control={form.control}
              className='w-full'
              name='priority'
              label='الأولوية'
              placeholder='اختر الأولوية'
              required
              options={priorityOptions}
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
