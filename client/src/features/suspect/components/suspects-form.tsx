
'use client';
import { Input } from "@/components/ui/input"
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import {
  SuspectDetails,
  SuspectStatus,
  SuspectStatusLabels,
  MaritalStatus,
  MaritalStatusLabels
} from '../types/suspect';
import { zodResolver } from '@hookform/resolvers/zod';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { createSuspect, updateSuspect } from '../api/suspect.service';
import { useState } from 'react';
import { format } from 'date-fns';
import { toast } from 'sonner';
import { Spinner } from '@/components/ui/spinner';

const formSchema = z.object({
  firstName: z.string().min(2, { message: 'الاسم الأول يجب أن يكون حرفين على الأقل' }),
  secondName: z.string().min(2, { message: 'اسم الأب مطلوب' }),
  thirdName: z.string().min(2, { message: 'اسم الجد مطلوب' }),
  fourthName: z.string().optional(),
  fivthName: z.string().optional(),
  codeNum: z.string().optional(),
  kunya: z.string().optional(),
  motherName: z.string().optional(),
  dateOfBirth: z.date().optional(),
  placeOfBirth: z.string().optional(),
  tribe: z.string().optional(),
  maritalStatus: z.string(),
  wivesCount: z.number().min(0, { message: 'عدد الزوجات يجب أن يكون 0 أو أكثر' }),
  childrenCount: z.number().min(0, { message: 'عدد الأطفال يجب أن يكون 0 أو أكثر' }),
  legalArticle: z.string().optional(),
  healthStatus: z.string().optional(),
  photoUrl: z.string().optional(),
  status: z.string(),
});

