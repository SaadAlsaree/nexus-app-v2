import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Users, Edit2Icon, Trash2Icon, LayoutList, Network } from 'lucide-react';
import { SuspectDetails, RelativesAndAssociate } from '../../types/suspect';
import { InfoItem } from '../info-item';
import { useState } from 'react';
import { FamilyTreeFlow } from './family-tree-flow';
import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs';

interface RelationsTabProps {
  suspect: SuspectDetails;
  onAddRelationship: () => void;
  onEditRelationship: (relationship: RelativesAndAssociate) => void;
  onDeleteRelationship: (relationship: RelativesAndAssociate) => void;
}

export function RelationsTab({
  suspect,
  onAddRelationship,
  onEditRelationship,
  onDeleteRelationship
}: RelationsTabProps) {
  const [viewMode, setViewMode] = useState<'list' | 'tree'>('list');

  return (
    <div className="space-y-4">
      <div className="flex flex-col sm:flex-row items-center justify-between gap-4 mb-2">
        <Tabs value={viewMode} onValueChange={(v) => setViewMode(v as 'list' | 'tree')} className="w-full sm:w-auto">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="list" className="flex items-center gap-2">
              <LayoutList className="h-4 w-4" />
              <span>عرض القائمة</span>
            </TabsTrigger>
            <TabsTrigger value="tree" className="flex items-center gap-2">
              <Network className="h-4 w-4" />
              <span>شجرة العائلة</span>
            </TabsTrigger>
          </TabsList>
        </Tabs>

        <Button onClick={onAddRelationship} className="w-full sm:w-auto">
          ادراج جديد
        </Button>
      </div>

      <Card className="border-none shadow-none bg-transparent">
        <CardContent className="p-0">
          {viewMode === 'list' ? (
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <Users className="h-5 w-5" />
                  الأقارب والمعارف
                </CardTitle>
              </CardHeader>
              <CardContent>
                {suspect.relativesAndAssociates && suspect.relativesAndAssociates.length > 0 ? (
                  <div className="space-y-3">
                    {suspect.relativesAndAssociates.map((relative) => (
                      <div key={relative.id} className="rounded-lg border p-4 bg-zinc-50 dark:bg-zinc-900/40 hover:bg-zinc-100 dark:hover:bg-zinc-900/60 transition-colors">
                        <div className='flex justify-end gap-1 mb-2'>
                          <Button variant="ghost" size="sm" onClick={() => onEditRelationship(relative)}>
                            <Edit2Icon className="h-4 w-4" />
                          </Button>
                          <Button variant="ghost" size="sm" onClick={() => onDeleteRelationship(relative)}>
                            <Trash2Icon className="h-4 w-4 text-red-500" />
                          </Button>
                        </div>
                        <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                          <div>
                            <h4 className="font-semibold text-base">{relative.fullName}</h4>
                            <Badge variant="outline" className="mt-1">
                              {relative.relationshipName}
                            </Badge>
                          </div>
                          {relative.currentStatus && (
                            <Badge variant="secondary" className="bg-zinc-200 dark:bg-zinc-800">{relative.currentStatus}</Badge>
                          )}
                        </div>
                        {relative.spouseName && (
                          <InfoItem label="اسم الزوج/الزوجة" value={relative.spouseName} className="mt-2" />
                        )}
                        {relative.notes && (
                          <InfoItem label="ملاحظات" value={relative.notes} className="mt-2 text-muted-foreground" />
                        )}
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="text-center py-12 border-2 border-dashed rounded-xl">
                    <p className="text-muted-foreground">لا توجد علاقات مسجلة</p>
                  </div>
                )}
              </CardContent>
            </Card>
          ) : (
            <div className="space-y-4">
              <Card>
                <CardHeader className="pb-2">
                  <CardTitle className="text-lg flex items-center gap-2">
                    <Network className="h-5 w-5" />
                    الشجرة الهرمية
                  </CardTitle>
                </CardHeader>
                <CardContent>
                    <FamilyTreeFlow suspect={suspect} />
                </CardContent>
              </Card>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
