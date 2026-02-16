'use client';

import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  User,
  Phone,
  Users,
  Shield,
  FileText,
  Calendar,
  Edit,
  Download,
  Loader2,
  FileBox // Changed from FileText for contrast or use FileIcon
} from 'lucide-react';
import { SuspectDetails, SuspectStatusLabels, LifeHistory, Operation, OrganizationalAssignment, Contact, Address, RelativesAndAssociate } from '../types/suspect';
import { TrainingCourseDetail } from '../types/training';
import Link from 'next/link';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { useState } from 'react';
import HistoryForm from './forms/history-form';
import TrainingForm from './forms/training-form';
import OperationForm from './forms/operation-form';
import ContactForm from './forms/contact-form';
import AddressForm from './forms/address-form';
import RelationshipForm from './forms/relationship-form';
import DeleteWarning from '@/components/delete-worning';
import { toast } from 'sonner';
import { deleteLifeHistory, downloadSuspectReport } from '../api/suspect.service';
import { deleteTrainingCourse } from '../api/training.service';
import { deleteOperation } from '../api/operation.service';
import { deleteOrganizationalUnit } from '../../org-unit/api/organization.service';
import { deleteContact } from '../api/contact.service';
import { deleteAddress } from '../api/address.service';
import { deleteRelationship } from '../api/relationship.service';
import { StackIcon } from '@radix-ui/react-icons';
import StatusForm from './forms/status-form';
import OrgAssignmentForm from './forms/org-assignment-form';
import { useTheme } from 'next-themes';

// Import New Tab Components
import { PersonalTab } from './tabs/personal-tab';
import { NetworkTab } from './tabs/network-tab';
import { ContactsTab } from './tabs/contacts-tab';
import { RelationsTab } from './tabs/relations-tab';
import { OrganizationalTab } from './tabs/organizational-tab';
import { CasesTab } from './tabs/cases-tab';
import { InterrogationSessionsTab } from './tabs/interrogation-sessions-tab';
import { EvidenceTab } from '../../evidence/components/evidence-tab';

interface SuspectDetailViewProps {
  suspect: SuspectDetails;
}

