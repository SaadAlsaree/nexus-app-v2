'use client';

import { memo } from 'react';
import { Handle, Position, NodeProps } from 'reactflow';
import { Card } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { OrgUnitLevelLabels, OrgUnitLevel } from '../types/organization';

interface OrgUnitNodeData {
  unitName: string;
  level: OrgUnitLevel;
  levelName: string;
  path: string;
  subUnitsCount: number;
}

function OrgUnitFlowNode({ data }: NodeProps<OrgUnitNodeData>) {
  const getLevelColor = (level: OrgUnitLevel) => {
    const colors: Record<OrgUnitLevel, string> = {
      [OrgUnitLevel.GeneralCommand]: 'border-blue-500 bg-blue-50',
      [OrgUnitLevel.Wilayah]: 'border-green-500 bg-green-50',
      [OrgUnitLevel.Sector]: 'border-yellow-500 bg-yellow-50',
      [OrgUnitLevel.Battalion]: 'border-orange-500 bg-orange-50',
      [OrgUnitLevel.Detachment]: 'border-red-500 bg-red-50',
      [OrgUnitLevel.Cell]: 'border-purple-500 bg-purple-50',
    };
    return colors[level] || 'border-gray-500 bg-gray-50';
  };

  return (
    <Card className={`max-w-[240px] min-w-[200px] border-l-4 px-3 py-2.5 shadow-md transition-all hover:shadow-lg ${getLevelColor(data.level)}`}>
      <Handle
        type='target'
        position={Position.Top}
        className='h-3 w-3 !bg-gray-400'
      />

      <div className='mb-2 truncate text-sm font-semibold' title={data.unitName}>
        {data.unitName}
      </div>

      <div className='mb-2 flex flex-wrap items-center gap-1.5'>
        <Badge variant='outline' className='text-xs'>
          {OrgUnitLevelLabels[data.level]}
        </Badge>
        {data.subUnitsCount > 0 && (
          <Badge variant='secondary' className='text-xs'>
            {data.subUnitsCount} وحدة
          </Badge>
        )}
      </div>

      <div className='text-xs text-muted-foreground truncate' title={data.path}>
        {data.path}
      </div>

      <Handle
        type='source'
        position={Position.Bottom}
        className='h-3 w-3 !bg-gray-400'
      />
    </Card>
  );
}

export default memo(OrgUnitFlowNode);
