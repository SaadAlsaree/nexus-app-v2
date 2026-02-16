import PageContainer from '@/components/layout/page-container';
import CasesViewPage from '@/features/case/components/cases-view';
import { Suspense } from 'react';

export const metadata = {
  title: 'Dashboard: Create Case'
};

export default function Page() {
  return (
    <PageContainer scrollable={true}>
      <Suspense fallback={<div>Loading...</div>}>
        <CasesViewPage caseId="new" />
      </Suspense>
    </PageContainer>
  );
}
