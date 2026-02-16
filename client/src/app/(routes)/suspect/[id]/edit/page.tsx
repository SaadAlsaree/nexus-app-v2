// app/(routes)/suspect/[id]/edit/page.tsx
import PageContainer from '@/components/layout/page-container';
import SuspectsViewPage from '@/features/suspect/components/suspects-view';

export default async function Page({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  
  return (
    <PageContainer scrollable={true}>
      <SuspectsViewPage suspectId={id} mode="edit" />
    </PageContainer>
  );
}