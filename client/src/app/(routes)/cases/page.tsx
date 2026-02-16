import PageContainer from "@/components/layout/page-container";
import { buttonVariants } from "@/components/ui/button";
import { DataTableSkeleton } from "@/components/ui/table/data-table-skeleton";
import CaseListingPage from "@/features/case/components/cases-listing";
import { searchParamsCache } from "@/lib/searchparams";
import { cn } from "@/lib/utils";
import { IconPlus } from "@tabler/icons-react";
import Link from "next/link";
import { SearchParams } from "nuqs/server";
import { Suspense } from "react";

export const metadata = {
  title: 'Dashboard: Cases'
};

type PageProps = {
  searchParams: Promise<SearchParams>;
};

export default async function Page(props: PageProps) {
  const searchParams = await props.searchParams;
  searchParamsCache.parse(searchParams);

  return (
    <PageContainer
      scrollable={false}
      pageTitle='القضايا'
      pageDescription='إدارة القضايا'
      pageHeaderAction={
        <Link
          href='/cases/new'
          className={cn(buttonVariants(), 'text-xs md:text-sm')}
        >
          <IconPlus className='mr-2 h-4 w-4' /> إضافة جديد
        </Link>
      }
    >
      <Suspense
        fallback={
          <DataTableSkeleton columnCount={5} rowCount={10} />
        }
      >
        <CaseListingPage />
      </Suspense>
    </PageContainer>
  )
}