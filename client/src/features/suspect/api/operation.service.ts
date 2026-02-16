import { Response, DataList } from '@/types';
import { OperationRequest } from '../types/operation';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function createOperation(body: OperationRequest): Promise<Response<void>> {
    try {
        const response = await fetch(`${API_URL}/api/operations`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });
        const data = (await response.json()) as Response<void>;
        return data;
    } catch (error) {
        console.error('Error creating operation:', error);
        throw error;
    }

}

export async function updateOperation(body: OperationRequest): Promise<Response<void>> {
    try {
        const response = await fetch(`${API_URL}/api/operations`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });
        const data = (await response.json()) as Response<void>;
        return data;
    } catch (error) {
        console.error('Error updating operation:', error);
        throw error;
    }
}

export async function deleteOperation(operationId: string): Promise<Response<void>> {
    try {
        const response = await fetch(`${API_URL}/api/operations/${operationId}`, {
            method: 'DELETE',
        });
        const data = (await response.json()) as Response<void>;
        return data;
    } catch (error) {
        console.error('Error deleting operation:', error);
        throw error;
    }
}