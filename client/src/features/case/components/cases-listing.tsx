
import { searchParamsCache } from '@/lib/searchparams';
import { CaseTable } from './cases-tables';
import { columns } from './cases-tables/columns';
import { getCases } from '../api/case.service';
import { CaseSummary } from '../types/case';

export default async function CaseListingPage() {
  const page = searchParamsCache.get('page');
  const search = searchParamsCache.get('name');
  const pageLimit = searchParamsCache.get('perPage');

  const filters = {
    page,
    pageSize: pageLimit,
    ...(search && { searchTerm: search })
  };

  const response = await getCases(filters);
  const totalCases = response?.data?.totalCount || 0;
  const cases: CaseSummary[] = response?.data?.items || [];


  return (
    <CaseTable
      data={cases}
      totalItems={totalCases}
      columns={columns}
    />
  );
}
