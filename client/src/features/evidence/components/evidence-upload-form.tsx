'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Button } from '@/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { createEvidence } from '../api/evidence.service';
import { toast } from 'sonner';
import { Loader2, Upload } from 'lucide-react';

const formSchema = z.object({
    description: z.string().optional(),
    file: z.any().refine((file) => file instanceof File, 'الرجاء اختيار ملف'),
});

interface EvidenceUploadFormProps {
    caseId?: string;
    suspectId?: string;
    onSuccess?: () => void;
    onCancel?: () => void;
}

export function EvidenceUploadForm({ caseId, suspectId, onSuccess, onCancel }: EvidenceUploadFormProps) {
    const [isUploading, setIsUploading] = useState(false);

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            description: '',
        },
    });

    async function onSubmit(values: z.infer<typeof formSchema>) {
        setIsUploading(true);
        try {
            const result = await createEvidence({
                file: values.file,
                caseId,
                suspectId,
                description: values.description,
            });

            if (result.succeeded) {
                toast.success('تم رفع الدليل بنجاح');
                form.reset();
                onSuccess?.();
            } else {
                toast.error(result.message || 'فشل رفع الدليل');
            }
        } catch (error) {
            toast.error('حدث خطأ أثناء الرفع');
        } finally {
            setIsUploading(false);
        }
    }

    return (
        <Form 
            form={form} 
            onSubmit={form.handleSubmit(onSubmit)} 
            className="space-y-4"
        >
            <FormField
                control={form.control}
                name="file"
                render={({ field: { value, onChange, ...field } }) => (
                    <FormItem>
                        <FormLabel>الملف</FormLabel>
                        <FormControl>
                            <Input
                                type="file"
                                onChange={(e) => {
                                    const file = e.target.files?.[0];
                                    if (file) onChange(file);
                                }}
                                {...field}
                            />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                )}
            />

            <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>الوصف</FormLabel>
                        <FormControl>
                            <Textarea
                                placeholder="أدخل وصفاً للدليل هنا..."
                                className="resize-none"
                                {...field}
                            />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                )}
            />

            <div className="flex justify-end gap-2 pt-4">
                {onCancel && (
                    <Button type="button" variant="outline" onClick={onCancel} disabled={isUploading}>
                        إلغاء
                    </Button>
                )}
                <Button type="submit" disabled={isUploading}>
                    {isUploading ? (
                        <>
                            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                            جاري الرفع...
                        </>
                    ) : (
                        <>
                            <Upload className="mr-2 h-4 w-4" />
                            رفع الملف
                        </>
                    )}
                </Button>
            </div>
        </Form>
    );
}
