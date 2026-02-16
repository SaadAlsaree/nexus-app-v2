'use client';

import * as React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { format } from 'date-fns';
import { toast } from 'sonner';
import { Check, ChevronsUpDown, Search, UserPlus, X } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { FormSelect } from '@/components/forms/form-select';
import { FormDatePicker } from '@/components/forms/form-date-picker';
import { FormTextarea } from '@/components/forms/form-textarea';
import { Spinner } from '@/components/ui/spinner';
import { ScrollArea } from '@/components/ui/scroll-area';

import { 
  AccusationType, 
  AccusationTypeLabels, 
  LegalStatusInCase, 
  LegalStatusInCaseLabels,
} from '../types/case';
import { addSuspectToCase } from '../api/case.service';
import { getSuspects } from '../../suspect/api/suspect.service';
import { Suspect } from '../../suspect/types/suspect';

const addSuspectSchema = z.object({
  suspectId: z.string().min(1, { message: 'يجب اختيار المتهم' }),
  accusationType: z.string().min(1, { message: 'نوع التهمة مطلوب' }),
  legalStatus: z.string().min(1, { message: 'الحالة القانونية مطلوبة' }),
  confessionDate: z.date().optional().nullable(),
  notes: z.string().optional().nullable(),
});

type AddSuspectFormValues = z.infer<typeof addSuspectSchema>;

interface AddSuspectToCaseFormProps {
  caseId: string;
  onSuccess?: () => void;
  onCancel?: () => void;
}

export function AddSuspectToCaseForm({ caseId, onSuccess, onCancel }: AddSuspectToCaseFormProps) {
  const [open, setOpen] = React.useState(false);
  const [loadingSuspects, setLoadingSuspects] = React.useState(false);
  const [suspects, setSuspects] = React.useState<Suspect[]>([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [submitting, setSubmitting] = React.useState(false);

  const form = useForm<AddSuspectFormValues>({
    resolver: zodResolver(addSuspectSchema),
    defaultValues: {
      suspectId: '',
      accusationType: String(AccusationType.Membership),
      legalStatus: String(LegalStatusInCase.DetainedPendingInvestigation),
      confessionDate: null,
      notes: '',
    },
  });

  const selectedSuspectId = form.watch('suspectId');
  const selectedSuspect = suspects.find((s) => s.id === selectedSuspectId);

  // Fetch suspects when searching
  React.useEffect(() => {
    const fetchSuspects = async () => {
      setLoadingSuspects(true);
      try {
        const response = await getSuspects({ searchTerm, pageSize: 10 });
        if (response.succeeded && response.data) {
          setSuspects(response.data.items);
        }
      } catch (error) {
        console.error('Failed to fetch suspects:', error);
      } finally {
        setLoadingSuspects(false);
      }
    };

    const timer = setTimeout(() => {
      fetchSuspects();
    }, 500);

    return () => clearTimeout(timer);
  }, [searchTerm]);

  async function onSubmit(values: AddSuspectFormValues) {
    setSubmitting(true);
    try {
      const formattedValues = {
        caseId,
        suspectId: values.suspectId,
        accusationType: Number(values.accusationType) as AccusationType,
        legalStatus: Number(values.legalStatus) as LegalStatusInCase,
        confessionDate: values.confessionDate ? format(values.confessionDate, 'yyyy-MM-dd') : undefined,
        notes: values.notes || undefined,
      };

      const response = await addSuspectToCase(formattedValues);

      if (response.succeeded) {
        toast.success('تم إضافة المتهم إلى القضية بنجاح');
        form.reset();
        onSuccess?.();
      } else {
        toast.error(response.message || 'فشل إضافة المتهم');
      }
    } catch (error) {
      toast.error('حدث خطأ أثناء إضافة المتهم');
      console.error(error);
    } finally {
      setSubmitting(false);
    }
  }

  const accusationOptions = Object.entries(AccusationTypeLabels).map(([value, label]) => ({
    label,
    value,
  }));

  const legalStatusOptions = Object.entries(LegalStatusInCaseLabels).map(([value, label]) => ({
    label,
    value,
  }));

  return (
    <Form form={form} onSubmit={form.handleSubmit(onSubmit)} className="space-y-6" dir="rtl">
        <FormField
          control={form.control}
          name="suspectId"
          render={({ field }) => (
            <FormItem className="flex flex-col">
              <FormLabel>المتهم</FormLabel>
              <Popover open={open} onOpenChange={setOpen}>
                <PopoverTrigger asChild>
                  <FormControl>
                    <Button
                      variant="outline"
                      role="combobox"
                      aria-expanded={open}
                      className={cn(
                        "w-full justify-between text-right",
                        !field.value && "text-muted-foreground"
                      )}
                    >
                      {field.value
                        ? suspects.find((suspect) => suspect.id === field.value)?.fullName || "متهم مختار"
                        : "اختر المتهم..."}
                      <ChevronsUpDown className="mr-2 h-4 w-4 shrink-0 opacity-50" />
                    </Button>
                  </FormControl>
                </PopoverTrigger>
                <PopoverContent className="w-[400px] p-0" align="start">
                  <Command>
                    <CommandInput 
                      placeholder="بحث عن متهم..." 
                      className="text-right" 
                      onValueChange={setSearchTerm}
                    />
                    <CommandList>
                      {loadingSuspects ? (
                        <div className="flex items-center justify-center py-6">
                          <Spinner className="h-4 w-4" />
                        </div>
                      ) : (
                        <>
                          <CommandEmpty>لم يتم العثور على متهم.</CommandEmpty>
                          <CommandGroup>
                            <ScrollArea className="h-72">
                              {suspects.map((suspect) => (
                                <CommandItem
                                  key={suspect.id}
                                  value={suspect.fullName}
                                  onSelect={() => {
                                    form.setValue("suspectId", suspect.id);
                                    setOpen(false);
                                  }}
                                  className="text-right flex justify-between items-center"
                                >
                                  <Check
                                    className={cn(
                                      "ml-2 h-4 w-4",
                                      field.value === suspect.id ? "opacity-100" : "opacity-0"
                                    )}
                                  />
                                  <span>{suspect.fullName}</span>
                                </CommandItem>
                              ))}
                            </ScrollArea>
                          </CommandGroup>
                        </>
                      )}
                    </CommandList>
                  </Command>
                </PopoverContent>
              </Popover>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormSelect
            control={form.control}
            name="accusationType"
            label="نوع التهمة"
            placeholder="اختر نوع التهمة"
            options={accusationOptions}
            required
          />

          <FormSelect
            control={form.control}
            name="legalStatus"
            label="الحالة القانونية في القضية"
            placeholder="اختر الحالة القانونية"
            options={legalStatusOptions}
            required
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormDatePicker
            control={form.control}
            name="confessionDate"
            label="تاريخ الاعتراف (إن وجد)"
          />
        </div>

        <FormTextarea
          control={form.control}
          name="notes"
          label="ملاحظات"
          placeholder="أدخل أي ملاحظات إضافية..."
        />

        <div className="flex justify-end gap-2">
          {onCancel && (
            <Button type="button" variant="outline" onClick={onCancel}>
              إلغاء
            </Button>
          )}
          <Button type="submit" disabled={submitting}>
            {submitting ? (
              <>
                <Spinner className="ml-2 h-4 w-4" />
                جاري الإضافة...
              </>
            ) : (
              <>
                <UserPlus className="ml-2 h-4 w-4" />
                إضافة المتهم للقضية
              </>
            )}
          </Button>
        </div>
    </Form>
  );
}
