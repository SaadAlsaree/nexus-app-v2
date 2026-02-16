"use client";
import z from 'zod';
import { OrgRole, OrgRoleLabels } from '../../types/org-assignment';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { FormInput } from '@/components/forms/form-input';
import { FormSelect } from '@/components/forms/form-select';
import { Button } from '@/components/ui/button';
import { Form } from '@/components/ui/form';
import { createOrgAssignment } from '../../api/org-Assignments.service';
import { toast } from 'sonner';
import { useEffect, useState } from 'react';
import { Spinner } from '@/components/ui/spinner';
import { getOrganizationalUnits } from '../../../org-unit/api/organization.service';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { useRouter } from 'next/navigation';
import { format } from 'date-fns';

const orgAssignmentFormSchema = z.object({
    suspectId: z.string().min(1, { message: 'Suspect ID is required.' }),
    orgUnitId: z.string().min(1, { message: 'Organizational unit is required.' }),
    directCommanderId: z.string().optional(),
    roleTitle: z.string().min(1, { message: 'Role is required.' }),
    startDate: z.date().optional(),
    endDate: z.date().optional()
});

export default function OrgAssignmentForm(
    { suspectId, title, onOpenChange, open }: 
    { suspectId: string; title: string; onOpenChange: (open: boolean) => void; open: boolean }
) {
    const [isLoading, setIsLoading] = useState(false);
    const [orgUnits, setOrgUnits] = useState<{ label: string; value: string }[]>([]);
    const router = useRouter();

    const defaultValues = {
        suspectId: suspectId || '',
        orgUnitId: '',
        directCommanderId: '',
        roleTitle: '',
        startDate: new Date(),
        endDate: new Date()
    };

    const form = useForm<z.infer<typeof orgAssignmentFormSchema>>({
        resolver: zodResolver(orgAssignmentFormSchema),
        defaultValues: defaultValues
    });

    useEffect(() => {
        const fetchOrgUnits = async () => {
            try {
                const response = await getOrganizationalUnits();
                if (response.data && response.data.totalCount > 0) {
                    const units = response.data.items.map((unit: any) => ({
                        label: unit.unitName,
                        value: unit.id
                    }));
                    setOrgUnits(units);
                }
            } catch (error) {
                console.error('Error fetching organizational units:', error);
                toast.error('فشل في تحميل الوحدات التنظيمية');
            }
        };
        if (open) {
            fetchOrgUnits();
        }
    }, [open]);

    const roleOptions = Object.entries(OrgRoleLabels).map(([value, label]) => ({
        label: label,
        value: value
    }));

    const onSubmit = async (values: z.infer<typeof orgAssignmentFormSchema>) => {
        try {
            setIsLoading(true);
            const safeStartDate = values.startDate ? format(values.startDate, 'yyyy-MM-dd') : undefined;
            const safeEndDate = values.endDate ? format(values.endDate, 'yyyy-MM-dd') : undefined;
            console.log('Submitted values:', values);
            await createOrgAssignment({
                suspectId: values.suspectId,
                orgUnitId: values.orgUnitId,
                directCommanderId: values.directCommanderId || undefined,
                roleTitle: Number(values.roleTitle) as OrgRole,
                startDate: safeStartDate || '',
                endDate: safeEndDate
            });
            toast.success('تمت إضافة التكليف التنظيمي بنجاح');
            form.reset();
            onOpenChange(false);
        } catch (error) {
            console.error('Error adding organizational assignment:', error);
            toast.error(`فشل في إضافة التكليف التنظيمي: ${error}`);
        } finally {
            router.refresh();
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent>
                <DialogHeader dir='rtl'>
                    <DialogTitle>{title}</DialogTitle>
                </DialogHeader>
                <Form form={form} onSubmit={form.handleSubmit(onSubmit)} className='space-y-4 gap-3'>
                    <div className='grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-2'>
                        <FormSelect
                            label='الوحدة التنظيمية'
                            name='orgUnitId'
                            control={form.control}
                            placeholder='اختر الوحدة'
                            required
                            options={orgUnits}
                        />
                        <FormSelect
                            label='الدور/المنصب'
                            name='roleTitle'
                            control={form.control}
                            placeholder='اختر الدور'
                            required
                            options={roleOptions}
                        />
                        <FormDatePicker
                            label='تاريخ البداية'
                            name='startDate'
                            control={form.control}
                            required
                        />
                        <FormDatePicker
                            label='تاريخ النهاية (اختياري)'
                            name='endDate'
                            control={form.control}
                        />
                        <FormInput
                            label='القائد المباشر (اختياري)'
                            name='directCommanderId'
                            control={form.control}
                        />
                    </div>
                    <Button type='submit' disabled={isLoading}>
                        {isLoading ? <Spinner /> : 'حفظ'}
                    </Button>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
