import PageContainer from "@/components/layout/page-container"
import { buttonVariants } from "@/components/ui/button";
import { DataTableSkeleton } from "@/components/ui/table/data-table-skeleton";
import { getSuspects } from "@/features/suspect/api/suspect.service";
import SuspectListingPage from "@/features/suspect/components/suspects-listing";
import { SuspectTable } from "@/features/suspect/components/suspects-tables";
import { searchParamsCache } from "@/lib/searchparams";
import { cn } from "@/lib/utils";
import { IconPlus } from "@tabler/icons-react";
import Link from "next/link";
import { SearchParams } from "nuqs/server";
import { Suspense } from "react";


export const metadata = {
  title: 'Dashboard: Suspects'
};

type PageProps = {
  searchParams: Promise<SearchParams>;
};


export default async function Page(props: PageProps) {
  const searchParams = await props.searchParams;
  // Allow nested RSCs to access the search params (in a type-safe way)
  searchParamsCache.parse(searchParams);

  return (
     <PageContainer
      scrollable={false}
      pageTitle='المتهمين'
      pageDescription='إدارة المتهمين'
      pageHeaderAction={
        <Link
          href='/suspect/new'
          className={cn(buttonVariants(), 'text-xs md:text-sm')}
        >
          <IconPlus className='mr-2 h-4 w-4' /> إضافة جديد
        </Link>
      }
    >
      <Suspense
        // key={key}
        fallback={
          <DataTableSkeleton columnCount={5} rowCount={8} filterCount={2} />
        }
      >
        <SuspectListingPage />
      </Suspense>
    </PageContainer>
  )
}
