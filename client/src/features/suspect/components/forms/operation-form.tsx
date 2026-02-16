import z from 'zod';
import { Operation } from '../../types/suspect';
import { OperationTypeLabels } from '../../types/operation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createOperation, updateOperation } from '../../api/operation.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { format } from 'date-fns';



const operationFormSchema = z.object({
    operationType: z.string().min(1, { message: 'Operation type is required.' }),
    date: z.union([z.date(), z.string()]).optional().nullable(),
    location: z.string().optional(),
    roleInOp: z.string().optional(),
    associates: z.string().optional()
});


export default function OperationForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: Operation; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const defaultValues = {
        operationType: initialData?.operationType?.toString() || '',
        date: initialData?.date ? new Date(initialData?.date) : null,
        location: initialData?.location || '',
        roleInOp: initialData?.roleInOp || '',
        associates: initialData?.associates || ''
    };
    useEffect(() => {        
        if (initialData) {
            form.reset({
                operationType: initialData.operationType?.toString() || '',
                date: initialData.date ? new Date(initialData.date) : null,
                location: initialData.location || '',
                roleInOp: initialData.roleInOp || '',
                associates: initialData.associates || ''
            });
        }
    }, [initialData]);
    const form = useForm<z.infer<typeof operationFormSchema>>({
        resolver: zodResolver(operationFormSchema),
        defaultValues: defaultValues
    });


    const operationTypeOptions = Object.entries(OperationTypeLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof operationFormSchema>) => {
        try {
            setIsLoading(true); 
            const safeDate = values.date ? format(new Date(values.date), 'yyyy-MM-dd') : undefined;
            const { operationType, ...rest } = values;
            
            if (initialData) {
                await updateOperation({ 
                    operationId: initialData.id, 
                    operationType: Number(operationType), 
                    ...rest, 
                    date: safeDate 
                });
                toast.success('تم تحديث العملية بنجاح');
                onOpenChange(false);
                return;
            }

            await createOperation({ 
                suspectId: id, 
                operationType: Number(operationType), 
                ...rest, 
                date: safeDate 
            });
            toast.success('تمت إضافة العملية بنجاح');
            onOpenChange(false);
        } catch (error) {
        console.error('Error adding operation:', error);
        toast.error(`فشل في إضافة العملية: ${error}`);
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
                        label='نوع العملية'
                        name='operationType'
                        control={form.control}
                        placeholder='اختر نوع العملية'
                        required
                        options={operationTypeOptions}
                    />
                    <FormDatePicker
                        label='التاريخ'
                        name='date'
                        control={form.control}
                    />
                    <FormInput
                        label='الموقع'
                        name='location'
                        control={form.control}
                    />
                    <FormInput
                        label='الدور في العملية' 
                        name='roleInOp'
                        control={form.control}
                    />
                   </div> 
                   <FormTextarea
                        label='المرافقون'
                        name='associates'
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
