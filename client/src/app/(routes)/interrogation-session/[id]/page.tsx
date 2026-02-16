import PageContainer from '@/components/layout/page-container';
import InterrogationSessionViewPage from '@/features/interrogation-session/components/interrogation-session-view';
import { Suspense } from 'react';

export const metadata = {
  title: 'Dashboard: Interrogation Session View'
};

type PageProps = {
  params: Promise<{ id: string }>;
};

export default async function Page({ params }: PageProps) {
  const { id } = await params;

  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
         <InterrogationSessionViewPage sessionId={id} mode="view" />
      </Suspense>
    </PageContainer>
  );
}
