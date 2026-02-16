
import { notFound } from 'next/navigation';
import { getInterrogationSessionById } from '../api/interrogation-session.service';
import InterrogationSessionForm from './interrogation-session-form';
import InterrogationSessionDetailView from './interrogation-session-detail-view';
import { Suspect } from '../../suspect/types/suspect';
import { CaseSummary } from '../../case/types/case';

type TInterrogationSessionViewPageProps = {
  sessionId: string;
  mode?: 'view' | 'edit' | 'new';
  suspectId?: string;
  caseId?: string;
  suspects?: Suspect[];
  cases?: CaseSummary[];
};

export default async function InterrogationSessionViewPage({
  sessionId,
  mode = 'view',
  suspectId,
  caseId,
  suspects = [],
  cases = []
}: TInterrogationSessionViewPageProps) {
  let sessionDetail = null;
  let pageTitle = 'إنشاء جلسة استجواب جديدة';

  if (sessionId !== 'new') {
    const response = await getInterrogationSessionById(sessionId);
    sessionDetail = response.data;
    if (!sessionDetail) {
      notFound();
    }
    pageTitle = mode === 'edit' ? `تعديل جلسة الاستجواب` : `تفاصيل جلسة الاستجواب`;
  }

  // Show form for new or edit mode
  if (sessionId === 'new' || mode === 'edit') {
    return (
      <InterrogationSessionForm 
        initialData={sessionDetail} 
        pageTitle={pageTitle} 
        suspectId={suspectId || sessionDetail?.suspectId}
        caseId={caseId || sessionDetail?.caseId}
        suspects={suspects}
        cases={cases}
      />
    );
  }

  // Show detail view for view mode
  if (!sessionDetail) {
    notFound();
  }
  
  return <InterrogationSessionDetailView session={sessionDetail} />;
}
