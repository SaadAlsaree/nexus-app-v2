import { Response } from '@/types';
import { AddressRequest } from '../types/address';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function createAddress(body: AddressRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/addresses`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function updateAddress(body: AddressRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/addresses`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteAddress(addressId: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/addresses/${addressId}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}
