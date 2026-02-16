import { searchParamsCache } from '@/lib/searchparams';
import { SuspectTable } from './suspects-tables';
import { columns } from './suspects-tables/columns';
import { getSuspects } from '../api/suspect.service';
import { Suspect } from '../types/suspect';

export default async function SuspectListingPage() {
  // Showcasing the use of search params cache in nested RSCs
  const page = searchParamsCache.get('page');
  const search = searchParamsCache.get('name');
  const pageLimit = searchParamsCache.get('perPage');

  const filters = {
    page,
    pageSize: pageLimit,
    ...(search && { searchTerm: search })
  };

  const response = await getSuspects(filters);
  const totalSuspects = response?.data?.totalCount || 0;
  const suspects: Suspect[] = response?.data?.items || [];

  return (
    <SuspectTable
      data={suspects}
      totalItems={totalSuspects}
      columns={columns}
    />
  );
}
