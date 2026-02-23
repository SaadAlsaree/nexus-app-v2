import { Response } from '@/types';
import { ContactRequest } from '../types/contact';
import { fetchClient } from '@/lib/fetch-client';

export async function createContact(body: ContactRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/contacts', 'POST', { body });

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

export async function updateContact(body: ContactRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/contacts', 'PUT', { body });

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

export async function deleteContact(contactId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/contacts/${contactId}`, 'DELETE');

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
