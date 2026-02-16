import { notFound } from "next/navigation";


export async function fetchClient<T>(
    url: string,
    method: 'GET' | 'POST' | 'PUT' | 'DELETE',
    options: Omit<RequestInit, 'body'> & { body?: unknown } = {}
): Promise<{ data: T | null; error?: { message: string; status: number } }> {
    const { body, ...rest } = options;

    // Strict separation: Server uses internal Docker network, Client uses public URL
    const isServer = typeof window === 'undefined';
    const apiUrl = (isServer && process.env.API_URL) || process.env.NEXT_PUBLIC_API_URL || '';

    // On server-side, we need a full URL
    if (isServer && !apiUrl) throw new Error('Missing API_URL for server-side rendering');

    const isFormData = body instanceof FormData;
    const headers: HeadersInit = {
        ...(isFormData ? {} : { 'Content-Type': 'application/json' }),
        ...(rest.headers || {})
    }

    let response: globalThis.Response;
    try {
        response = await fetch(apiUrl + url, {
            method,
            headers,
            body: isFormData ? (body as any) : (body ? JSON.stringify(body) : undefined),
            ...rest,
        });
    } catch (err: any) {
        console.error('Fetch error:', err);
        return { data: null, error: { message: 'Network error or service unavailable', status: 503 } };
    }

    const contentType = response.headers.get('Content-Type');
    const isJson = contentType?.includes('application/json')
        || contentType?.includes('application/problem+json');

    const parsed = isJson ? await response.json() : await response.text();

    if (!response.ok) {
        console.log(response);
        if (response.status === 404) return notFound();
        // if (response.status === 500) throw new Error("Server error. Please try again later.");

        let message = '';

        if (response.status === 401) {
            const authHeader = response.headers.get('WWW-Authenticate');
            if (authHeader?.includes('error_description')) {
                const match = authHeader.match(/error_description="(.+?)"/);
                if (match) message = match[1];
            } else {
                message = "You must be logged in to do that"
            }
        }

        if (!message) {
            if (typeof parsed === 'string') {
                message = parsed
            } else if (parsed?.message) {
                message = parsed?.message;
            } else {
                message = getFallbackMessage(response.status)
            }
        }

        return { data: null, error: { message, status: response.status } };
    }

    return { data: parsed as T };
}

function getFallbackMessage(status: number): string {
    switch (status) {
        case 400: return 'Bad request. Please check your input.';
        case 403: return 'You do not have permission to access this resource.';
        case 500: return 'Server error. Please try again later.';
        default: return 'An unexpected error occurred.';
    }
}