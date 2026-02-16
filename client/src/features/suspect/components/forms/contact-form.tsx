import z from 'zod';
import { Contact } from '../../types/suspect';
import { ContactTypeLabels } from '../../types/contact';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createContact, updateContact } from '../../api/contact.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';


const contactFormSchema = z.object({
    type: z.string().min(1, { message: 'Contact type is required.' }),
    value: z.string().min(1, { message: 'Contact value is required.' }),
    ownerName: z.string().optional()
});

export default function ContactForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: Contact; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const defaultValues = {
        type: initialData?.type?.toString() || '',
        value: initialData?.value || '',
        ownerName: initialData?.ownerName || ''
    };
    useEffect(() => {        
        if (initialData) {
            form.reset({
                type: initialData.type?.toString() || '',
                value: initialData.value || '',
                ownerName: initialData.ownerName || ''
            });
        }
    }, [initialData]);
    const form = useForm<z.infer<typeof contactFormSchema>>({
        resolver: zodResolver(contactFormSchema),
        defaultValues: defaultValues
    });

    const contactTypeOptions = Object.entries(ContactTypeLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof contactFormSchema>) => {
        try {
            setIsLoading(true);
            if (initialData) {
                await updateContact({ 
                    contactId: initialData.id, 
                    type: Number(values.type),
                    value: values.value,
                    ownerName: values.ownerName
                });
                toast.success('تم تحديث معلومات الاتصال بنجاح');
                onOpenChange(false);
                return;
            }
        console.log('Submitted values:', values);
        await createContact({ 
            suspectId: id, 
            type: Number(values.type),
            value: values.value,
            ownerName: values.ownerName
        });
        toast.success('تمت إضافة معلومات الاتصال بنجاح');
        onOpenChange(false);
        } catch (error) {
        console.error('Error adding contact:', error);
        toast.error(`فشل في إضافة معلومات الاتصال: ${error}`);
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
                        label='نوع الاتصال'
                        name='type'
                        control={form.control}
                        placeholder='اختر نوع الاتصال'
                        required
                        options={contactTypeOptions}
                    />
                    <FormInput
                        label='القيمة'
                        name='value'
                        control={form.control}
                        required
                    />
                    <FormInput
                        label='اسم المالك (اختياري)'
                        name='ownerName'
                        control={form.control}
                    />
                   </div> 
                   <Button type='submit' disabled={isLoading}>{isLoading ? <Spinner /> : 'حفظ'}</Button>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
