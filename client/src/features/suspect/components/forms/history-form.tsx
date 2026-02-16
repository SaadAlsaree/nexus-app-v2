import z from 'zod';
import { LifeHistory } from '../../types/suspect';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createLifeHistory, updateLifeHistory } from '../../api/suspect.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';


const historyFormSchema = z.object({
    educationLevel: z.string().min(1, { message: 'Education level is required.' }),
    schoolsAttended: z.string().optional(),
    civilianJobs: z.string().optional(),
    radicalizationStory: z.string().optional()
});

export default function HistoryForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: LifeHistory; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const defaultValues = {
        educationLevel: initialData?.educationLevel || '',
        schoolsAttended: initialData?.schoolsAttended || '',
        civilianJobs: initialData?.civilianJobs || '',
        radicalizationStory: initialData?.radicalizationStory || ''
    };
    useEffect(() => {        
        if (initialData) {
            form.reset({
                educationLevel: initialData.educationLevel || '',
                schoolsAttended: initialData.schoolsAttended || '',
                civilianJobs: initialData.civilianJobs || '',
                radicalizationStory: initialData.radicalizationStory || ''
            });
        }
    }, [initialData]);
    const form = useForm<z.infer<typeof historyFormSchema>>({
        resolver: zodResolver(historyFormSchema),
        defaultValues: defaultValues
    });

    const onSubmit = async (values: z.infer<typeof historyFormSchema>) => {
        try {
            setIsLoading(true);
            if (initialData) {
                await updateLifeHistory({ lifeHistoryId: initialData.id, ...values });
                toast.success('تم تحديث السيرة الحياتية بنجاح');
                onOpenChange(false);
                return;
            }
        console.log('Submitted values:', values);
        await createLifeHistory({ suspectId: id, ...values });
        toast.success('تمت إضافة السيرة الحياتية بنجاح');
        onOpenChange(false);
        } catch (error) {
        console.error('Error adding life history:', error);
        toast.error(`فشل في إضافة السيرة الحياتية: ${error}`);
        } finally {
            setIsLoading(false);
        }
    };
    // Form implementation will go here
    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent >
                <DialogHeader dir='rtl'>
                    <DialogTitle>{title}</DialogTitle>
                </DialogHeader>
                <Form form={form} onSubmit={form.handleSubmit(onSubmit)} className='space-y-4 gap-3'>
                   <div className='grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-2'>
                    <FormInput
                    label='المستوى التعليمي'
                    name='educationLevel'
                    control={form.control}
                    />
                    <FormInput
                    label='المدارس التي درس فيها'
                    name='schoolsAttended'
                    control={form.control}
                    />
                    <FormInput
                    label='الوظائف المدنية' 
                    name='civilianJobs'
                    control={form.control}
                    />
                    
                   </div> 
                   <FormTextarea
                    label='قصة التطرف'
                    name='radicalizationStory'
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