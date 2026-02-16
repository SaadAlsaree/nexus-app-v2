export enum RelationshipType {
    Father = 1,
    Mother = 2,
    Brother = 3,
    Sister = 4,
    Uncle = 5,
    Cousin = 6,
    InLaw = 7,
    Friend = 8,
    Associate = 9,
    Son = 10,
    Daughter = 11,
    Spouse = 12,
    MaternalUncle = 13,
    MaternalCousin = 14,
    Aunt = 15,
    MaternalCousinFemale = 16
}

export const RelationshipTypeLabels: Record<RelationshipType, string> = {
    [RelationshipType.Father]: 'أب',
    [RelationshipType.Mother]: 'أم',
    [RelationshipType.Brother]: 'أخ',
    [RelationshipType.Sister]: 'أخت',
    [RelationshipType.Uncle]: 'عم',
    [RelationshipType.Cousin]: 'ابن عم',
    [RelationshipType.InLaw]: 'نسيب',
    [RelationshipType.Friend]: 'صديق',
    [RelationshipType.Associate]: 'شريك/زميل',
    [RelationshipType.Son]: 'ابن',
    [RelationshipType.Daughter]: 'ابنة',
    [RelationshipType.Spouse]: 'زوج / زوجة',
    [RelationshipType.MaternalUncle]: 'خال',
    [RelationshipType.MaternalCousin]: 'ابن خال',
    [RelationshipType.Aunt]: 'عمة',
    [RelationshipType.MaternalCousinFemale]: 'ابنة عمة',
};

export interface RelationshipRequest {
  suspectId: string,
  firstName: string,
  secondName?: string,
  thirdName?: string,
  fourthName?: string,
  fivthName?: string,
  tribe?: string,
  relationship: RelationshipType,
  notes?: string,
  relatedPersonId?: string
}
