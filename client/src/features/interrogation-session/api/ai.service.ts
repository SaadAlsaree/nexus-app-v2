export interface Data {
    language: string
    segments: Segment[]
}

export interface Segment {
    speaker: string
    start: number
    end: number
    text: string
}

export interface LLMResponse {
    id?: string;
    object?: string;
    created?: number;
    model?: string;
    choices?: Choice[];
    usage?: Usage;
}

export interface Choice {
    index?: number;
    message?: Message;
    finish_reason?: string;
}

export interface Message {
    role?: string;
    content?: string;
}

export interface Usage {
    prompt_tokens?: number;
    completion_tokens?: number;
    total_tokens?: number;
}

const AI_API_URL = process.env.NEXT_PUBLIC_AI_API_URL || 'http://localhost:8005/v1';
const LLM_API_URL = process.env.NEXT_PUBLIC_LLM_API_URL || 'http://localhost:8002/v1';
const AI_BEARER_TOKEN = 'sk-inv-ai-2026-x7k9m2p4q8r1s5t3';
const AI_LLM_BEARER_TOKEN = 'sk-inv-ai-9c7c4cf98971258899a9bcf5082609c4';

/**
 * Transcribes and diarizes an audio file using the AI service.
 * 
 * @param file The audio file to transcribe
 * @param language The language of the audio (default: 'ar')
 * @param numSpeakers The number of speakers (default: 2)
 * @returns Promise with the transcribed data and segments
 */
export async function transcribeDiarize(
    file: File | Blob,
    language: string = 'ar',
    numSpeakers: number = 2
): Promise<Data> {
    const formData = new FormData();
    formData.append('file', file, 'recording.wav');
    formData.append('language', language);
    formData.append('num_speakers', numSpeakers.toString());

    const response = await fetch(`${AI_API_URL}/audio/transcribe-diarize`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${AI_BEARER_TOKEN}`,
            // Note: Do not set 'Content-Type': 'multipart/form-data'. 
            // The browser will automatically set it with the correct boundary.
        },
        body: formData,
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`AI Service Error (${response.status}): ${errorText || response.statusText}`);
    }

    return await response.json();
}

// /chat/completions
export async function LLMQuery(
    query: string
): Promise<LLMResponse> {
    const response = await fetch(`${LLM_API_URL}/chat/completions`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${AI_LLM_BEARER_TOKEN}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            model: "qwen2.5-14b-instruct",
            prompt_type: "summary",
            messages: [
                {
                    role: "user",
                    content: query
                }
            ],
            temperature: 0.3,
            max_tokens: 1000
        }),
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`AI Service Error (${response.status}): ${errorText || response.statusText}`);
    }

    return await response.json();
}