// types/alert.ts

export enum AlertLevel {
  Low = 1,
  Medium = 2,
  High = 3,
  RedAlert = 4
}

export interface SystemAlert {
  id: string;
  title: string;
  message: string;
  level: AlertLevel;
  source: string;
  relatedEntityId?: string;
  createdAt: string;
  isRead: boolean;
}