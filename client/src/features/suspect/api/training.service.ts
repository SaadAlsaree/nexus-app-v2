import { Response, DataList } from '@/types';
import { TrainingCourseDetail, TrainingCourseRequest } from '../types/training';
import { fetchClient } from '@/lib/fetch-client';

export async function getAllTrainingCourses(suspectId: string): Promise<Response<DataList<TrainingCourseDetail[]>>> {
    const { data, error } = await fetchClient<Response<DataList<TrainingCourseDetail[]>>>(`/api/suspects/${suspectId}/training-courses`, 'GET');

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

export async function createTrainingCourse(body: TrainingCourseRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/training-courses', 'POST', { body });

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

export async function updateTrainingCourse(body: TrainingCourseRequest): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>('/api/training-courses', 'PUT', { body });

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

export async function deleteTrainingCourse(trainingCourseId: string): Promise<Response<void>> {
    const { data, error } = await fetchClient<Response<void>>(`/api/training-courses/${trainingCourseId}`, 'DELETE');

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
