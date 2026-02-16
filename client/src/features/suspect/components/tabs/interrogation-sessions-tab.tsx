
'use client';

import { useEffect, useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { FileText, Plus, Calendar, User, MapPin } from 'lucide-react';
import { SuspectDetails } from '../../types/suspect';
import { getInterrogationSessionsBySuspectId } from '@/features/interrogation-session/api/interrogation-session.service';
import { InterrogationSession, InterrogationTypeLabels, InterrogationOutcomeLabels } from '@/features/interrogation-session/types/interrogation-session';
import { Skeleton } from '@/components/ui/skeleton';
import Link from 'next/link';

interface InterrogationSessionsTabProps {
  suspect: SuspectDetails;
  formatDate: (date: string | null) => string;
}

export function InterrogationSessionsTab({ suspect, formatDate }: InterrogationSessionsTabProps) {
  const [sessions, setSessions] = useState<InterrogationSession[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    async function fetchSessions() {
      try {
        setIsLoading(true);
        const response = await getInterrogationSessionsBySuspectId(suspect.id);
        if (response.succeeded && response.data) {
          setSessions(response.data);
        }
      } catch (error) {
        console.error('Error fetching interrogation sessions:', error);
      } finally {
        setIsLoading(false);
      }
    }

    fetchSessions();
  }, [suspect.id]);

  if (isLoading) {
    return (
      <div className="space-y-4">
        {[1, 2, 3].map((i) => (
          <Card key={i}>
            <CardContent className="p-6">
              <Skeleton className="h-6 w-1/3 mb-2" />
              <Skeleton className="h-4 w-1/4 mb-4" />
              <div className="space-y-2">
                <Skeleton className="h-4 w-full" />
                <Skeleton className="h-4 w-full" />
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <Card>
        <CardHeader className="flex flex-row items-center justify-between pb-2">
          <CardTitle className="text-xl font-bold flex items-center gap-2">
            <FileText className="h-5 w-5" />
            جلسات الاستجواب
          </CardTitle>
          <Link href={`/interrogation-session/new?suspectId=${suspect.id}`}>
            <Button size="sm">
              <Plus className="h-4 w-4 ml-1" />
              إضافة جلسة
            </Button>
          </Link>
        </CardHeader>
        <CardContent>
          {sessions.length > 0 ? (
            <div className="grid gap-4">
              {sessions.map((session) => (
                <Link key={session.id} href={`/interrogation-session/${session.id}`}>
                  <Card className="hover:bg-accent/50 transition-colors cursor-pointer">
                    <CardContent className="p-4">
                      <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                        <div className="space-y-1">
                          <div className="flex items-center gap-2">
                            <h4 className="font-semibold text-lg">
                              {session.sessionTypeName || InterrogationTypeLabels[session.sessionType]}
                            </h4>
                            <Badge variant={session.outcome === 1 ? 'default' : session.outcome === 3 ? 'destructive' : 'secondary'}>
                              {session.outcomeName || InterrogationOutcomeLabels[session.outcome]}
                            </Badge>
                          </div>
                          <div className="flex flex-wrap items-center gap-x-4 gap-y-1 text-sm text-muted-foreground">
                            <span className="flex items-center gap-1">
                              <Calendar className="h-3.5 w-3.5" />
                              {formatDate(session.sessionDate)}
                            </span>
                            <span className="flex items-center gap-1">
                              <User className="h-3.5 w-3.5" />
                              المحقق: {session.interrogatorName}
                            </span>
                            <span className="flex items-center gap-1">
                              <MapPin className="h-3.5 w-3.5" />
                              {session.location}
                            </span>
                          </div>
                        </div>
                        {session.isRatifiedByJudge && (
                          <Badge variant="outline" className="w-fit">مصدق قضائياً</Badge>
                        )}
                      </div>
                      <div className="mt-3 text-sm line-clamp-2 text-muted-foreground">
                        {session.summaryContent}
                      </div>
                    </CardContent>
                  </Card>
                </Link>
              ))}
            </div>
          ) : (
            <div className="text-center py-12">
              <FileText className="h-12 w-12 mx-auto text-muted-foreground opacity-20 mb-4" />
              <p className="text-muted-foreground">لا توجد جلسات استجواب مسجلة لهذا المتهم</p>
              <Link href={`/interrogation-session/new?suspectId=${suspect.id}`} className="mt-4 inline-block">
                <Button variant="outline" size="sm">إضافة أول جلسة</Button>
              </Link>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
