export interface Evidence {
    id: string;
    fileName: string;
    fileType: string;
    hashChecksum: string;
    description: string | null;
    createdAt: string;
    downloadUrl: string;
}

export interface CreateEvidenceRequest {
    file: File;
    caseId?: string;
    suspectId?: string;
    description?: string;
}

export interface UpdateEvidenceRequest {
    id: string;
    description?: string;
    fileName?: string;
}

export interface EvidenceQuery {
    page?: number;
    pageSize?: number;
}
