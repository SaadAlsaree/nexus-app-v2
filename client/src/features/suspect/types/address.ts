export enum AddressType {
    PermanentResidence = 1,
    TemporaryResidence = 2,
    SafeHouse = 3,
    OrgHeadquarters = 4,
    MeetingPoint = 5,
}

export const AddressTypeLabels: Record<AddressType, string> = {
    [AddressType.PermanentResidence]: 'سكن أصلي',
    [AddressType.TemporaryResidence]: 'سكن مؤقت',
    [AddressType.SafeHouse]: 'مضافة',
    [AddressType.OrgHeadquarters]: 'مقر قيادة',
    [AddressType.MeetingPoint]: 'نقطة التقاء',
};

export interface AddressRequest {
    suspectId?: string
    type: AddressType
    city: string
    district: string
    details?: string
    gpsCoordinates?: string
    addressId?: string
}
