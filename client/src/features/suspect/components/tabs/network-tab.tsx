import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import NetworkPage from '../network/network-graph';

interface NetworkTabProps {
  suspectId: string;
  theme: 'light' | 'dark';
}

export function NetworkTab({ suspectId, theme }: NetworkTabProps) {
  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <CardTitle>شبكة العلاقات</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4 md:grid-cols-1 lg:grid-cols-1">
            <NetworkPage id={suspectId} theme={theme} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
