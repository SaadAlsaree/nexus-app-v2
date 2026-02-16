import { Response, DataList } from '@/types';
import {
    Suspect,
    SuspectQuery,
    SuspectDetails,
    CreateSuspectRequest,
    UpdateSuspectRequest,
    UpdateSuspectStatusRequest,
    NetworkSuspect,
    LifeHistoryRequest
} from '../types/suspect';
import { fetchClient } from '@/lib/fetch-client';


export async function getSuspects(query: SuspectQuery): Promise<Response<DataList<Suspect>>> {
    const params = new URLSearchParams();
    if (query.page) params.append('Page', query.page.toString());
    if (query.pageSize) params.append('PageSize', query.pageSize.toString());
    if (query.searchTerm) params.append('SearchTerm', query.searchTerm);

    const { data, error } = await fetchClient<Response<DataList<Suspect>>>(`/api/suspects?${params.toString()}`, 'GET');

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

export async function getSuspectById(id: string): Promise<Response<SuspectDetails>> {
    const { data, error } = await fetchClient<Response<SuspectDetails>>(`/api/suspects/${id}`, 'GET');

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

export async function getSuspectNetwork(id: string): Promise<Response<NetworkSuspect>> {
    const { data, error } = await fetchClient<Response<NetworkSuspect>>(`/api/suspects/${id}/network`, 'GET');

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

export async function createSuspect(request: CreateSuspectRequest): Promise<Response<string>> {
    const { data, error } = await fetchClient<Response<string>>('/api/suspects', 'POST', { body: request });

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

export async function updateSuspect(request: UpdateSuspectRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/suspects/${request.suspectId}`, 'PUT', { body: request });

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

export async function updateSuspectStatus(request: UpdateSuspectStatusRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/suspects/${request.suspectId}/status`, 'PUT', { body: request });

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

export async function createLifeHistory(values: LifeHistoryRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/life-histories', 'POST', { body: values });

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

export async function updateLifeHistory(values: LifeHistoryRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/life-histories', 'PUT', { body: values });

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

export async function deleteLifeHistory(lifeHistoryId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/life-histories/${lifeHistoryId}`, 'DELETE');

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

export async function downloadSuspectReport(id: string): Promise<void> {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL;
    const response = await fetch(`${apiUrl}/api/suspects/${id}/report`, {
        method: 'GET',
    });

    if (!response.ok) {
        throw new Error('فشل تحميل التقرير');
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    
    const contentDisposition = response.headers.get('Content-Disposition');
    let filename = `suspect_report_${id.substring(0, 8)}.pdf`;
    
    if (contentDisposition) {
        const filenameMatch = contentDisposition.match(/filename\*?=['"]?(?:UTF-8'')?([^'";\n]+)['"]?/i);
        if (filenameMatch && filenameMatch[1]) {
            filename = decodeURIComponent(filenameMatch[1]);
        } else {
            const fallbackMatch = contentDisposition.match(/filename=['"]?([^'";\n]+)['"]?/i);
            if (fallbackMatch && fallbackMatch[1]) {
                filename = fallbackMatch[1];
            }
        }
    }

    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
}