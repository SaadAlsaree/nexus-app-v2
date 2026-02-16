import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Edit2Icon, Trash2Icon } from 'lucide-react';
import { SuspectDetails, LifeHistory } from '../../types/suspect';
import { InfoItem } from '../info-item';

interface PersonalTabProps {
  suspect: SuspectDetails;
  formatDate: (date: string | null) => string;
  onAddHistory: () => void;
  onEditHistory: (history: LifeHistory) => void;
  onDeleteHistory: (history: LifeHistory) => void;
}

export function PersonalTab({ 
  suspect, 
  formatDate, 
  onAddHistory, 
  onEditHistory, 
  onDeleteHistory 
}: PersonalTabProps) {
  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <CardTitle>المعلومات الأساسية</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            <InfoItem label="الاسم الأول" value={suspect.firstName} />
            <InfoItem label="الاسم الثاني" value={suspect.secondName} />
            <InfoItem label="الاسم الثالث" value={suspect.thirdName} />
            <InfoItem label="الاسم الرابع" value={suspect.fourthName} />
            <InfoItem label="الاسم الخامس" value={suspect.fivthName} />
            <InfoItem label="اسم العائلة" value={suspect.familyName} />
            <InfoItem label="اسم الأم" value={suspect.motherName} />
            <InfoItem label="العشيرة" value={suspect.tribe} />
            <InfoItem label="تاريخ الميلاد" value={formatDate(suspect.dateOfBirth)} />
            <InfoItem label="مكان الميلاد" value={suspect.placeOfBirth} />
            <InfoItem label='الحالة الصحية' value={suspect.healthStatus} />
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>المعلومات العائلية</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            <InfoItem label="الحالة الاجتماعية" value={suspect.maritalStatusName} />
            <InfoItem label="عدد الزوجات" value={suspect.wivesCount} />
            <InfoItem label="عدد الأطفال" value={suspect.childrenCount} />
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle>السيرة الحياتية</CardTitle>
            <Button variant="outline" onClick={onAddHistory}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent className="space-y-4">
          {suspect.lifeHistories.length > 0 ? suspect.lifeHistories.map((history) => (
            <div key={history.id} className="rounded-lg border p-4 space-y-3 bg-gray-100 dark:bg-zinc-900">
              <div className='relative top-0 right-0 flex justify-end gap-1'>
                <Button variant="outline" size="sm" onClick={() => onEditHistory(history)}>
                  <Edit2Icon className="h-4 w-4" />
                </Button>
                <Button variant="outline" size="sm" onClick={() => onDeleteHistory(history)}>
                  <Trash2Icon className="h-4 w-4" color='red' />
                </Button>
              </div>
              <div className="grid gap-3 md:grid-cols-2">
                <InfoItem label="المستوى التعليمي" value={history.educationLevel} />
                <InfoItem label="المدارس التي درس فيها" value={history.schoolsAttended} />
                <InfoItem label="الوظائف المدنية" value={history.civilianJobs} />
              </div>
              {history.radicalizationStory ? (
                <div className="pt-2 border-t">
                  <InfoItem label="قصة التطرف" value={history.radicalizationStory} />
                </div>
              ) : <p className="text-muted-foreground">لا توجد قصة تطرف مسجلة</p>}
            </div>
          )) : (
            <p className="text-muted-foreground text-center py-8">لا توجد سيرة حياتية مسجلة</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
