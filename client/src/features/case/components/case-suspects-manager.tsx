'use client';

import * as React from 'react';
import { toast } from 'sonner';
import { Plus, Trash2, User, Users } from 'lucide-react';
import { useRouter } from 'next/navigation';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';

import { CaseDetails, AccusationTypeLabels, LegalStatusInCaseLabels } from '../types/case';
import { removeSuspectFromCase } from '../api/case.service';
import { AddSuspectToCaseForm } from './add-suspect-to-case-form';
import Link from 'next/link';

interface CaseSuspectsManagerProps {
  caseDetail: CaseDetails;
  onRefresh?: () => void;
}

export function CaseSuspectsManager({ caseDetail, onRefresh }: CaseSuspectsManagerProps) {
  const [isAddDialogOpen, setIsAddDialogOpen] = React.useState(false);
  const [isRemoving, setIsRemoving] = React.useState<string | null>(null);
  const router = useRouter();

  const handleRemove = async (suspectId: string) => {
    setIsRemoving(suspectId);
    try {
      const response = await removeSuspectFromCase({
        caseId: caseDetail.id,
        suspectId: suspectId,
      });

      if (response.succeeded) {
        toast.success('تم إزالة المتهم من القضية بنجاح');
        if (onRefresh) {
          onRefresh();
        } else {
          router.refresh();
        }
      } else {
        toast.error(response.message || 'فشل إزالة المتهم');
      }
    } catch (error) {
      toast.error('حدث خطأ أثناء إزالة المتهم');
      console.error(error);
    } finally {
      setIsRemoving(null);
    }
  };

  const formatDate = (date: string | null) => {
    if (!date) return null;
    return new Date(date).toLocaleDateString('ar-EG', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  return (
    <Card className="w-full" dir="rtl">
      <CardHeader className="flex flex-row items-center justify-between space-y-0">
        <div className="space-y-1">
          <CardTitle className="text-2xl flex items-center gap-2">
            <Users className="h-6 w-6" />
            المتهمين في القضية
          </CardTitle>
          <CardDescription>إدارة المتهمين المرتبطين بهذه القضية</CardDescription>
        </div>
        <Dialog open={isAddDialogOpen} onOpenChange={setIsAddDialogOpen}>
          <DialogTrigger asChild>
            <Button className="gap-2">
              <Plus className="h-4 w-4" />
              إضافة متهم
            </Button>
          </DialogTrigger>
          <DialogContent className="sm:max-w-[600px]">
            <DialogHeader>
              <DialogTitle className="text-right">إضافة متهم للقضية</DialogTitle>
              <DialogDescription className="text-right">
                اختر متهماً وحدد نوع التهمة والحالة القانونية له في هذه القضية.
              </DialogDescription>
            </DialogHeader>
            <AddSuspectToCaseForm 
              caseId={caseDetail.id} 
              onSuccess={() => {
                setIsAddDialogOpen(false);
                if (onRefresh) onRefresh();
                router.refresh();
              }}
              onCancel={() => setIsAddDialogOpen(false)}
            />
          </DialogContent>
        </Dialog>
      </CardHeader>
      <CardContent>
        {caseDetail.suspects && caseDetail.suspects.length > 0 ? (
          <div className="grid gap-4">
            {caseDetail.suspects.map((suspect) => (
              <div
                key={suspect.suspectId}
                className="flex flex-col md:flex-row md:items-center justify-between p-4 rounded-lg border bg-card hover:bg-accent/50 transition-colors gap-4"
              >
                <div className="flex items-start gap-4">
                  <div className="mt-1 bg-primary/10 p-2 rounded-full">
                    <User className="h-5 w-5 text-primary" />
                  </div>
                  <div>
                    <Link href={`/suspect/${suspect.suspectId}`} className="font-bold text-lg hover:underline text-blue-600">
                      {suspect.fullName}
                    </Link>
                    <div className="flex flex-wrap gap-2 mt-1">
                      <Badge variant="outline">
                        {AccusationTypeLabels[suspect.accusationType]}
                      </Badge>
                      <Badge variant="secondary">
                        {LegalStatusInCaseLabels[suspect.legalStatus]}
                      </Badge>
                      {suspect.confessionDate && (
                        <Badge variant="outline" className="text-xs">
                          تاريخ الاعتراف: {formatDate(suspect.confessionDate)}
                        </Badge>
                      )}
                    </div>
                    {suspect.notes && (
                      <p className="text-sm text-muted-foreground mt-2 border-r-2 pr-2 mr-1">
                        {suspect.notes}
                      </p>
                    )}
                  </div>
                </div>

                <AlertDialog>
                  <AlertDialogTrigger asChild>
                    <Button
                      variant="ghost"
                      size="icon"
                      className="text-destructive hover:text-destructive hover:bg-destructive/10 self-end md:self-center"
                      disabled={isRemoving === suspect.suspectId}
                    >
                      <Trash2 className="h-5 w-5" />
                    </Button>
                  </AlertDialogTrigger>
                  <AlertDialogContent>
                    <AlertDialogHeader>
                      <AlertDialogTitle className="text-right">هل أنت متأكد؟</AlertDialogTitle>
                      <AlertDialogDescription className="text-right">
                        سيتم إزالة المتهم {suspect.fullName} من هذه القضية. هذا الإجراء لا يحذف المتهم من النظام، بل يزيل ارتباطه بهذه القضية فقط.
                      </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter className="flex-row-reverse gap-2">
                      <AlertDialogAction
                        onClick={() => handleRemove(suspect.suspectId)}
                        className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                      >
                        إزالة
                      </AlertDialogAction>
                      <AlertDialogCancel>إلغاء</AlertDialogCancel>
                    </AlertDialogFooter>
                  </AlertDialogContent>
                </AlertDialog>
              </div>
            ))}
          </div>
        ) : (
          <div className="text-center py-12 border-2 border-dashed rounded-lg">
            <Users className="h-12 w-12 mx-auto text-muted-foreground opacity-20" />
            <p className="mt-2 text-muted-foreground font-medium">لا يوجد متهمين مرتبطين بهذه القضية</p>
            <Button
              variant="link"
              onClick={() => setIsAddDialogOpen(true)}
              className="mt-1"
            >
              إضافة المتهم الأول الآن
            </Button>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
