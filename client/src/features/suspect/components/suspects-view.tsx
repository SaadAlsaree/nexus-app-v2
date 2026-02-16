import { notFound } from 'next/navigation';
import { getSuspectById } from '../api/suspect.service';
import SuspectsForm from './suspects-form';
import SuspectDetailView from './suspect-detail-view';

type TSuspectsViewPageProps = {
  suspectId: string;
  mode?: 'view' | 'edit' | 'new';
};

export default async function SuspectsViewPage({
  suspectId,
  mode = 'view'
}: TSuspectsViewPageProps) {
  let suspect = null;
  let pageTitle = 'إنشاء متهم جديد';

  if (suspectId !== 'new') {
    const response = await getSuspectById(suspectId);
    suspect = response.data;
    if (!suspect) {
      notFound();
    }
    pageTitle = mode === 'edit' ? `تعديل المتهم` : `تفاصيل المتهم`;
  }

  // Show form for new or edit mode
  if (suspectId === 'new' || mode === 'edit') {
    return <SuspectsForm initialData={suspect} pageTitle={pageTitle} />;
  }

  // Show detail view for view mode
  if (!suspect) {
    notFound();
  }
  
  return <SuspectDetailView suspect={suspect} />;
}
