
'use client';

import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Edit, CheckCircle, XCircle, FileText, Loader2 } from 'lucide-react';
import { 
  InterrogationSession, 
  InterrogationTypeLabels, 
  InterrogationOutcomeLabels 
} from '../types/interrogation-session';
import Link from 'next/link';
import { downloadInterrogationSessionReport } from '../api/interrogation-session.service';
import { useState } from 'react';
import { toast } from 'sonner';

interface InterrogationSessionDetailViewProps {
  session: InterrogationSession;
}

export default function InterrogationSessionDetailView({ session }: InterrogationSessionDetailViewProps) {
  const [isDownloading, setIsDownloading] = useState(false);

  const handleDownload = async () => {
    try {
      setIsDownloading(true);
      await downloadInterrogationSessionReport(
        session.id, 
        session.suspect.fullName, 
        session.sessionDate
      );
      toast.success('تم تحميل التقرير بنجاح');
    } catch (error) {
      console.error('Download error:', error);
      toast.error('حدث خطأ أثناء تحميل التقرير');
    } finally {
      setIsDownloading(false);
    }
  };
  
  const formatDate = (date: string | null) => {
    if (!date) return 'غير محدد';
    return new Date(date).toLocaleDateString('ar-EG', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  return (
    <div className="space-y-6" dir="rtl">
      <Card>
        <CardContent className="pt-6">
          <div className="flex flex-col gap-6 md:flex-row md:items-start md:justify-between">
            <div className="space-y-2">
              <h1 className="text-3xl font-bold">جلسة استجواب</h1>
              <p className="text-muted-foreground text-lg">التاريخ: {formatDate(session.sessionDate)}</p>
              <div className="flex flex-wrap items-center gap-2">
                <Badge variant="outline">
                  {InterrogationTypeLabels[session.sessionType]}
                </Badge>
                <Badge variant={session.outcome === 1 ? 'default' : session.outcome === 3 ? 'destructive' : 'secondary'}>
                  {InterrogationOutcomeLabels[session.outcome]}
                </Badge>
                {session.isRatifiedByJudge ? (
                  <Badge variant="default" className="gap-1">
                    <CheckCircle className="h-3 w-3" />
                    مصدق قضائياً
                  </Badge>
                ) : (
                  <Badge variant="outline" className="gap-1">
                    <XCircle className="h-3 w-3" />
                    غير مصدق
                  </Badge>
                )}
              </div>
            </div>
            <div className="flex items-center gap-2">
              <Button 
                variant="outline" 
                onClick={handleDownload} 
                disabled={isDownloading}
                className="gap-2"
              >
                {isDownloading ? (
                  <Loader2 className="h-4 w-4 animate-spin" />
                ) : (
                  <FileText className="h-4 w-4" />
                )}
                تحميل التقرير
              </Button>
              <Link href={`/interrogation-session/${session.id}/edit`}>
                <Button variant="outline">
                  <Edit className="h-4 w-4 ml-2" />
                  تعديل
                </Button>
              </Link>
            </div>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>المعلومات الأساسية</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2">
            <InfoItem label="المحقق" value={session.interrogatorName} />
            <InfoItem label="الموقع" value={session.location} />
            <InfoItem label="نوع الجلسة" value={session.sessionTypeName || InterrogationTypeLabels[session.sessionType]} />
            <InfoItem label="النتيجة" value={session.outcomeName || InterrogationOutcomeLabels[session.outcome]} />
            <InfoItem label="تاريخ الإنشاء" value={formatDate(session.createdAt)} />
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>محتوى الجلسة</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="whitespace-pre-wrap text-sm leading-relaxed">{session.content}</p>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>ملخص الجلسة</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="whitespace-pre-wrap text-sm leading-relaxed">{session.summaryContent}</p>
        </CardContent>
      </Card>

      {session.investigatorNotes && (
        <Card>
          <CardHeader>
            <CardTitle>ملاحظات المحقق</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="whitespace-pre-wrap text-sm leading-relaxed">{session.investigatorNotes}</p>
          </CardContent>
        </Card>
      )}

      <div className="flex gap-2">
        <Link href={`/suspect/${session.suspectId}`}>
          <Button variant="outline">العودة للمشتبه به</Button>
        </Link>
        <Link href={`/cases/${session.caseId}`}>
          <Button variant="outline">عرض القضية</Button>
        </Link>
      </div>
    </div>
  );
}

function InfoItem({ label, value }: { label: string; value: any }) {
  return (
    <div>
      <dt className="text-muted-foreground text-sm font-medium">{label}</dt>
      <dd className="text-foreground mt-1 text-sm">{value || 'غير محدد'}</dd>
    </div>
  );
}
