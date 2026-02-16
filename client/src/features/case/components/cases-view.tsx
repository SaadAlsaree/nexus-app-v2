
import { notFound } from 'next/navigation';
import { getCaseById } from '../api/case.service';
import CaseForm from './case-form';
import CaseDetailView from './case-detail-view';

type TCasesViewPageProps = {
  caseId: string;
  mode?: 'view' | 'edit' | 'new';
};

export default async function CasesViewPage({
  caseId,
  mode = 'view'
}: TCasesViewPageProps) {
  let caseDetail = null;
  let pageTitle = 'إنشاء قضية جديدة';

  if (caseId !== 'new') {
    const response = await getCaseById(caseId);
    caseDetail = response.data;
    if (!caseDetail) {
      notFound();
    }
    pageTitle = mode === 'edit' ? `تعديل القضية` : `تفاصيل القضية`;
  }

  // Show form for new or edit mode
  if (caseId === 'new' || mode === 'edit') {
    return <CaseForm initialData={caseDetail} pageTitle={pageTitle} />;
  }

  // Show detail view for view mode
  if (!caseDetail) {
    notFound();
  }
  
  return <CaseDetailView caseDetail={caseDetail} />;
}
