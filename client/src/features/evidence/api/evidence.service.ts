import { Response } from '@/types';
import { fetchClient } from '@/lib/fetch-client';
import {
    Evidence,
    CreateEvidenceRequest,
    UpdateEvidenceRequest,
    EvidenceQuery
} from '../types/evidence';

const BASE_URL = '/api/evidence';

export async function getEvidenceList(query: EvidenceQuery): Promise<Evidence[]> {
    const params = new URLSearchParams();
    if (query.page) params.append('Page', query.page.toString());
    if (query.pageSize) params.append('PageSize', query.pageSize.toString());

    const { data } = await fetchClient<Evidence[]>(`${BASE_URL}?${params.toString()}`, 'GET');
    return data || [];
}

export async function getEvidenceById(id: string): Promise<Response<Evidence>> {
    const { data, error } = await fetchClient<Response<Evidence>>(`${BASE_URL}/${id}`, 'GET');

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

export async function getEvidenceByCaseId(caseId: string): Promise<Evidence[]> {
    const { data } = await fetchClient<Evidence[]>(`${BASE_URL}/case/${caseId}`, 'GET');
    return data || [];
}

export async function getEvidenceBySuspectId(suspectId: string): Promise<Evidence[]> {
    const { data } = await fetchClient<Evidence[]>(`${BASE_URL}/suspect/${suspectId}`, 'GET');
    return data || [];
}

export async function createEvidence(request: CreateEvidenceRequest): Promise<Response<string>> {
    const formData = new FormData();
    formData.append('File', request.file);
    if (request.caseId) formData.append('CaseId', request.caseId);
    if (request.suspectId) formData.append('SuspectId', request.suspectId);
    if (request.description) formData.append('Description', request.description);

    const { data, error } = await fetchClient<Response<string>>(BASE_URL, 'POST', {
        body: formData
    });

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

export async function updateEvidence(request: UpdateEvidenceRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`${BASE_URL}/${request.id}`, 'PUT', {
        body: request
    });

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

export async function deleteEvidence(id: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`${BASE_URL}/${id}`, 'DELETE');

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
