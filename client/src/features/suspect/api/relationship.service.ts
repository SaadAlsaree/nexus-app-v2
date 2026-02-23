import { Response } from '@/types';
import { RelationshipRequest } from '../types/relationship';
import { fetchClient } from '@/lib/fetch-client';

export async function createRelationship(body: RelationshipRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/suspects/${body.suspectId}/relationships`, 'POST', { body });

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

export async function updateRelationship(body: RelationshipRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/suspects/${body.suspectId}/relationships/${body.relatedPersonId}`, 'PUT', { body });

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

export async function deleteRelationship(suspectId: string, relatedPersonId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/suspects/${suspectId}/relationships/${relatedPersonId}`, 'DELETE');

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
