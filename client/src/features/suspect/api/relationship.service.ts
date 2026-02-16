import { Response } from '@/types';
import { RelationshipRequest } from '../types/relationship';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function createRelationship(body: RelationshipRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/suspects/${body.suspectId}/relationships`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function updateRelationship(body: RelationshipRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/suspects/${body.suspectId}/relationships/${body.relatedPersonId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteRelationship(suspectId: string, relatedPersonId: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/suspects/${suspectId}/relationships/${relatedPersonId}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}
