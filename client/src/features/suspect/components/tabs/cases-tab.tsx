import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { FileText } from 'lucide-react';
import { SuspectDetails } from '../../types/suspect';
import { InfoItem } from '../info-item';

interface CasesTabProps {
  suspect: SuspectDetails;
  formatDate: (date: string | null) => string;
}

export function CasesTab({ suspect, formatDate }: CasesTabProps) {
  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <FileText className="h-5 w-5" />
            القضايا المرتبطة
          </CardTitle>
        </CardHeader>
        <CardContent>
          {suspect.caseInvolvements && suspect.caseInvolvements.length > 0 ? (
            <div className="space-y-4">
              {suspect.caseInvolvements.map((caseInvolvement) => (
                <div key={caseInvolvement.id} className="rounded-lg border p-4">
                  <div className="mb-3 flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                    <div>
                      <h4 className="font-semibold">{caseInvolvement.caseTitle}</h4>
                      <p className="text-muted-foreground text-sm">
                        رقم القضية: {caseInvolvement.caseFileNumber}
                      </p>
                    </div>
                    <div className="flex gap-2">
                      <Badge variant="outline">{caseInvolvement.accusationTypeName}</Badge>
                      <Badge variant="secondary">{caseInvolvement.legalStatusName}</Badge>
                    </div>
                  </div>
                  <div className="grid gap-2 md:grid-cols-2">
                    {caseInvolvement.confessionDate && (
                      <InfoItem
                        label="تاريخ الاعتراف"
                        value={formatDate(caseInvolvement.confessionDate)}
                      />
                    )}
                    {caseInvolvement.notes && (
                      <InfoItem label="ملاحظات" value={caseInvolvement.notes} />
                    )}
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-muted-foreground text-center py-8">لا توجد قضايا مرتبطة</p>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
