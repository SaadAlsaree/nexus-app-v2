import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { GraduationCap, Shield, Briefcase, Edit2Icon, Trash2Icon } from 'lucide-react';
import { SuspectDetails, TrainingCourse, Operation, OrganizationalAssignment } from '../../types/suspect';
import { TrainingCourseDetail } from '../../types/training';
import { InfoItem } from '../info-item';

interface OrganizationalTabProps {
  suspect: SuspectDetails;
  formatDate: (date: string | null) => string;
  onAddTraining: () => void;
  onEditTraining: (course: TrainingCourseDetail) => void;
  onDeleteTraining: (course: TrainingCourseDetail) => void;
  onAddOperation: () => void;
  onEditOperation: (operation: Operation) => void;
  onDeleteOperation: (operation: Operation) => void;
  onAddOrganization: () => void;
  onEditOrganization: (assignment: OrganizationalAssignment) => void;
  onDeleteOrganization: (assignment: OrganizationalAssignment) => void;
}

export function OrganizationalTab({
  suspect,
  formatDate,
  onAddTraining,
  onEditTraining,
  onDeleteTraining,
  onAddOperation,
  onEditOperation,
  onDeleteOperation,
  onAddOrganization,
  onEditOrganization,
  onDeleteOrganization
}: OrganizationalTabProps) {
  return (
    <div className="space-y-4">
      {/* Bayah Details */}
      <Card>
        <CardHeader>
          <CardTitle>تفاصيل البيعة</CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          {suspect.bayahDetails.length > 0 ? suspect.bayahDetails.map((bayah) => (
            <div key={bayah.id} className="rounded-lg border p-4">
              <div className="grid gap-3 md:grid-cols-2">
                <InfoItem label="التاريخ" value={formatDate(bayah.date)} />
                <InfoItem label="المكان" value={bayah.location} />
                <InfoItem label="المجند" value={bayah.recruiterName} />
                <InfoItem label="نص البيعة" value={bayah.textOfPledge} />
              </div>
            </div>
          )) : (
            <p className="text-muted-foreground text-center py-8">لا توجد تفاصيل بيعة مسجلة</p>
          )}
        </CardContent>
      </Card>

      {/* Training Courses */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <GraduationCap className="h-5 w-5" />
              الدورات التدريبية
            </CardTitle>
            <Button variant="outline" onClick={onAddTraining}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent className="space-y-3">
          {suspect.trainingCourses.length > 0 ? suspect.trainingCourses.map((course) => (
            <div key={course.id} className="rounded-lg border p-4 space-y-3 bg-gray-100 dark:bg-zinc-900">
              <div className='relative top-0 right-0 flex justify-end gap-1'>
                <Button variant="outline" size="sm" onClick={() => onEditTraining(course)}>
                  <Edit2Icon className="h-4 w-4" />
                </Button>
                <Button variant="outline" size="sm" onClick={() => onDeleteTraining(course)}>
                  <Trash2Icon className="h-4 w-4" color='red' />
                </Button>
              </div>
              <div className="mb-2">
                <Badge>{course.courseTypeName}</Badge>
              </div>
              <div className="grid gap-2 md:grid-cols-2">
                <InfoItem label="المكان" value={course.location} />
                <InfoItem label="المدرب" value={course.trainerName} />
                <InfoItem label="الزملاء" value={course.classmates} />
              </div>
            </div>
          )) : (
            <p className="text-muted-foreground text-center py-8">لا توجد دورات تدريبية مسجلة</p>
          )}
        </CardContent>
      </Card>

      {/* Operations */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <Shield className="h-5 w-5" />
              العمليات
            </CardTitle>
            <Button variant="outline" onClick={onAddOperation}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent className="space-y-3">
          {suspect.operations.length > 0 ? suspect.operations.map((operation) => (
            <div key={operation.id} className="rounded-lg border p-4 space-y-3 bg-gray-100 dark:bg-zinc-900">
              <div className='relative top-0 right-0 flex justify-end gap-1'>
                <Button variant="outline" size="sm" onClick={() => onEditOperation(operation)}>
                  <Edit2Icon className="h-4 w-4" />
                </Button>
                <Button variant="outline" size="sm" onClick={() => onDeleteOperation(operation)}>
                  <Trash2Icon className="h-4 w-4" color='red' />
                </Button>
              </div>
              <div className="mb-2">
                <Badge variant="destructive">{operation.operationTypeName}</Badge>
              </div>
              <div className="grid gap-2 md:grid-cols-2">
                <InfoItem label="التاريخ" value={formatDate(operation.date)} />
                <InfoItem label="المكان" value={operation.location} />
                <InfoItem label="الدور في العملية" value={operation.roleInOp} />
                <InfoItem label="المرافقون" value={operation.associates} />
              </div>
            </div>
          )) : (
            <p className="text-muted-foreground text-center py-8">لا توجد عمليات مسجلة</p>
          )}
        </CardContent>
      </Card>

      {/* Organizational Assignments */}
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle className="flex items-center gap-2">
              <Briefcase className="h-5 w-5" />
              المناصب التنظيمية
            </CardTitle>
            <Button variant="outline" onClick={onAddOrganization}>
              ادراج جديد
            </Button>
          </div>
        </CardHeader>
        <CardContent className="space-y-3">
          {suspect.organizationalAssignments.length > 0 ? suspect.organizationalAssignments.map((assignment) => (
            <div key={assignment.id} className="rounded-lg border p-4 space-y-3 bg-gray-100 dark:bg-zinc-900">
              <div className='relative top-0 right-0 flex justify-end gap-1'>
                <Button variant="outline" size="sm" onClick={() => onEditOrganization(assignment)}>
                  <Edit2Icon className="h-4 w-4" />
                </Button>
                <Button variant="outline" size="sm" onClick={() => onDeleteOrganization(assignment)}>
                  <Trash2Icon className="h-4 w-4" color='red' />
                </Button>
              </div>
              <div className="mb-2 flex items-center justify-between">
                <Badge>{assignment.roleTitleName}</Badge>
                <span className="text-muted-foreground text-sm">
                  {assignment.orgUnitName}
                </span>
              </div>
              <div className="grid gap-2 md:grid-cols-2">
                <InfoItem label="تاريخ البدء" value={formatDate(assignment.startDate)} />
                <InfoItem label="تاريخ الانتهاء" value={formatDate(assignment.endDate)} />
                {assignment.directCommanderName && (
                  <InfoItem label="القائد المباشر" value={assignment.directCommanderName} />
                )}
              </div>
            </div>
          )) : (
            <p className="text-muted-foreground text-center py-8">لا توجد مناصب تنظيمية مسجلة</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
