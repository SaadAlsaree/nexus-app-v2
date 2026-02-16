
import { Response, DataList } from '@/types';
import {
    CaseSummary,
    CaseDetails,
    CreateCaseRequest,
    UpdateCaseRequest,
    AddSuspectToCaseRequest,
    RemoveSuspectFromCaseRequest,
    CaseQuery
} from '../types/case';
import { fetchClient } from '@/lib/fetch-client';

export async function getCases(query: CaseQuery): Promise<Response<DataList<CaseSummary>>> {
    const params = new URLSearchParams();
    if (query.page) params.append('Page', query.page.toString());
    if (query.pageSize) params.append('PageSize', query.pageSize.toString());
    if (query.searchTerm) params.append('SearchTerm', query.searchTerm);

    const { data, error } = await fetchClient<Response<DataList<CaseSummary>>>(`/api/cases?${params.toString()}`, 'GET');

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

export async function getCaseById(id: string): Promise<Response<CaseDetails>> {
    const { data, error } = await fetchClient<Response<CaseDetails>>(`/api/cases/${id}`, 'GET');

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

export async function createCase(request: CreateCaseRequest): Promise<Response<string>> {
    const { data, error } = await fetchClient<Response<string>>('/api/cases', 'POST', { body: request });

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

export async function updateCase(request: UpdateCaseRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/cases', 'PUT', { body: request });

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


export async function deleteCase(id: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/cases/${id}`, 'DELETE');

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

export async function addSuspectToCase(request: AddSuspectToCaseRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/cases/${request.caseId}/suspects`, 'POST', { body: request });

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

export async function removeSuspectFromCase(request: RemoveSuspectFromCaseRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/cases/${request.caseId}/suspects/${request.suspectId}`, 'DELETE');

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

