import z from 'zod';
import { OrganizationalAssignment } from '../../../suspect/types/suspect';
import { OrganizationalUnitDetail, OrganizationalUnitRequest, OrgUnitLevelLabels, OrgUnitListItem } from '../../types/organization';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createOrganizationalUnit, getOrganizationalUnitById, getOrganizationalUnits, updateOrganizationalUnit } from '../../api/organization.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';


const organizationFormSchema = z.object({
    unitName: z.string().min(1, { message: 'Unit name is required.' }),
    level: z.string().min(1, { message: 'Level is required.' }),
    parentUnitId: z.string().optional()
});

export default function OrganizationForm(
    {id, initialData, title, onOpenChange, open} : 
    {id: string; initialData?: OrganizationalUnitRequest; title: string; onOpenChange: (open: boolean) => void; open: boolean}
) {
    const [isLoading, setIsLoading] =  useState(false);
    const [allOrgUnits, setAllOrgUnits] = useState<OrgUnitListItem[]>([]);
    const defaultValues = {
        unitName: '',
        level:  '',
        parentUnitId:  ''
    };

    useEffect(() => {
        getOrganizationalUnits().then(response => {
            if (response.succeeded && response.data) {
                setAllOrgUnits(response.data.items);
            } else {
                toast.error('فشل في تحميل الوحدات التنظيمية');
            }
        }).catch(error => {
            console.error('Error fetching organizational units:', error);
            toast.error('حدث خطأ أثناء تحميل الوحدات التنظيمية');
        });
    }, []);
    useEffect(() => {        
        if (id) {
            getOrganizationalUnitById(id).then(unitDetail => {
                const unitData = unitDetail.data;
                form.reset({
                    unitName: unitData?.unitName || '',
                    level: unitData?.level?.toString() || '',
                    parentUnitId: unitData?.parentUnitId || ''
                });
        })
        }
    }, [id]);
    const form = useForm<z.infer<typeof organizationFormSchema>>({
        resolver: zodResolver(organizationFormSchema),
        defaultValues: defaultValues
    });

    const levelOptions = Object.entries(OrgUnitLevelLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const unitOptions = allOrgUnits.map(unit => ({
        label: unit.unitName,
        value: unit.id
    }));

    const onSubmit = async (values: z.infer<typeof organizationFormSchema>) => {
        try {
            setIsLoading(true);
            if (initialData) {
                await updateOrganizationalUnit({ 
                    unitId: id, 
                    unitName: values.unitName,
                    level: Number(values.level),
                    parentUnitId: values.parentUnitId || null
                });
                toast.success('تم تحديث الوحدة التنظيمية بنجاح');
                onOpenChange(false);
                return;
            }
        await createOrganizationalUnit({ 
            unitName: values.unitName,
            level: Number(values.level),
            parentUnitId: values.parentUnitId || null
        });
        toast.success('تمت إضافة الوحدة التنظيمية بنجاح');
        onOpenChange(false);
        } catch (error) {
        console.error('Error adding organizational unit:', error);
        toast.error(`فشل في إضافة الوحدة التنظيمية: ${error}`);
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
                    <FormInput
                        label='اسم الوحدة'
                        name='unitName'
                        control={form.control}
                        required
                    />
                    <FormSelect
                        label='المستوى التنظيمي'
                        name='level'
                        control={form.control}
                        placeholder='اختر المستوى'
                        required
                        options={levelOptions}
                    />
                    <FormSelect
                        label='الوحدة الأم (اختياري)'
                        name='parentUnitId'
                        control={form.control}
                        placeholder='اختر الوحدة الأم'
                        options={unitOptions}
                    />
                   </div> 
                   <Button type='submit' disabled={isLoading}>{isLoading ? <Spinner /> : 'حفظ'}</Button>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
