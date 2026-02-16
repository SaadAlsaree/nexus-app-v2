import PageContainer from '@/components/layout/page-container';
import InterrogationSessionViewPage from '@/features/interrogation-session/components/interrogation-session-view';
import { Suspense } from 'react';
import { getSuspects } from '@/features/suspect/api/suspect.service';
import { getCases } from '@/features/case/api/case.service';

export const metadata = {
  title: 'Dashboard: Create Interrogation Session'
};

type PageProps = {
  searchParams: Promise<{ suspectId?: string; caseId?: string }>;
};

export default async function Page({ searchParams }: PageProps) {
  const { suspectId, caseId } = await searchParams;

  const [suspectsRes, casesRes] = await Promise.all([
    getSuspects({ pageSize: 1000 }),
    getCases({ pageSize: 1000 })
  ]);

  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
        <InterrogationSessionViewPage 
          sessionId="new" 
          mode="new" 
          suspectId={suspectId} 
          caseId={caseId} 
          suspects={suspectsRes.data?.items || []}
          cases={casesRes.data?.items || []}
        />
      </Suspense>
    </PageContainer>
  );
}
