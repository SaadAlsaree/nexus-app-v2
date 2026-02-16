
'use client';

import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { FileText, User, Edit } from 'lucide-react';
import { CaseDetails, CaseStatusLabels, PriorityLevelLabels } from '../types/case';
import Link from 'next/link';
import { CaseSuspectsManager } from './case-suspects-manager';

interface CaseDetailViewProps {
  caseDetail: CaseDetails;
}

export default function CaseDetailView({ caseDetail }: CaseDetailViewProps) {
  
  const formatDate = (date: string | null) => {
    if (!date) return 'غير محدد';
    return new Date(date).toLocaleDateString('ar-EG', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  return (
    <div className="space-y-6" dir="rtl">
      <Card>
        <CardContent className="pt-6">
            <div className="flex flex-col gap-6 md:flex-row md:items-start md:justify-between">
            <div className="space-y-2">
                <h1 className="text-3xl font-bold">{caseDetail.title}</h1>
                <p className="text-muted-foreground text-lg">رقم القضية: {caseDetail.caseFileNumber}</p>
                <div className="flex flex-wrap items-center gap-2">
                    <Badge variant="outline">
                        {CaseStatusLabels[caseDetail.status]}
                    </Badge>
                     <Badge variant={caseDetail.priority >= 3 ? 'destructive' : 'secondary'}>
                        {PriorityLevelLabels[caseDetail.priority]}
                    </Badge>
                </div>
            </div>
             <div className="flex items-center gap-2">
                <Link href={`/cases/${caseDetail.id}/edit`}>
                <Button variant="outline">
                    <Edit className="h-4 w-4 ml-2" />
                    تعديل
                </Button>
                </Link>
             </div>
            </div>
        </CardContent>
      </Card>

       <Tabs defaultValue="info" className="w-full">
        <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="info">
                <FileText className="h-4 w-4 ml-2" />
                تفاصيل القضية
            </TabsTrigger>
            <TabsTrigger value="suspects">
                <User className="h-4 w-4 ml-2" />
                المتهمين ({caseDetail.suspectsCount})
            </TabsTrigger>
        </TabsList>

        <TabsContent value="info" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>المعلومات الأساسية</CardTitle>
                </CardHeader>
                <CardContent>
                    <div className="grid gap-4 md:grid-cols-2">
                         <InfoItem label="رقم القضية" value={caseDetail.caseFileNumber} />
                         <InfoItem label="العنوان" value={caseDetail.title} />
                         <InfoItem label="تاريخ الفتح" value={formatDate(caseDetail.openDate)} />
                         <InfoItem label="ضابط التحقيق" value={caseDetail.investigatingOfficer} />
                         <InfoItem label="الحالة" value={CaseStatusLabels[caseDetail.status]} />
                         <InfoItem label="الأولوية" value={PriorityLevelLabels[caseDetail.priority]} />
                         <InfoItem label="تاريخ الإنشاء" value={formatDate(caseDetail.createdAt)} />
                    </div>
                </CardContent>
            </Card>
        </TabsContent>

        <TabsContent value="suspects" className="space-y-4">
             <CaseSuspectsManager caseDetail={caseDetail} />
        </TabsContent>
       </Tabs>
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