export default function SuspectDetailView({ suspect }: SuspectDetailViewProps) {
  const getInitials = () => {
    return `${suspect.firstName?.[0] || ''}${suspect.secondName?.[0] || ''}`.toUpperCase();
  };

  // Form States
  const [historyFormOpen, setHistoryFormOpen] = useState(false);
  const [statusFormOpen, setStatusFormOpen] = useState(false);
  const [trainingFormOpen, setTrainingFormOpen] = useState(false);
  const [operationFormOpen, setOperationFormOpen] = useState(false);
  const [organizationFormOpen, setOrganizationFormOpen] = useState(false);
  const [contactFormOpen, setContactFormOpen] = useState(false);
  const [addressFormOpen, setAddressFormOpen] = useState(false);
  const [relationshipFormOpen, setRelationshipFormOpen] = useState(false);

  // Selected Item States for Edit/Delete
  const [lifeHistory, setLifeHistory] = useState<LifeHistory | null>(null);
  const [trainingCourse, setTrainingCourse] = useState<TrainingCourseDetail | null>(null);
  const [operation, setOperation] = useState<Operation | null>(null);
  const [organizationalAssignment, setOrganizationalAssignment] = useState<OrganizationalAssignment | null>(null);
  const [contact, setContact] = useState<Contact | null>(null);
  const [address, setAddress] = useState<Address | null>(null);
  const [relationship, setRelationship] = useState<RelativesAndAssociate | null>(null);

  // Delete Warning States
  const [deleteWarningOpen, setDeleteWarningOpen] = useState(false);
  const [deleteTrainingWarningOpen, setDeleteTrainingWarningOpen] = useState(false);
  const [deleteOperationWarningOpen, setDeleteOperationWarningOpen] = useState(false);
  const [deleteOrganizationWarningOpen, setDeleteOrganizationWarningOpen] = useState(false);
  const [deleteContactWarningOpen, setDeleteContactWarningOpen] = useState(false);
  const [deleteAddressWarningOpen, setDeleteAddressWarningOpen] = useState(false);
  const [deleteRelationshipWarningOpen, setDeleteRelationshipWarningOpen] = useState(false);

  const { resolvedTheme } = useTheme();
  const theme = (resolvedTheme === 'dark' ? 'dark' : 'light') as 'light'|'dark';

  const [isDownloading, setIsDownloading] = useState(false);

  const handleDownloadReport = async () => {
    try {
      setIsDownloading(true);
      await downloadSuspectReport(suspect.id);
      toast.success('تم تجهيز التقرير والتحميل سيبدأ قريباً');
    } catch (error) {
      toast.error('فشل تحميل التقرير، يرجى المحاولة لاحقاً');
      console.error(error);
    } finally {
      setIsDownloading(false);
    }
  };

  const formatDate = (date: string | null) => {
    if (!date) return 'غير محدد';
    return new Date(date).toLocaleDateString('ar-EG', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  // Delete Action Handlers
  const onDeleteHistoryAction = async () => {
    if (!lifeHistory?.id) return;
    try {
      const res = await deleteLifeHistory(lifeHistory.id);
      if (res.succeeded) toast.success('تم حذف السيرة الحياتية بنجاح');
      setDeleteWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف السيرة الحياتية: ${error}`);
    }
  };

  const onDeleteTrainingAction = async () => {
    if (!trainingCourse?.id) return;
    try {
      const res = await deleteTrainingCourse(trainingCourse.id);
      if (res.succeeded) toast.success('تم حذف الدورة التدريبية بنجاح');
      setDeleteTrainingWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف الدورة التدريبية: ${error}`);
    }
  };

  const onDeleteOperationAction = async () => {
    if (!operation?.id) return;
    try {
      const res = await deleteOperation(operation.id);
      if (res.succeeded) toast.success('تم حذف العملية بنجاح');
      setDeleteOperationWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف العملية: ${error}`);
    }
  };

  const onDeleteOrganizationAction = async () => {
    if (!organizationalAssignment?.id) return;
    try {
      const res = await deleteOrganizationalUnit(organizationalAssignment.id);
      if (res.succeeded) toast.success('تم حذف المنصب التنظيمي بنجاح');
      setDeleteOrganizationWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف المنصب التنظيمي: ${error}`);
    }
  };

  const onDeleteContactAction = async () => {
    if (!contact?.id) return;
    try {
      const res = await deleteContact(contact.id);
      if (res.succeeded) toast.success('تم حذف معلومات الاتصال بنجاح');
      setDeleteContactWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف معلومات الاتصال: ${error}`);
    }
  };

  const onDeleteAddressAction = async () => {
    if (!address?.id) return;
    try {
      const res = await deleteAddress(address.id);
      if (res.succeeded) toast.success('تم حذف العنوان بنجاح');
      setDeleteAddressWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف العنوان: ${error}`);
    }
  };

  const onDeleteRelationshipAction = async () => {
    if (!relationship?.id) return;
    try {
      const res = await deleteRelationship(suspect.id, relationship.id);
      if (res.succeeded) toast.success('تم حذف العلاقة بنجاح');
      setDeleteRelationshipWarningOpen(false);
    } catch (error) {
      toast.error(`فشل في حذف العلاقة: ${error}`);
    }
  };

  return (
    <div className="space-y-6">
      {/* Forms and Warnings */}
      <DeleteWarning onDelete={onDeleteHistoryAction} open={deleteWarningOpen} onOpenChange={setDeleteWarningOpen} title="حذف السيرة الحياتية" description="هل أنت متأكد أنك تريد حذف هذه السيرة الحياتية؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteTrainingAction} open={deleteTrainingWarningOpen} onOpenChange={setDeleteTrainingWarningOpen} title="حذف الدورة التدريبية" description="هل أنت متأكد أنك تريد حذف هذه الدورة التدريبية؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteOperationAction} open={deleteOperationWarningOpen} onOpenChange={setDeleteOperationWarningOpen} title="حذف العملية" description="هل أنت متأكد أنك تريد حذف هذه العملية؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteOrganizationAction} open={deleteOrganizationWarningOpen} onOpenChange={setDeleteOrganizationWarningOpen} title="حذف المنصب التنظيمي" description="هل أنت متأكد أنك تريد حذف هذا المنصب التنظيمي؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteContactAction} open={deleteContactWarningOpen} onOpenChange={setDeleteContactWarningOpen} title="حذف معلومات الاتصال" description="هل أنت متأكد أنك تريد حذف معلومات الاتصال هذه؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteAddressAction} open={deleteAddressWarningOpen} onOpenChange={setDeleteAddressWarningOpen} title="حذف العنوان" description="هل أنت متأكد أنك تريد حذف هذا العنوان؟ هذا الإجراء لا يمكن التراجع عنه." />
      <DeleteWarning onDelete={onDeleteRelationshipAction} open={deleteRelationshipWarningOpen} onOpenChange={setDeleteRelationshipWarningOpen} title="حذف العلاقة" description="هل أنت متأكد أنك تريد حذف هذه العلاقة؟ هذا الإجراء لا يمكن التراجع عنه." />

      <HistoryForm id={suspect.id} initialData={lifeHistory || undefined} open={historyFormOpen} onOpenChange={setHistoryFormOpen} title={lifeHistory ? "تعديل السيرة الحياتية" : "إضافة سيرة حياتية جديدة"} />
      <TrainingForm id={suspect.id} initialData={trainingCourse || undefined} open={trainingFormOpen} onOpenChange={setTrainingFormOpen} title={trainingCourse ? "تعديل الدورة التدريبية" : "إضافة دورة تدريبية جديدة"} />
      <OperationForm id={suspect.id} initialData={operation || undefined} open={operationFormOpen} onOpenChange={setOperationFormOpen} title={operation ? "تعديل العملية" : "إضافة عملية جديدة"} />
      <OrgAssignmentForm suspectId={suspect.id} open={organizationFormOpen} onOpenChange={setOrganizationFormOpen} title={organizationalAssignment ? "تعديل المنصب التنظيمي" : "إضافة منصب تنظيمي جديد"} />
      <ContactForm id={suspect.id} initialData={contact || undefined} open={contactFormOpen} onOpenChange={setContactFormOpen} title={contact ? "تعديل معلومات الاتصال" : "إضافة معلومات اتصال جديدة"} />
      <AddressForm id={suspect.id} initialData={address || undefined} open={addressFormOpen} onOpenChange={setAddressFormOpen} title={address ? "تعديل العنوان" : "إضافة عنوان جديد"} />
      <RelationshipForm id={suspect.id} initialData={relationship || undefined} open={relationshipFormOpen} onOpenChange={setRelationshipFormOpen} title={relationship ? "تعديل العلاقة" : "إضافة علاقة جديدة"} />
      <StatusForm id={suspect.id} initialStatus={String(suspect.currentStatus)} open={statusFormOpen} onOpenChange={setStatusFormOpen} title="تعديل حالة المشتبه به" />

      {/* Header Section */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex flex-col gap-6 md:flex-row md:items-start md:justify-between">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
              <Avatar className="h-24 w-24">
                <AvatarImage src={suspect.photoUrl} alt={suspect.fullName} />
                <AvatarFallback className="text-2xl">{getInitials()}</AvatarFallback>
              </Avatar>
              <div className="space-y-2">
                <h1 className="text-3xl font-bold">{suspect.fullName}</h1>
                {suspect.kunya && (
                  <p className="text-muted-foreground text-lg">{suspect.kunya}</p>
                )}
                <div className="flex flex-wrap items-center gap-2">
                  <Badge variant={suspect.currentStatus === 1 ? 'destructive' : 'secondary'}>
                    {suspect.currentStatusName || SuspectStatusLabels[suspect.currentStatus as keyof typeof SuspectStatusLabels] || 'غير معروف'}
                  </Badge>
                  {suspect.legalArticle && (
                    <Badge variant="outline">{suspect.legalArticle}</Badge>
                  )}
                </div>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <Link href={`/suspect/${suspect.id}/edit`}>
                <Button variant="outline">
                  <Edit className="h-4 w-4" />
                  تعديل
                </Button>
              </Link>
              <Button 
                variant="outline" 
                onClick={handleDownloadReport} 
                disabled={isDownloading}
                className="text-primary hover:text-primary"
              >
                {isDownloading ? (
                  <Loader2 className="h-4 w-4 animate-spin" />
                ) : (
                  <Download className="h-4 w-4" />
                )}
                تحميل التقرير
              </Button>
              <Button onClick={() => setStatusFormOpen(true)}>
                <StackIcon className="h-4 w-4" />
                تعديل الحالة
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Tabs Section */}
      <Tabs defaultValue="personal" className="w-full">
        <TabsList className="grid w-full grid-cols-2 lg:grid-cols-8">
          <TabsTrigger value="personal">
            <User className="h-4 w-4" />
            المعلومات الشخصية
          </TabsTrigger>
          <TabsTrigger value="network">
            <User className="h-4 w-4" />
            شبكة التقاطعات
          </TabsTrigger>
          <TabsTrigger value="contacts">
            <Phone className="h-4 w-4" />
            العناوين والاتصال
          </TabsTrigger>
          <TabsTrigger value="relations">
            <Users className="h-4 w-4" />
            الأقارب والمعارف
          </TabsTrigger>
          <TabsTrigger value="organizational">
            <Shield className="h-4 w-4" />
            التنظيمية
          </TabsTrigger>
          <TabsTrigger value="cases">
            <FileText className="h-4 w-4" />
            القضايا
          </TabsTrigger>
            <TabsTrigger value="interrogation-sessions">
            <FileText className="h-4 w-4" />
            جلسات الاستجواب
          </TabsTrigger>  
          <TabsTrigger value="evidence">
            <Shield className="h-4 w-4" />
            الأدلة والمرفقات
          </TabsTrigger>
        </TabsList>

        <TabsContent value="personal">
          <PersonalTab
            suspect={suspect}
            formatDate={formatDate}
            onAddHistory={() => { setLifeHistory(null); setHistoryFormOpen(true); }}
            onEditHistory={(h) => { setLifeHistory(h); setHistoryFormOpen(true); }}
            onDeleteHistory={(h) => { setLifeHistory(h); setDeleteWarningOpen(true); }}
          />
        </TabsContent>

        <TabsContent value="network">
          <NetworkTab suspectId={suspect.id} theme={theme} />
        </TabsContent>

        <TabsContent value="contacts">
          <ContactsTab
            suspect={suspect}
            onAddAddress={() => { setAddress(null); setAddressFormOpen(true); }}
            onEditAddress={(a) => { setAddress(a); setAddressFormOpen(true); }}
            onDeleteAddress={(a) => { setAddress(a); setDeleteAddressWarningOpen(true); }}
            onAddContact={() => { setContact(null); setContactFormOpen(true); }}
            onEditContact={(c) => { setContact(c); setContactFormOpen(true); }}
            onDeleteContact={(c) => { setContact(c); setDeleteContactWarningOpen(true); }}
          />
        </TabsContent>

        <TabsContent value="relations">
          <RelationsTab
            suspect={suspect}
            onAddRelationship={() => { setRelationship(null); setRelationshipFormOpen(true); }}
            onEditRelationship={(r) => { setRelationship(r); setRelationshipFormOpen(true); }}
            onDeleteRelationship={(r) => { setRelationship(r); setDeleteRelationshipWarningOpen(true); }}
          />
        </TabsContent>

        <TabsContent value="organizational">
          <OrganizationalTab
            suspect={suspect}
            formatDate={formatDate}
            onAddTraining={() => { setTrainingCourse(null); setTrainingFormOpen(true); }}
            onEditTraining={(c) => { setTrainingCourse(c); setTrainingFormOpen(true); }}
            onDeleteTraining={(c) => { setTrainingCourse(c); setDeleteTrainingWarningOpen(true); }}
            onAddOperation={() => { setOperation(null); setOperationFormOpen(true); }}
            onEditOperation={(o) => { setOperation(o); setOperationFormOpen(true); }}
            onDeleteOperation={(o) => { setOperation(o); setDeleteOperationWarningOpen(true); }}
            onAddOrganization={() => { setOrganizationalAssignment(null); setOrganizationFormOpen(true); }}
            onEditOrganization={(a) => { setOrganizationalAssignment(a); setOrganizationFormOpen(true); }}
            onDeleteOrganization={(a) => { setOrganizationalAssignment(a); setDeleteOrganizationWarningOpen(true); }}
          />
        </TabsContent>

        <TabsContent value="cases">
          <CasesTab suspect={suspect} formatDate={formatDate} />
        </TabsContent>

        <TabsContent value="interrogation-sessions">
          <InterrogationSessionsTab suspect={suspect} formatDate={formatDate} />
        </TabsContent>

        <TabsContent value="evidence">
          <EvidenceTab suspectId={suspect.id} />
        </TabsContent>
      </Tabs>

      {/* Metadata Footer */}
      <Card>
        <CardContent className="pt-6">
          <div className="flex items-center justify-between text-sm text-muted-foreground">
            <div className="flex items-center gap-2">
              <Calendar className="h-4 w-4" />
              <span>تاريخ الإنشاء: {formatDate(suspect.createdAt)}</span>
            </div>
            <div>
              <span>المعرف: {suspect.id}</span>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
