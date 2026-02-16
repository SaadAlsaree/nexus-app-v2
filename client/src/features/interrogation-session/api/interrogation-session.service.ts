
import { Response, DataList } from '@/types';
import {
    InterrogationSession,
    InterrogationSessionSummary,
    InterrogationSessionQuery,
    CreateInterrogationSessionRequest,
    UpdateInterrogationSessionRequest
} from '../types/interrogation-session';
import { fetchClient } from '@/lib/fetch-client';



export async function getInterrogationSessions(query: InterrogationSessionQuery): Promise<Response<DataList<InterrogationSessionSummary>>> {
    const params = new URLSearchParams();
    if (query.page) params.append('Page', query.page.toString());
    if (query.pageSize) params.append('PageSize', query.pageSize.toString());
    if (query.searchTerm) params.append('SearchTerm', query.searchTerm);

    const { data, error } = await fetchClient<Response<DataList<InterrogationSessionSummary>>>(`/api/interrogation-sessions?${params.toString()}`, 'GET');

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

export async function getInterrogationSessionsBySuspectId(suspectId: string): Promise<Response<InterrogationSession[]>> {
    const { data, error } = await fetchClient<Response<InterrogationSession[]>>(`/api/suspects/${suspectId}/interrogation-sessions`, 'GET');

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

export async function getInterrogationSessionById(id: string): Promise<Response<InterrogationSession>> {
    const { data, error } = await fetchClient<Response<InterrogationSession>>(`/api/interrogation-sessions/${id}`, 'GET');

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

export async function createInterrogationSession(request: CreateInterrogationSessionRequest): Promise<Response<string>> {
    const { data, error } = await fetchClient<Response<string>>('/api/interrogation-sessions', 'POST', { body: request });

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

export async function updateInterrogationSession(request: UpdateInterrogationSessionRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/interrogation-sessions/${request.id}`, 'PUT', { body: request });

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

export async function deleteInterrogationSession(id: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/interrogation-sessions/${id}`, 'DELETE');

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

export async function downloadInterrogationSessionReport(id: string, suspectName: string, sessionDate: string) {
    const response = await fetch(process.env.NEXT_PUBLIC_API_URL + `/api/interrogation-sessions/${id}/report`, {
        method: 'GET',
    });

    if (!response.ok) {
        throw new Error('فشل تحميل التقرير');
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    
    // Format date for filename
    const dateStr = new Date(sessionDate).toISOString().split('T')[0];
    const safeName = suspectName.replace(/[^a-z0-9\u0600-\u06FF]/gi, '_').toLowerCase();
    
    a.download = `InterrogationSession_Report_${safeName}_${dateStr}.pdf`;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(a);
}
