export enum CourseType {
    Sharia = 1,
    MilitaryBasic = 2,
    Explosives = 3,
    Sniping = 4,
    Intelligence = 5,
    Leadership = 6,
}

export const CourseTypeLabels: Record<CourseType, string> = {
    [CourseType.Sharia]: 'شرعية',
    [CourseType.MilitaryBasic]: 'عسكرية أساسية',
    [CourseType.Explosives]: 'هندسة وتفخيخ',
    [CourseType.Sniping]: 'قنص',
    [CourseType.Intelligence]: 'أمنية/استخبارات',
    [CourseType.Leadership]: 'إدارية/قيادة',
};

export interface TrainingCourseRequest {
    suspectId?: string
    courseType: CourseType
    location?: string
    trainerName?: string
    classmates?: any
    trainingCourseId?: string
}

export interface TrainingCourseDetail {
    id: string
    suspectId?: string
    courseType: CourseType
    location: any
    trainerName: any
    classmates: any
    createdAt?: string
}