export default function SuspectsForm({
  initialData,
  pageTitle
}: {
  initialData: SuspectDetails | null;
  pageTitle: string;
}) {
  const defaultValues = {
    firstName: initialData?.firstName || '',
    secondName: initialData?.secondName || '',
    thirdName: initialData?.thirdName || '',
    fourthName: initialData?.fourthName || '',
    fivthName: initialData?.fivthName || '',
    codeNum: initialData?.codeNum || '',
    kunya: initialData?.kunya || '',
    motherName: initialData?.motherName || '',
    dateOfBirth: initialData?.dateOfBirth ? new Date(initialData.dateOfBirth) : undefined,
    placeOfBirth: initialData?.placeOfBirth || '',
    tribe: initialData?.tribe || '',
    maritalStatus: String(initialData?.maritalStatus ?? MaritalStatus.Single),
    wivesCount: initialData?.wivesCount ?? 0,
    childrenCount: initialData?.childrenCount ?? 0,
    legalArticle: initialData?.legalArticle || '',
    healthStatus: initialData?.healthStatus || '',
    photoUrl: initialData?.photoUrl || '',
    status: String(initialData?.currentStatus ?? SuspectStatus.Unknown),
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
      
      const safeDate = values.dateOfBirth ? format(values.dateOfBirth, 'yyyy-MM-dd') : undefined;

      const formattedValues = {
        firstName: values.firstName,
        secondName: values.secondName,
        thirdName: values.thirdName,
        fourthName: values.fourthName || undefined,
        fivthName: values.fivthName || undefined,
        kunya: values.kunya || undefined,
        motherName: values.motherName || undefined,
        dateOfBirth: safeDate,
        codeNum: values.codeNum || undefined,
        placeOfBirth: values.placeOfBirth || undefined,
        tribe: values.tribe || undefined,
        maritalStatus: Number(values.maritalStatus) as MaritalStatus,
        wivesCount: values.wivesCount ,
        childrenCount: values.childrenCount,
        legalArticle: values.legalArticle || undefined,
        healthStatus: values.healthStatus || undefined,
        photoUrl: values.photoUrl || undefined,
        status: Number(values.status) as SuspectStatus,
      };

      if (initialData) {
        const res = await updateSuspect({ ...formattedValues, suspectId: initialData.id });
        console.log(res.data);
        if (res.succeeded) {
          toast.success('تم تحديث بيانات المشتبه به بنجاح');
          setIsLoading(false);
          router.push(`/suspect/${initialData.id}`);
        } else {
          toast.error(res.message || 'حدث خطأ أثناء التحديث');
          setIsLoading(false);
        }
      } else {
        const res = await createSuspect({ ...formattedValues});
        if (res.succeeded) {
          toast.success('تم إنشاء المشتبه به بنجاح');
          setIsLoading(false);
          router.push(`/`);
        } else {
          toast.error(res.message || 'حدث خطأ أثناء الإنشاء');
          setIsLoading(false);
        }
      }
    } catch (error) {
      toast.error(`حدث خطأ: ${(error as Error).message}`);
      setIsLoading(false);
    }
  }

  const statusOptions = Object.entries(SuspectStatusLabels).map(([value, label]) => ({
    label: label,
    value: value
  }));

  const maritalStatusOptions = Object.entries(MaritalStatusLabels).map(([value, label]) => ({
    label: label,
    value: value
  }));

  return (
    <Card className='mx-auto w-full bg-zinc-100 dark:bg-zinc-800' dir="rtl" >
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
          {/* Personal Information Section */}
          <div className="space-y-4">
            <h3 className="text-lg font-semibold border-b pb-2">المعلومات الشخصية</h3>
            <div className='grid grid-cols-1 gap-6 md:grid-cols-6'>
              <FormInput
                control={form.control}
                name='firstName'
                label='الاسم الأول'
                placeholder='أدخل الاسم الأول'
                required
              />
              <FormInput
                control={form.control}
                name='secondName'
                label='اسم الأب'
                placeholder='أدخل اسم الأب'
                required
              />
              <FormInput
                control={form.control}
                name='thirdName'
                label='اسم الجد'
                placeholder='أدخل اسم الجد'
                required
              />
              <FormInput
                control={form.control}
                name='fourthName'
                label='الاسم الرابع'
                placeholder='أدخل الاسم الرابع (اختياري)'
              />
              <FormInput
                control={form.control}
                name='fivthName'
                label='الاسم الخامس'
                placeholder='أدخل الاسم الخامس (اختياري)'
              />
              <FormInput
                control={form.control}
                name='kunya'
                label='الكنية'
                placeholder='أدخل الكنية (اختياري)'
              />
            </div>
          </div>

          {/* Additional Information Section */}
          <div className="space-y-4">
            <h3 className="text-lg font-semibold border-b pb-2">معلومات إضافية</h3>
            <div className='grid grid-cols-1 gap-6 md:grid-cols-12'>
              <FormInput
                control={form.control}
                className='col-span-4'
                name='motherName'
                label='اسم الأم'
                placeholder='أدخل اسم الأم (اختياري)'
              />
              <FormField
                control={form.control}
                name='dateOfBirth'
                render={({ field }) => (
                  <FormItem className='col-span-4'>
                    <FormLabel>تاريخ الميلاد</FormLabel>
                    <FormControl>
                      <Input
                        type='date'
                        placeholder='اختر تاريخ الميلاد'
                        value={field.value ? format(field.value, 'yyyy-MM-dd') : ''}
                        onChange={(e) => {
                          field.onChange(e.target.value ? new Date(e.target.value) : undefined);
                        }}
                        onBlur={field.onBlur}
                        name={field.name}
                        ref={field.ref}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
             
              <FormInput
                 className='col-span-4'
                control={form.control}
                name='placeOfBirth'
                label='مكان الولادة'
                placeholder='أدخل مكان الولادة (اختياري)'
              />
              <FormInput
                className='col-span-3'
                control={form.control}
                name='tribe'
                label='العشيرة/القبيلة'
                placeholder='أدخل العشيرة (اختياري)'
              />
            
               <FormSelect
               className='col-span-3'
                control={form.control}
                name='maritalStatus'
                label='الحالة الاجتماعية'
                placeholder='اختر الحالة الاجتماعية'
                options={maritalStatusOptions}
              />
             
              <FormInput
                className='col-span-3'
                control={form.control}
                name='wivesCount'
                label='عدد الزوجات'
                placeholder='أدخل عدد الزوجات'
                type='number'
              />
              <FormInput
                className='col-span-3'
                control={form.control}
                name='childrenCount'
                label='عدد الأطفال'
                placeholder='أدخل عدد الأطفال'
                type='number'
              />
            </div>
          </div>

          {/* Legal and Health Information Section */}
          <div className="space-y-4">
            <h3 className="text-lg font-semibold border-b pb-2">المعلومات القانونية والصحية</h3>
            <div className='grid grid-cols-1 gap-6 md:grid-cols-2'>
              <FormInput
                control={form.control}
                name='legalArticle'
                label='المادة القانونية'
                placeholder='أدخل المادة القانونية (اختياري)'
              />
              <FormInput
                control={form.control}
                name='codeNum'
                label='الرقم الاحصائي'
                placeholder='أدخل الرقم الاحصائي (اختياري)'
              />
              <FormInput
                control={form.control}
                name='healthStatus'
                label='الحالة الصحية'
                placeholder='أدخل الحالة الصحية (اختياري)'
              />
              <FormSelect
                control={form.control}
                className='w-full'
                name='status'
                label='الحالة الحالية'
                placeholder='اختر الحالة'
                required
                options={statusOptions}
              />
              <FormInput
                control={form.control}
                name='photoUrl'
                label='رابط الصورة'
                placeholder='أدخل رابط الصورة (اختياري)'
              />
            </div>
          </div>

          <div className="flex justify-start">
            <Button type='submit' disabled={isLoading}>
              {isLoading ? <Spinner className=" h-4 w-4" /> : 'حفظ'}
            </Button>
          </div>
        </Form>
      </CardContent>
    </Card>
  );
}
