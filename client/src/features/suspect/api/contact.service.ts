import { Response } from '@/types';
import { ContactRequest } from '../types/contact';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function createContact(body: ContactRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/contacts`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function updateContact(body: ContactRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/contacts`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteContact(contactId: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/contacts/${contactId}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}
