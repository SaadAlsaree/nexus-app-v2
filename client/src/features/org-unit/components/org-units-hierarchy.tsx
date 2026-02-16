'use client';

import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { ChevronDown, ChevronRight, Plus, Edit2Icon, Trash2Icon } from 'lucide-react';
import { OrgUnitHierarchyNode, OrgUnitLevelLabels } from '../types/organization';
import { deleteOrganizationalUnit } from '../api/organization.service';
import { toast } from 'sonner';
import OrganizationForm from './forms/organization-form';
import DeleteWarning from '@/components/delete-worning';
import { Separator } from '@/components/ui/separator';

interface OrgUnitsHierarchyProps {
  hierarchy?: OrgUnitHierarchyNode[];
  onRefresh?: () => void;
}

export default function OrgUnitsHierarchy({ hierarchy, onRefresh }: OrgUnitsHierarchyProps) {
  const [selectedUnit, setSelectedUnit] = useState<OrgUnitHierarchyNode | null>(null);
  const [formOpen, setFormOpen] = useState(false);
  const [deleteWarningOpen, setDeleteWarningOpen] = useState(false);

  const handleEdit = (unit: OrgUnitHierarchyNode) => {
    setSelectedUnit(unit);
    setFormOpen(true);
  };

  const handleDelete = (unit: OrgUnitHierarchyNode) => {
    setSelectedUnit(unit);
    setDeleteWarningOpen(true);
  };

  const onDeleteConfirm = async () => {
    try {
      if (!selectedUnit) return;
      const res = await deleteOrganizationalUnit(selectedUnit.id);
      if (res.succeeded) {
        toast.success('تم حذف الوحدة التنظيمية بنجاح');
        setDeleteWarningOpen(false);
        setSelectedUnit(null);
        onRefresh?.();
      }
    } catch (error) {
      console.error('Error deleting organizational unit:', error);
      toast.error(`فشل في حذف الوحدة التنظيمية: ${error}`);
    }
  };

  const handleFormClose = (open: boolean) => {
    setFormOpen(open);
    if (!open) {
      setSelectedUnit(null);
      onRefresh?.();
    }
  };

  return (
    <>
      <DeleteWarning
        onDelete={onDeleteConfirm}
        open={deleteWarningOpen}
        onOpenChange={setDeleteWarningOpen}
        title="حذف الوحدة التنظيمية"
        description="هل أنت متأكد أنك تريد حذف هذه الوحدة التنظيمية؟ هذا الإجراء لا يمكن التراجع عنه."
      />
      <OrganizationForm
        id={selectedUnit?.id || ''}
        initialData={selectedUnit as any}
        open={formOpen}
        onOpenChange={handleFormClose}
        title={selectedUnit ? 'تعديل الوحدة التنظيمية' : 'إضافة وحدة تنظيمية جديدة'}
      />

      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <CardTitle>الهيكل التنظيمي</CardTitle>
            <Button onClick={() => setFormOpen(true)}>
              <Plus className="ml-2 h-4 w-4" />
              إضافة وحدة جديدة
            </Button>
          </div>
        </CardHeader>
        <CardContent>
          {hierarchy && hierarchy.length > 0 ? (
            <div className="space-y-2 ">
              {hierarchy.map((node) => (
                <>
                <TreeNode
                  key={node.id}
                  node={node}
                  onEdit={handleEdit}
                  onDelete={handleDelete}
                  level={0}
                />
                <Separator className="my-2" />
                </>
              ))}
            </div>
          ) : (
            <p className="text-muted-foreground text-center py-8">لا توجد وحدات تنظيمية</p>
          )}
        </CardContent>
      </Card>
    </>
  );
}

interface TreeNodeProps {
  node: OrgUnitHierarchyNode;
  onEdit: (unit: OrgUnitHierarchyNode) => void;
  onDelete: (unit: OrgUnitHierarchyNode) => void;
  level: number;
}

function TreeNode({ node, onEdit, onDelete, level }: TreeNodeProps) {
  const [isExpanded, setIsExpanded] = useState(true);
  const hasChildren = node.subUnits && node.subUnits.length > 0;

  const getLevelColor = (level: number) => {
    const colors = [
      'border-blue-500',
      'border-green-500',
      'border-yellow-500',
      'border-orange-500',
      'border-red-500',
      'border-purple-500',
    ];
    return colors[level % colors.length];
  };

  return (
    <div className="space-y-2">
      <div
        className={`rounded-lg border-l-4 ${getLevelColor(level)} bg-card p-4 hover:bg-accent/50 transition-colors`}
        style={{ marginRight: `${level * 24}px` }}
      >
        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-3 flex-1">
            {hasChildren && (
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setIsExpanded(!isExpanded)}
                className="h-6 w-6 p-0"
              >
                {isExpanded ? (
                  <ChevronDown className="h-4 w-4" />
                ) : (
                  <ChevronRight className="h-4 w-4" />
                )}
              </Button>
            )}
            {!hasChildren && <div className="w-6" />}
            
            <div className="flex-1">
              <div className="flex items-center gap-2 mb-1">
                <h4 className="font-semibold text-lg">{node.unitName}</h4>
                <Badge variant="outline">{OrgUnitLevelLabels[node.level]}</Badge>
                {node.subUnits.length > 0 && (
                  <Badge variant="secondary">{node.subUnits.length} وحدة فرعية</Badge>
                )}
              </div>
              <p className="text-muted-foreground text-sm">{node.path}</p>
            </div>
          </div>

          <div className="flex gap-2">
            <Button
              variant="outline"
              size="sm"
              onClick={() => onEdit(node)}
            >
              <Edit2Icon className="h-4 w-4" />
            </Button>
            <Button
              variant="outline"
              size="sm"
              onClick={() => onDelete(node)}
            >
              <Trash2Icon className="h-4 w-4 text-red-500" />
            </Button>
          </div>
        </div>
      </div>

      {hasChildren && isExpanded && (
        <div className="space-y-2">
          {node.subUnits.map((child) => (
            <TreeNode
              key={child.id}
              node={child}
              onEdit={onEdit}
              onDelete={onDelete}
              level={level + 1}
            />
          ))}
        </div>
      )}
    </div>
  );
}
