import PageContainer from '@/components/layout/page-container';
import SuspectsViewPage from '@/features/suspect/components/suspects-view';
import { Suspense } from 'react';

export const metadata = {
  title: 'Dashboard: Suspect View'
};

type PageProps = {
  params: Promise<{ id: string }>;
};

export default async function Page({ params }: PageProps) {
  const { id } = await params;

  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
        <SuspectsViewPage suspectId={id} />
      </Suspense>
    </PageContainer>
  );
}
