import { OrgUnitLevelLabels } from '../../types/organization';

export const LEVEL_OPTIONS = Object.entries(OrgUnitLevelLabels).map(([value, label]) => ({
  value: value,
  label: label
}));
