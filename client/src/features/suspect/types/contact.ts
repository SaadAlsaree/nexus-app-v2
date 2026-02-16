export enum ContactType {
    MobilePhone = 1,
    Landline = 2,
    Telegram = 3,
    WhatsApp = 4,
    Facebook = 5,
    Email = 6,
    RadioDevice = 7,
}

export const ContactTypeLabels: Record<ContactType, string> = {
    [ContactType.MobilePhone]: 'هاتف محمول',
    [ContactType.Landline]: 'هاتف أرضي',
    [ContactType.Telegram]: 'تيليجرام',
    [ContactType.WhatsApp]: 'واتساب',
    [ContactType.Facebook]: 'فيسبوك',
    [ContactType.Email]: 'بريد إلكتروني',
    [ContactType.RadioDevice]: 'جهاز لاسلكي',
};

export interface ContactRequest {
    suspectId?: string
    type: ContactType
    value: string
    ownerName?: string
    contactId?: string
}
