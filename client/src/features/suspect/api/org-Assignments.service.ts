import { Response } from "@/types";
import { OrgAssignmentRequest } from "../types/org-assignment";

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function createOrgAssignment(body: OrgAssignmentRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/org-assignments`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteOrgAssignment(id: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/org-assignments/${id}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}