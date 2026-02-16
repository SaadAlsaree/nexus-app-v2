import z from 'zod';
import { Address } from '../../types/suspect';
import { AddressTypeLabels } from '../../types/address';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createAddress, updateAddress } from '../../api/address.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';
import { useRouter } from 'next/navigation';


const addressFormSchema = z.object({
    type: z.string().min(1, { message: 'Address type is required.' }),
    city: z.string().min(1, { message: 'City is required.' }),
    district: z.string().min(1, { message: 'District is required.' }),
    details: z.string().optional(),
    gpsCoordinates: z.string().optional()
});

export default function AddressForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: Address; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const router = useRouter();
    const defaultValues = {
        type: initialData?.type?.toString() || '',
        city: initialData?.city || '',
        district: initialData?.district || '',
        details: initialData?.details || '',
        gpsCoordinates: initialData?.gpsCoordinates || ''
    };
    useEffect(() => {        
        if (initialData) {
            form.reset({
                type: initialData.type?.toString() || '',
                city: initialData.city || '',
                district: initialData.district || '',
                details: initialData.details || '',
                gpsCoordinates: initialData.gpsCoordinates || ''
            });
        }
    }, [initialData]);
    const form = useForm<z.infer<typeof addressFormSchema>>({
        resolver: zodResolver(addressFormSchema),
        defaultValues: defaultValues
    });

    const addressTypeOptions = Object.entries(AddressTypeLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof addressFormSchema>) => {
        try {
            setIsLoading(true);
            if (initialData) {
                await updateAddress({ 
                    addressId: initialData.id, 
                    type: Number(values.type),
                    city: values.city,
                    district: values.district,
                    details: values.details,
                    gpsCoordinates: values.gpsCoordinates
                });
                toast.success('تم تحديث العنوان بنجاح');
               
                onOpenChange(false);
                return;
            }
        console.log('Submitted values:', values);
        await createAddress({ 
            suspectId: id, 
            type: Number(values.type),
            city: values.city,
            district: values.district,
            details: values.details,
            gpsCoordinates: values.gpsCoordinates
        });
        toast.success('تمت إضافة العنوان بنجاح');
   
        onOpenChange(false);
        } catch (error) {
        console.error('Error adding address:', error);
        toast.error(`فشل في إضافة العنوان: ${error}`);
        } finally {
            router.refresh();
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
                        label='نوع العنوان'
                        name='type'
                        control={form.control}
                        placeholder='اختر نوع العنوان'
                        required
                        options={addressTypeOptions}
                    />
                    <FormInput
                        label='المدينة'
                        name='city'
                        control={form.control}
                        required
                    />
                    <FormInput
                        label='الحي'
                        name='district'
                        control={form.control}
                        required
                    />
                    <FormInput
                        label='الإحداثيات (اختياري)'
                        name='gpsCoordinates'
                        control={form.control}
                    />
                   </div> 
                   <FormTextarea
                        label='التفاصيل (اختياري)'
                        name='details'
                        control={form.control}
                        config={{
                            rows: 5
                        }}
                    />
                   <Button type='submit' disabled={isLoading}>{isLoading ? <Spinner /> : 'حفظ'}</Button>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
