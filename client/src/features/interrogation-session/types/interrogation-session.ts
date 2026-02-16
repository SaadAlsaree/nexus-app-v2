
export enum InterrogationType {
    InitialInquiry = 1,
    DetailedConfession = 2,
    Confrontation = 3,
    CrimeSceneReenactment = 4,
    JudicialRatification = 5
}

export const InterrogationTypeLabels: Record<InterrogationType, string> = {
    [InterrogationType.InitialInquiry]: 'استجواب أولي',
    [InterrogationType.DetailedConfession]: 'تدوين إفادة تفصيلية',
    [InterrogationType.Confrontation]: 'مواجهة مع متهم آخر',
    [InterrogationType.CrimeSceneReenactment]: 'كشف دلالة (تمثيل الجريمة)',
    [InterrogationType.JudicialRatification]: 'تصديق أقوال أمام القاضي'
};

export enum InterrogationOutcome {
    FullConfession = 1,
    PartialAdmission = 2,
    Denial = 3,
    ProvidedIntelligence = 4,
    Silent = 5,
    Contradictory = 6
}

export const InterrogationOutcomeLabels: Record<InterrogationOutcome, string> = {
    [InterrogationOutcome.FullConfession]: 'اعتراف كامل',
    [InterrogationOutcome.PartialAdmission]: 'إقرار جزئي',
    [InterrogationOutcome.Denial]: 'إنكار التهمة',
    [InterrogationOutcome.ProvidedIntelligence]: 'أدلى بمعلومات استخبارية (تعاون)',
    [InterrogationOutcome.Silent]: 'التزم الصمت',
    [InterrogationOutcome.Contradictory]: 'أقوال متناقضة'
};

export interface SuspectDto {
    id: string;
    firstName: string;
    secondName: string;
    thirdName: string;
    fourthName?: string;
    fivthName?: string;
    fullName: string;
    kunya?: string;
    motherName?: string;
    dateOfBirth?: string;
    placeOfBirth?: string;
    tribe?: string;
    maritalStatus?: number;
    maritalStatusName?: string;
    wivesCount?: number;
    childrenCount?: number;
    legalArticle?: string;
    healthStatus?: string;
    photoUrl?: string;
    currentStatus?: number;
    currentStatusName?: string;
    createdAt: string;
}

export interface InterrogationSession {
    id: string;
    suspectId: string;
    suspect: SuspectDto;
    caseId: string;
    sessionDate: string;
    interrogatorName: string;
    location: string;
    sessionType: InterrogationType;
    sessionTypeName: string;
    content: string;
    summaryContent: string;
    outcome: InterrogationOutcome;
    outcomeName: string;
    investigatorNotes?: string;
    isRatifiedByJudge: boolean;
    createdAt: string;
}

export interface InterrogationSessionSummary {
    id: string;
    suspectId: string;
    suspectFullName: string;
    caseId: string;
    caseTitle: string;
    sessionDate: string;
    interrogatorName: string;
    location: string;
    sessionType: InterrogationType;
    sessionTypeName: string;
    outcome: InterrogationOutcome;
    outcomeName: string;
    isRatifiedByJudge: boolean;
    createdAt: string;
}

export interface InterrogationSessionQuery {
    page?: number;
    pageSize?: number;
    searchTerm?: string;
}

export interface CreateInterrogationSessionRequest {
    suspectId: string;
    caseId: string;
    sessionDate: string;
    interrogatorName: string;
    location: string;
    sessionType: InterrogationType;
    content: string;
    summaryContent: string;
    outcome: InterrogationOutcome;
    investigatorNotes?: string;
    isRatifiedByJudge: boolean;
}

export interface UpdateInterrogationSessionRequest {
    id: string;
    suspectId: string;
    caseId: string;
    sessionDate: string;
    interrogatorName: string;
    location: string;
    sessionType: InterrogationType;
    content: string;
    summaryContent: string;
    outcome: InterrogationOutcome;
    investigatorNotes?: string;
    isRatifiedByJudge: boolean;
}
