export enum OrgUnitLevel {
    GeneralCommand = 1,
    Wilayah = 2,
    Sector = 3,
    Battalion = 4,
    Detachment = 5,
    Cell = 6,
}

export const OrgUnitLevelLabels: Record<OrgUnitLevel, string> = {
    [OrgUnitLevel.GeneralCommand]: 'القيادة العامة',
    [OrgUnitLevel.Wilayah]: 'ولاية',
    [OrgUnitLevel.Sector]: 'قاطع',
    [OrgUnitLevel.Battalion]: 'كتيبة',
    [OrgUnitLevel.Detachment]: 'مفرزة',
    [OrgUnitLevel.Cell]: 'خلية صغيرة',
};

export interface OrganizationalUnitRequest {
    unitName: string
    level: OrgUnitLevel
    parentUnitId?: any
}

export interface OrganizationalUnitDetail {
    id: string
    unitName: string
    level: OrgUnitLevel
    path: string
    subUnitsCount: number
    createdAt: string
    parentUnitId?: string | null
    parentUnitName?: string | null
}

export interface OrgUnitHierarchyNode {
    id: string
    unitName: string
    level: OrgUnitLevel
    path: string
    subUnits: OrgUnitHierarchyNode[]
}

export interface OrgUnitListItem {
    id: string
    unitName: string
    level: OrgUnitLevel
    levelName: string
    path: string
    subUnitsCount: number
    createdAt: string
}