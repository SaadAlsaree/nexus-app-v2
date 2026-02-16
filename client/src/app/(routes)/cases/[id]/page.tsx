import PageContainer from '@/components/layout/page-container';
import CasesViewPage from '@/features/case/components/cases-view';
import { Suspense } from 'react';

export const metadata = {
  title: 'Dashboard: Case View'
};

type PageProps = {
  params: Promise<{ id: string }>;
};

export default async function Page({ params }: PageProps) {
  const { id } = await params;

  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
         <CasesViewPage caseId={id} />
      </Suspense>
    </PageContainer>
  );
}
