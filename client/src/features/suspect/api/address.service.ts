import { Response } from '@/types';
import { AddressRequest } from '../types/address';
import { fetchClient } from '@/lib/fetch-client';

export async function createAddress(body: AddressRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/addresses', 'POST', { body });

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

export async function updateAddress(body: AddressRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/addresses', 'PUT', { body });

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

export async function deleteAddress(addressId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/addresses/${addressId}`, 'DELETE');

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
