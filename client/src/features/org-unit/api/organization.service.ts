import { Response, DataList } from '@/types';
import { OrganizationalUnitRequest, OrganizationalUnitDetail, OrgUnitHierarchyNode, OrgUnitListItem } from '../types/organization';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function getOrganizationalUnits(): Promise<Response<DataList<OrgUnitListItem>>> {
    const response = await fetch(`${API_URL}/api/org-units`);
    if (!response.ok) {
        throw new Error(`Failed to fetch organizational units: ${response.statusText}`);
    }
    const contentType = response.headers.get("content-type");
    if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Received non-JSON response from server");
    }
    const data = (await response.json()) as Response<DataList<OrgUnitListItem>>;
    return data;
}

export async function createOrganizationalUnit(body: OrganizationalUnitRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/org-units`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;

}

export async function updateOrganizationalUnit(body: OrganizationalUnitRequest & { unitId: string }): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/org-units`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteOrganizationalUnit(unitId: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/org-units/${unitId}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function getOrganizationalUnitById(unitId: string): Promise<Response<OrganizationalUnitDetail>> {
    const response = await fetch(`${API_URL}/api/org-units/${unitId}`);
    if (!response.ok) {
        throw new Error(`Failed to fetch organizational unit: ${response.statusText}`);
    }
    const data = (await response.json()) as Response<OrganizationalUnitDetail>;
    return data;
}

export async function getOrganizationalUnitsHierarchy(): Promise<Response<OrgUnitHierarchyNode[]>> {
    const response = await fetch(`${API_URL}/api/org-units/hierarchy`);
    if (!response.ok) {
        throw new Error(`Failed to fetch organizational units hierarchy: ${response.statusText}`);
    }
    const data = (await response.json()) as Response<OrgUnitHierarchyNode[]>;
    return data;
}