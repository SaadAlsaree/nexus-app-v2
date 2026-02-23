import { Response } from "@/types";
import { OrgAssignmentRequest } from "../types/org-assignment";
import { fetchClient } from '@/lib/fetch-client';

export async function createOrgAssignment(body: OrgAssignmentRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/org-assignments', 'POST', { body });

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

export async function deleteOrgAssignment(id: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/org-assignments/${id}`, 'DELETE');

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