import z from 'zod';
import { RelativesAndAssociate } from '../../types/suspect';
import { RelationshipTypeLabels, RelationshipRequest } from '../../types/relationship';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createRelationship, updateRelationship } from '../../api/relationship.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';


const relationshipFormSchema = z.object({
    firstName: z.string().min(1, { message: 'الاسم الأول مطلوب' }),
    secondName: z.string().optional(),
    thirdName: z.string().optional(),
    fourthName: z.string().optional(),
    fivthName: z.string().optional(),
    tribe: z.string().optional(),
    relationship: z.string().min(1, { message: 'صلة القرابة مطلوبة' }),
    notes: z.string().optional()
});

export default function RelationshipForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: RelativesAndAssociate; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    
    const getNameParts = (fullName: string | undefined) => {
        if (!fullName) return { firstName: '', secondName: '', thirdName: '', fourthName: '', fivthName: '', tribe: '' };
        const parts = fullName.split(' ');
        return {
            firstName: parts[0] || '',
            secondName: parts[1] || '',
            thirdName: parts[2] || '',
            fourthName: parts[3] || '',
            fivthName: parts.slice(4).join(' ') || '',
            tribe: '' // Tribe is usually separate or not easily splittable from fullName
        };
    };

    const defaultValues = {
        ...getNameParts(initialData?.fullName),
        relationship: initialData?.relationship?.toString() || '',
        notes: initialData?.notes || '',
        tribe: ''
    };

    const form = useForm<z.infer<typeof relationshipFormSchema>>({
        resolver: zodResolver(relationshipFormSchema),
        defaultValues: defaultValues
    });

    useEffect(() => {        
        if (initialData) {
            form.reset({
                ...getNameParts(initialData.fullName),
                relationship: initialData.relationship?.toString() || '',
                notes: initialData.notes || '',
                tribe: ''
            });
        }
    }, [initialData, form]);

    const relationshipTypeOptions = Object.entries(RelationshipTypeLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof relationshipFormSchema>) => {
        try {
            setIsLoading(true);
            const request: RelationshipRequest = {
                suspectId: id,
                firstName: values.firstName,
                secondName: values.secondName,
                thirdName: values.thirdName,
                fourthName: values.fourthName,
                fivthName: values.fivthName,
                tribe: values.tribe,
                relationship: Number(values.relationship),
                notes: values.notes
            };

            if (initialData) {
                await updateRelationship({ 
                    ...request,
                    relatedPersonId: initialData.id,
                });
                toast.success('تم تحديث العلاقة بنجاح');
            } else {
                await createRelationship(request);
                toast.success('تمت إضافة العلاقة بنجاح');
            }
            onOpenChange(false);
        } catch (error) {
            console.error('Error handling relationship:', error);
            toast.error(`حدث خطأ: ${error}`);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent className="max-w-2xl">
                <DialogHeader dir='rtl'>
                    <DialogTitle className="text-right">{title}</DialogTitle>
                </DialogHeader>
                <Form form={form} onSubmit={form.handleSubmit(onSubmit)} className='space-y-4'>
                   <div className='grid grid-cols-1 gap-4 md:grid-cols-3'>
                        <FormInput
                            label='الاسم الأول'
                            name='firstName'
                            control={form.control}
                            required
                        />
                        <FormInput
                            label='اسم الأب'
                            name='secondName'
                            control={form.control}
                        />
                        <FormInput
                            label='اسم الجد'
                            name='thirdName'
                            control={form.control}
                        />
                        <FormInput
                            label='الاسم الرابع'
                            name='fourthName'
                            control={form.control}
                        />
                        <FormInput
                            label='الاسم الخامس'
                            name='fivthName'
                            control={form.control}
                        />
                        <FormInput
                            label='العشيرة/القبيلة'
                            name='tribe'
                            control={form.control}
                        />
                        <FormSelect
                            className='w-full col-span-1 md:col-span-3'
                            label='صلة القرابة'
                            name='relationship'
                            control={form.control}
                            placeholder='اختر صلة القرابة'
                            required
                            options={relationshipTypeOptions}
                        />
                   </div> 
                   <FormTextarea
                        label='ملاحظات (اختياري)'
                        name='notes'
                        control={form.control}
                        config={{
                            rows: 3
                        }}
                    />
                   <div className="flex justify-end">
                        <Button type='submit' disabled={isLoading} className="w-full md:w-auto">
                            {isLoading ? <Spinner className="h-4 w-4" /> : 'حفظ'}
                        </Button>
                   </div>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
