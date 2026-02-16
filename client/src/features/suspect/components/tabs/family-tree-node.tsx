'use client';

import { memo } from 'react';
import { Handle, Position, NodeProps } from 'reactflow';
import { Card } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { User, Heart, Baby, Users } from 'lucide-react';

export interface FamilyTreeNodeData {
  fullName: string;
  relationshipName: string;
  isMainSuspect?: boolean;
  currentStatus?: string;
  photoUrl?: string;
}

function FamilyTreeNode({ data }: NodeProps<FamilyTreeNodeData>) {
  const getRelationshipIcon = () => {
    const rel = data.relationshipName?.toLowerCase() || '';
    if (data.isMainSuspect) return <User className="h-4 w-4" />;
    if (rel.includes('أب') || rel.includes('أو')) return <Users className="h-4 w-4" />;
    if (rel.includes('زوج') || rel.includes('نسيب')) return <Heart className="h-4 w-4 text-pink-500" />;
    if (rel.includes('ابن') || rel.includes('بنت') || rel.includes('ابنة')) return <Baby className="h-4 w-4 text-blue-500" />;
    return <User className="h-4 w-4" />;
  };

  const getBorderColor = () => {
    if (data.isMainSuspect) return 'border-blue-600 dark:border-blue-400 bg-blue-50/50 dark:bg-blue-900/20';
    const rel = data.relationshipName || '';
    if (rel.includes('زوج')) return 'border-pink-500 dark:border-pink-400 bg-pink-50/50 dark:bg-pink-900/20';
    if (rel.includes('أب') || rel.includes('أو')) return 'border-amber-500 dark:border-amber-400 bg-amber-50/50 dark:bg-amber-900/20';
    return 'border-zinc-200 dark:border-zinc-800 bg-white dark:bg-zinc-950';
  };

  return (
    <Card className={`min-w-45 max-w-55 border-l-4 px-3 py-3 shadow-sm transition-all hover:shadow-md ${getBorderColor()}`}>
      <Handle
        type="target"
        position={Position.Top}
        className="bg-zinc-400! dark:bg-zinc-600! h-2 w-2"
      />

      <div className="flex items-start gap-3">
        <div className="flex-1 min-w-0">
          <div className="flex items-center gap-2 mb-1">
            <div className="p-1 rounded-full bg-zinc-100 dark:bg-zinc-800">
              {getRelationshipIcon()}
            </div>
            <p className="text-sm font-bold truncate dir-rtl" title={data.fullName}>
              {data.fullName}
            </p>
          </div>
          
          <div className="flex flex-wrap gap-1 mt-2">
            <Badge variant={data.isMainSuspect ? "default" : "secondary"} className="text-[10px] px-1.5 py-0 h-4">
              {data.isMainSuspect ? "الشخص المعني" : data.relationshipName}
            </Badge>
            {data.currentStatus && (
              <Badge variant="outline" className="text-[10px] px-1.5 py-0 h-4 border-zinc-300 dark:border-zinc-700">
                {data.currentStatus}
              </Badge>
            )}
          </div>
        </div>
      </div>

      <Handle
        type="source"
        position={Position.Bottom}
        className="bg-zinc-400! dark:bg-zinc-600! h-2 w-2"
      />
    </Card>
  );
}

export default memo(FamilyTreeNode);
