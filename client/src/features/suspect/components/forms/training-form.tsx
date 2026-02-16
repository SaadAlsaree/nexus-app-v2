import z from 'zod';
import { TrainingCourseDetail, CourseTypeLabels } from '../../types/training';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createTrainingCourse, updateTrainingCourse } from '../../api/training.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';


const trainingFormSchema = z.object({
    courseType: z.string().min(1, { message: 'Course type is required.' }),
    location: z.string().optional(),
    trainerName: z.string().optional(),
    classmates: z.string().optional()
});

export default function TrainingForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: TrainingCourseDetail; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const defaultValues = {
        courseType: initialData?.courseType?.toString() || '',
        location: initialData?.location || '',
        trainerName: initialData?.trainerName || '',
        classmates: initialData?.classmates || ''
    };
    useEffect(() => {        
        if (initialData) {
            form.reset({
                courseType: initialData.courseType?.toString() || '',
                location: initialData.location || '',
                trainerName: initialData.trainerName || '',
                classmates: initialData.classmates || ''
            });
        }
    }, [initialData]);
    const form = useForm<z.infer<typeof trainingFormSchema>>({
        resolver: zodResolver(trainingFormSchema),
        defaultValues: defaultValues
    });

    const courseTypeOptions = Object.entries(CourseTypeLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof trainingFormSchema>) => {
        try {
            setIsLoading(true);
            const { courseType, ...restValues } = values;
            if (initialData) {
                await updateTrainingCourse({ trainingCourseId: initialData.id, courseType: Number(courseType), ...restValues });
                toast.success('تم تحديث الدورة التدريبية بنجاح');
                onOpenChange(false);
                return;
            }
        console.log('Submitted values:', values);
        await createTrainingCourse({ suspectId: id, courseType: Number(courseType), ...restValues });
        toast.success('تمت إضافة الدورة التدريبية بنجاح');
        onOpenChange(false);
        } catch (error) {
        console.error('Error adding training course:', error);
        toast.error(`فشل في إضافة الدورة التدريبية: ${error}`);
        } finally {
            setIsLoading(false);
        }
    };
    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent >
                <DialogHeader dir='rtl'>
                    <DialogTitle>{title}</DialogTitle>
                </DialogHeader>
                <Form form={form} onSubmit={form.handleSubmit(onSubmit)} className='space-y-4 gap-3'>
                   <div className='grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-2'>
                    <FormSelect
                        label='نوع الدورة'
                        name='courseType'
                        control={form.control}
                        placeholder='اختر نوع الدورة'
                        required
                        options={courseTypeOptions}
                    />
                    <FormInput
                        label='الموقع'
                        name='location'
                        control={form.control}
                    />
                    <FormInput
                        label='اسم المدرب'
                        name='trainerName'
                        control={form.control}
                    />
                   </div> 
                   <FormTextarea
                        label='زملاء الدورة'
                        name='classmates'
                        control={form.control}
                        config={{
                            rows: 10
                        }}
                    />
                   <Button type='submit' disabled={isLoading}>{isLoading ? <Spinner /> : 'حفظ'}</Button>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
