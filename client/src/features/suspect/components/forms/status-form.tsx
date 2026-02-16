import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import z from 'zod';
import { SuspectStatus, SuspectStatusLabels } from '../../types/suspect';
import { updateSuspectStatus } from '../../api/suspect.service';
import { toast } from 'sonner';
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Form } from '@/components/ui/form';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Spinner } from '@/components/ui/spinner';
import { FormTextarea } from '@/components/forms/form-textarea';

const statusFormSchema = z.object({
    status: z.string(),
    note: z.string().optional()
});

export default function StatusForm(
    {id, initialStatus, title, onOpenChange, open} : 
    {id: string; initialStatus?: string; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const defaultValues = {
        status: initialStatus || '',
        note: ''
    };
    useEffect(() => {
        if (initialStatus) {
            form.reset({
                status: initialStatus || '',
                note: ''
            });
        }
    }, [initialStatus]);
    const form = useForm<z.infer<typeof statusFormSchema>>({
        resolver: zodResolver(statusFormSchema),
        defaultValues: defaultValues
    });

    const statusOptions = Object.entries(SuspectStatusLabels).map(([value, label]) => ({
        label: label,
        value: value
      }));
      
    const onSubmit = async (values: z.infer<typeof statusFormSchema>) => {
        try {
            setIsLoading(true);
            await updateSuspectStatus({ suspectId: id, newStatus: Number(values.status) as SuspectStatus, notes: values.note });
            toast.success('تم تحديث حالة المشتبه به بنجاح');
            onOpenChange(false);
        } catch (error) {
            console.error('Error updating suspect status:', error);
            toast.error(`فشل في تحديث حالة المشتبه به: ${error}`);
        }
        finally {
            setIsLoading(false);
        }
    };
    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent >
                <DialogHeader dir='rtl'>
                    <DialogTitle>{title}</DialogTitle>
                </DialogHeader>
                <Form 
                form={form}
                onSubmit={form.handleSubmit(onSubmit)}
                className='space-y-2'
                >
                    <FormSelect
                        control={form.control}
                        className='w-full'
                        name='status'
                        label='الحالة'
                        placeholder='اختر الحالة'
                        required
                        options={statusOptions}
                    />
                    <FormTextarea
                        label='ملاحظات'
                        name='note'
                        control={form.control}
                        placeholder='أدخل ملاحظات إضافية (اختياري)'
                        className='w-full'
                    />
                    <Button type='submit' disabled={isLoading}>{isLoading ? <Spinner /> : 'حفظ'}</Button>
                </Form>

            </DialogContent>
        </Dialog>
    );
}