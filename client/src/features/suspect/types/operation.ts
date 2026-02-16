export enum OperationType {
    Offensive = 1,
    Defensive = 2,
    IED_Planting = 3,
    VBIED_Attack = 4,
    Assassination = 5,
    SniperAttack = 6,
    SuicideMission = 7,
    Reconnaissance = 8,
    LogisticalSupport = 9,
    Kidnapping = 10,
    Execution = 11,
    MortarShelling = 12,
}

export const OperationTypeLabels: Record<OperationType, string> = {
    [OperationType.Offensive]: 'هجوم / صولة',
    [OperationType.Defensive]: 'صد تعرض / رباط',
    [OperationType.IED_Planting]: 'زرع عبوات ناسفة / تفخيخ طرق',
    [OperationType.VBIED_Attack]: 'هجوم بعجلة مفخخة',
    [OperationType.Assassination]: 'اغتيال (كواتم / عبوات لاصقة)',
    [OperationType.SniperAttack]: 'قنص',
    [OperationType.SuicideMission]: 'عملية انغماسية / استشهادية',
    [OperationType.Reconnaissance]: 'رصد / استطلاع',
    [OperationType.LogisticalSupport]: 'دعم لوجستي / نقل مؤن وسلاح',
    [OperationType.Kidnapping]: 'خطف',
    [OperationType.Execution]: 'تصفية / إعدام',
    [OperationType.MortarShelling]: 'قصف هاون / صواريخ',
};

export interface OperationRequest {
    suspectId?: string
    operationType: OperationType
    date?: any
    location?: any
    roleInOp?: any
    associates?: any
    operationId?: string
}