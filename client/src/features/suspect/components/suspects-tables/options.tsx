import { SuspectStatusLabels, SuspectStatus } from '../../types/suspect';

export const STATUS_OPTIONS = Object.entries(SuspectStatusLabels).map(([value, label]) => ({
  value: value,
  label: label
}));
