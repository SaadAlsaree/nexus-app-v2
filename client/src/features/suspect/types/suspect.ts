export enum SuspectStatus {
    Unknown = 0,
    Detained = 1,
    Fugitive = 2,
    Killed = 3,
    Sentenced = 4,
    Released = 5,
}

export const SuspectStatusLabels: Record<SuspectStatus, string> = {
    [SuspectStatus.Unknown]: 'غير معروف',
    [SuspectStatus.Detained]: 'موقوف',
    [SuspectStatus.Fugitive]: 'هارب',
    [SuspectStatus.Killed]: 'مقتول',
    [SuspectStatus.Sentenced]: 'محكوم',
    [SuspectStatus.Released]: 'مُفرج عنه',
};

export enum MaritalStatus {
    Single = 1,
    Married = 2,
    Divorced = 3,
    Widower = 4,
}

export const MaritalStatusLabels: Record<MaritalStatus, string> = {
    [MaritalStatus.Single]: 'أعزب',
    [MaritalStatus.Married]: 'متزوج',
    [MaritalStatus.Divorced]: 'مطلق',
    [MaritalStatus.Widower]: 'أرمل',
};

export interface UpdateSuspectRequest {
    suspectId?: string;
    firstName?: string;
    secondName?: string;
    thirdName?: string;
    fourthName?: string;
    fivthName?: string;
    kunya?: string;
    codeNum?: string;
    motherName?: string;
    dateOfBirth?: string;
    placeOfBirth?: string;
    tribe?: string;
    maritalStatus?: MaritalStatus;
    wivesCount?: number;
    childrenCount?: number;
    legalArticle?: string;
    healthStatus?: string;
    photoUrl?: string;
    status?: SuspectStatus;
}

export interface UpdateSuspectStatusRequest {
    suspectId: string;
    newStatus: SuspectStatus;
    notes: any;
}

export interface Suspect {
    addresses: Address[];
    bayahDetails: BayahDetail[];
    caseInvolvements: CaseInvolvement[];
    childrenCount: number;
    contacts: Contact[];
    createdAt: string;
    currentStatus: SuspectStatus;
    currentStatusName: string;
    dateOfBirth?: string;
    familyName: string;
    firstName: string;
    fivthName?: string;
    fourthName?: string;
    fullName: string;
    healthStatus?: any;
    id: string;
    kunya: string;
    legalArticle: string;
    lifeHistories: LifeHistory[];
    maritalStatus?: any;
    maritalStatusName?: any;
    motherName: string;
    operations: Operation[];
    organizationalAssignments: OrganizationalAssignment[];
    photoUrl: string | null;
    placeOfBirth: string;
    relativesAndAssociates: RelativesAndAssociate[];
    secondName: string;
    thirdName: string;
    trainingCourses: TrainingCourse[];
    tribe?: string;
    wivesCount: number;
}


export interface CreateSuspectRequest {
   
  firstName?: string;
  secondName?: string;
  thirdName?: string;
  fourthName?: string;
  fivthName?: string;
  codeNum?: string;
  kunya?: string;
  motherName?: string;
  dateOfBirth?: string;
  placeOfBirth?: string;
  tribe?: string;
  status?: SuspectStatus;
  legalArticle?: string;

}



export interface SuspectQuery {
    page?: number;
    pageSize?: number;
    searchTerm?: string;
}

export interface NetworkSuspect {
    nodes: NetworkSuspectNode[]
    links: NetworkSuspectLink[]
}

export interface NetworkSuspectNode {
    id: string
    label: string
    type: string
    group: string
}

export interface NetworkSuspectLink {
    source: string
    target: string
    label: string
    type: string
}


// Suspect Details

export interface SuspectDetails {
    id: string
    firstName: string
    secondName: string
    thirdName: string
    fourthName: any
    fivthName: string
    familyName: any
    fullName: string
    codeNum: string
    kunya: any
    motherName: any
    dateOfBirth: any
    placeOfBirth: any
    tribe: any
    maritalStatus: any
    maritalStatusName: any
    wivesCount: number
    childrenCount: number
    legalArticle: any
    healthStatus: any
    photoUrl: any
    currentStatus: number
    currentStatusName: any
    createdAt: string
    addresses: Address[]
    contacts: Contact[]
    relativesAndAssociates: RelativesAndAssociate[]
    lifeHistories: LifeHistory[]
    bayahDetails: BayahDetail[]
    trainingCourses: TrainingCourse[]
    operations: Operation[]
    organizationalAssignments: OrganizationalAssignment[]
    caseInvolvements: CaseInvolvement[]
}

export interface Address {
    id: string
    type: number
    typeName: string
    city: string
    district: string
    details: any
    gpsCoordinates: any
}

export interface Contact {
    id: string
    type: number
    typeName: string
    value: string
    ownerName: any
}

export interface RelativesAndAssociate {
    id: string
    fullName: string
    relationship: number
    relationshipName: string
    spouseName: any
    currentStatus: any
    notes: any
}

export interface LifeHistory {
    id: string
    educationLevel: any
    schoolsAttended: any
    civilianJobs: any
    radicalizationStory: any
}

export interface BayahDetail {
    id: string
    date: any
    location: any
    recruiterName: any
    textOfPledge: any
}

export interface TrainingCourse {
    id: string
    courseType: number
    courseTypeName: string
    location: any
    trainerName: any
    classmates: any
}

export interface Operation {
    id: string
    operationType: number
    operationTypeName: string
    date: any
    location: any
    roleInOp: any
    associates: any
}

export interface OrganizationalAssignment {
    id: string
    orgUnitId: string
    orgUnitName: string
    directCommanderId: any
    directCommanderName: any
    roleTitle: number
    roleTitleName: string
    startDate: string
    endDate: any
}

export interface CaseInvolvement {
    id: string
    caseId: string
    caseFileNumber: string
    caseTitle: string
    accusationType: number
    accusationTypeName: string
    legalStatus: number
    legalStatusName: string
    confessionDate: any
    notes: any
}

export interface LifeHistoryRequest {
    suspectId?: string
    educationLevel?: string
    schoolsAttended?: string
    civilianJobs?: string
    radicalizationStory?: string
    lifeHistoryId?: string
}



