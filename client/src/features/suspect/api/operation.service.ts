import { Response, DataList } from '@/types';
import { OperationRequest } from '../types/operation';
import { fetchClient } from '@/lib/fetch-client';

export async function createOperation(body: OperationRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/operations', 'POST', { body });

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

export async function updateOperation(body: OperationRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/operations', 'PUT', { body });

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

export async function deleteOperation(operationId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/operations/${operationId}`, 'DELETE');

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