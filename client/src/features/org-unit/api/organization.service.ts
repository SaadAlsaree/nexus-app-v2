import { Response, DataList } from '@/types';
import { OrganizationalUnitRequest, OrganizationalUnitDetail, OrgUnitHierarchyNode, OrgUnitListItem } from '../types/organization';
import { fetchClient } from '@/lib/fetch-client';

export async function getOrganizationalUnits(): Promise<Response<DataList<OrgUnitListItem>>> {
    const { data, error } = await fetchClient<Response<DataList<OrgUnitListItem>>>('/api/org-units', 'GET');
    
    if (error) {
        throw new Error(`Failed to fetch organizational units: ${error.message}`);
    }
    
    return data!;
}

export async function createOrganizationalUnit(body: OrganizationalUnitRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/org-units', 'POST', { body });

    if (error) {
        return {
            succeeded: false,
            data: null,
            message: error.message,
            code: error.status.toString(),
            errors: []
        };
    }

    return data!;
}

export async function updateOrganizationalUnit(body: OrganizationalUnitRequest & { unitId: string }): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/org-units', 'PUT', { body });

    if (error) {
        return {
            succeeded: false,
            data: null,
            message: error.message,
            code: error.status.toString(),
            errors: []
        };
    }

    return data!;
}

export async function deleteOrganizationalUnit(unitId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/org-units/${unitId}`, 'DELETE');

    if (error) {
        return {
            succeeded: false,
            data: null,
            message: error.message,
            code: error.status.toString(),
            errors: []
        };
    }

    return data!;
}

export async function getOrganizationalUnitById(unitId: string): Promise<Response<OrganizationalUnitDetail>> {
    const { data, error } = await fetchClient<Response<OrganizationalUnitDetail>>(`/api/org-units/${unitId}`, 'GET');
    
    if (error) {
        throw new Error(`Failed to fetch organizational unit: ${error.message}`);
    }
    
    return data!;
}

export async function getOrganizationalUnitsHierarchy(): Promise<Response<OrgUnitHierarchyNode[]>> {
    const { data, error } = await fetchClient<Response<OrgUnitHierarchyNode[]>>('/api/org-units/hierarchy', 'GET');
    
    if (error) {
        throw new Error(`Failed to fetch organizational units hierarchy: ${error.message}`);
    }
    
    return data!;
}