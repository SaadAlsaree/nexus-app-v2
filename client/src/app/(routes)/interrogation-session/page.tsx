import PageContainer from "@/components/layout/page-container";
import { buttonVariants } from "@/components/ui/button";
import { DataTableSkeleton } from "@/components/ui/table/data-table-skeleton";
import InterrogationSessionListingPage from "@/features/interrogation-session/components/interrogation-sessions-listing";
import { cn } from "@/lib/utils";
import { IconPlus } from "@tabler/icons-react";
import Link from "next/link";
import { Suspense } from "react";
import { notFound } from "next/navigation";

import { SearchParams } from "nuqs/server";
import { searchParamsCache } from "@/lib/searchparams";

export const metadata = {
  title: 'Dashboard: Interrogation Sessions'
};

type PageProps = {
  searchParams: Promise<SearchParams>;
};

export default async function Page(props: PageProps) {
  const searchParams = await props.searchParams;
  searchParamsCache.parse(searchParams);

  const suspectId = searchParams.suspectId as string | undefined;
  const page = searchParamsCache.get('page');
  const searchTerm = searchParamsCache.get('name') || undefined;
  const pageSize = searchParamsCache.get('perPage');

  return (
    <PageContainer
      scrollable={false}
      pageTitle='جلسات الاستجواب'
      pageDescription='إدارة جلسات الاستجواب'
      pageHeaderAction={
        (
          <Link
            href={`/interrogation-session/new?suspectId=${suspectId}`}
            className={cn(buttonVariants(), 'text-xs md:text-sm')}
          >
            <IconPlus className='mr-2 h-4 w-4' /> إضافة جديد
          </Link>
        )
      }
    >
      <Suspense
        fallback={
          <DataTableSkeleton columnCount={8} rowCount={10} />
        }
      >
        <InterrogationSessionListingPage 
          suspectId={suspectId} 
          page={page}
          pageSize={pageSize}
          searchTerm={searchTerm}
        />
      </Suspense>
    </PageContainer>
  )
}
