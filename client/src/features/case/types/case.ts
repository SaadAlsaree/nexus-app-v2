
export enum CaseStatus {
    UnderInvestigation = 1,
    ReferredToCourt = 2,
    Sentenced = 3,
    ClosedInsufficientEvidence = 4,
    Archived = 5
}

export const CaseStatusLabels: Record<CaseStatus, string> = {
    [CaseStatus.UnderInvestigation]: 'قيد التحقيق',
    [CaseStatus.ReferredToCourt]: 'محالة للمحكمة',
    [CaseStatus.Sentenced]: 'صدر حكم',
    [CaseStatus.ClosedInsufficientEvidence]: 'مغلقة لعدم كفاية الأدلة',
    [CaseStatus.Archived]: 'أرشيف'
};

export enum PriorityLevel {
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

export const PriorityLevelLabels: Record<PriorityLevel, string> = {
    [PriorityLevel.Low]: 'منخفضة',
    [PriorityLevel.Medium]: 'متوسطة',
    [PriorityLevel.High]: 'عالية',
    [PriorityLevel.Critical]: 'خطيرة جداً'
};

export enum LegalStatusInCase {
    DetainedPendingInvestigation = 1,
    ReleasedOnBail = 2,
    Fugitive = 3,
    Sentenced = 4,
    Acquitted = 5,
    ReleasedInsufficientEvidence = 6,
    Witness = 7,
    TransferedToOtherCourt = 8
}

export const LegalStatusInCaseLabels: Record<LegalStatusInCase, string> = {
    [LegalStatusInCase.DetainedPendingInvestigation]: 'موقوف على ذمة التحقيق',
    [LegalStatusInCase.ReleasedOnBail]: 'مُكفَل (مفرج عنه بكفالة)',
    [LegalStatusInCase.Fugitive]: 'هارب من العدالة في هذه القضية',
    [LegalStatusInCase.Sentenced]: 'صدر عليه حكم (محكوم)',
    [LegalStatusInCase.Acquitted]: 'مُبرأ (إفراج)',
    [LegalStatusInCase.ReleasedInsufficientEvidence]: 'مفرج عنه لعدم كفاية الأدلة (غلق الدعوى)',
    [LegalStatusInCase.Witness]: 'تحولت صفته إلى شاهد (مخبر سري / شاهد ملك)',
    [LegalStatusInCase.TransferedToOtherCourt]: 'محال إلى محكمة أخرى/اختصاص آخر'
};

export enum AccusationType {
    Membership = 1,
    DirectParticipation = 2,
    Funding = 3,
    Harboring = 4,
    LogisticalSupport = 5,
    Manufacturing = 6,
    Recruitment = 7,
    Kidnapping = 8,
    PlantingIEDs = 9,
    Assassination = 10,
    CyberTerrorism = 11
}

export const AccusationTypeLabels: Record<AccusationType, string> = {
    [AccusationType.Membership]: 'الانتماء لتنظيم إرهابي',
    [AccusationType.DirectParticipation]: 'المشاركة في عمل إرهابي (تنفيذ)',
    [AccusationType.Funding]: 'تمويل الإرهاب',
    [AccusationType.Harboring]: 'التستر والإيواء (مضايف)',
    [AccusationType.LogisticalSupport]: 'الدعم اللوجستي (نقل مؤن، نقل انتحاريين)',
    [AccusationType.Manufacturing]: 'تصنيع وتطوير أسلحة/متفجرات',
    [AccusationType.Recruitment]: 'التجنيد والتحريض',
    [AccusationType.Kidnapping]: 'الخطف',
    [AccusationType.PlantingIEDs]: 'زرع العبوات',
    [AccusationType.Assassination]: 'الاغتيال',
    [AccusationType.CyberTerrorism]: 'الإرهاب الإلكتروني (إدارة قنوات، نشر إصدارات)'
};

export interface CaseSummary {
    id: string;
    caseFileNumber: string;
    title: string;
    openDate: string;
    status: CaseStatus;
    statusName?: string;
    investigatingOfficer?: string;
    priority: PriorityLevel;
    priorityName?: string;
    suspectsCount: number;
    createdAt: string;
}

export interface CaseSuspect {
    suspectId: string;
    fullName: string;
    accusationType: AccusationType;
    accusationTypeName?: string;
    legalStatus: LegalStatusInCase;
    legalStatusName?: string;
    confessionDate?: string;
    notes?: string;
}

export interface CaseDetails {
    id: string;
    caseFileNumber: string;
    title: string;
    openDate: string;
    status: CaseStatus;
    statusName?: string;
    investigatingOfficer: string;
    priority: PriorityLevel;
    priorityName?: string;
    createdAt: string;
    suspectsCount: number;
    suspects: CaseSuspect[];
}

export interface CreateCaseRequest {
    caseFileNumber: string;
    title: string;
    openDate: string;
    status: CaseStatus;
    investigatingOfficer: string;
    priority: PriorityLevel;
}

export interface UpdateCaseRequest {
    caseId?: string;
    caseFileNumber: string;
    title: string;
    openDate: string;
    status: CaseStatus;
    investigatingOfficer: string;
    priority: PriorityLevel;
}

export interface AddSuspectToCaseRequest {
    caseId: string;
    suspectId: string;
    accusationType: AccusationType;
    legalStatus: LegalStatusInCase;
    confessionDate?: string;
    notes?: string;
}

export interface RemoveSuspectFromCaseRequest {
    caseId: string;
    suspectId: string;
}

export interface CaseQuery {
    page?: number;
    pageSize?: number;
    searchTerm?: string;
}

