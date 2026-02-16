import PageContainer from '@/components/layout/page-container';
import InterrogationSessionViewPage from '@/features/interrogation-session/components/interrogation-session-view';
import { Suspense } from 'react';
import { getSuspects } from '@/features/suspect/api/suspect.service';
import { getCases } from '@/features/case/api/case.service';

export const metadata = {
  title: 'Interrogation Session Edit'
};

type PageProps = {
  params: Promise<{ id: string }>;
};

export default async function Page({ params }: PageProps) {
  const { id } = await params;

  const [suspectsRes, casesRes] = await Promise.all([
    getSuspects({ pageSize: 1000 }),
    getCases({ pageSize: 1000 })
  ]);

  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
         <InterrogationSessionViewPage 
            sessionId={id} 
            mode="edit" 
            suspects={suspectsRes.data?.items || []}
            cases={casesRes.data?.items || []}
         />
      </Suspense>
    </PageContainer>
  );
}
