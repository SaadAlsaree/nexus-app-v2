import { searchParamsCache } from '@/lib/searchparams';
import { OrgUnitTable } from './organizations-tabls';
import { getOrganizationalUnits } from '../api/organization.service';
import { OrgUnitListItem } from '../types/organization';
import { columns } from './organizations-tabls/columns';
import PageContainer from '@/components/layout/page-container';



export default async function OrgUnitsTablePage() {
  // Showcasing the use of search params cache in nested RSCs
  const page = searchParamsCache.get('page');
  const search = searchParamsCache.get('name');
  const pageLimit = searchParamsCache.get('perPage');

  const filters = {
    page,
    pageSize: pageLimit,
    ...(search && { searchTerm: search })
  };

  const response = await getOrganizationalUnits();
  const totalUnits = response?.data?.totalCount || 0;
  const units: OrgUnitListItem[] = response?.data?.items || [];

  return (
    <OrgUnitTable
      data={units}
      totalItems={totalUnits}
      columns={columns}
    />
  );
}
