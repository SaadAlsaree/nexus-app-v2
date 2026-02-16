export enum OrgRole {
    Wali = 1,
    Emir = 2,
    Soldier = 3,
    Transporter = 4,
    Administrator = 5,
    Recruiter = 6,
    SuicideBomber = 7
}

export const OrgRoleLabels: Record<OrgRole, string> = {
    [OrgRole.Wali]: 'والي',
    [OrgRole.Emir]: 'أمير',
    [OrgRole.Soldier]: 'جندي',
    [OrgRole.Transporter]: 'ناقل',
    [OrgRole.Administrator]: 'إداري',
    [OrgRole.Recruiter]: 'شرعي/منسّب',
    [OrgRole.SuicideBomber]: 'انغماسي/استشهادي',
};

export interface OrgAssignmentRequest {
    suspectId: string
    orgUnitId: string
    directCommanderId?: any
    roleTitle: OrgRole
    startDate: string
    endDate?: string
}
