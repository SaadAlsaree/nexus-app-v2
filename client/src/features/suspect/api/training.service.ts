import { Response, DataList } from '@/types';
import { TrainingCourseDetail, TrainingCourseRequest } from '../types/training';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function getAllTrainingCourses(suspectId: string): Promise<Response<DataList<TrainingCourseDetail[]>>> {
    const response = await fetch(`${API_URL}/api/suspects/${suspectId}/training-courses`);
    if (!response.ok) {
        throw new Error(`Failed to fetch suspects: ${response.statusText}`);
    }
    const contentType = response.headers.get("content-type");
    if (!contentType || !contentType.includes("application/json")) {
        throw new Error("Received non-JSON response from server");
    }
    const data = (await response.json()) as Response<DataList<TrainingCourseDetail[]>>;
    return data;
}

export async function createTrainingCourse(body: TrainingCourseRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/training-courses`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function updateTrainingCourse(body: TrainingCourseRequest): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/training-courses`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    });
    const data = (await response.json()) as Response<void>;
    return data;
}

export async function deleteTrainingCourse(trainingCourseId: string): Promise<Response<void>> {
    const response = await fetch(`${API_URL}/api/training-courses/${trainingCourseId}`, {
        method: 'DELETE',
    });
    const data = (await response.json()) as Response<void>;
    return data;
}