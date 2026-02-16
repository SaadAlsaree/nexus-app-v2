
import { InterrogationSessionTable } from './interrogation-sessions-table';
import { columns } from './interrogation-sessions-table/columns';
import { getInterrogationSessions, getInterrogationSessionsBySuspectId } from '../api/interrogation-session.service';
import { InterrogationSessionSummary } from '../types/interrogation-session';

interface InterrogationSessionListingProps {
  suspectId?: string;
  page?: number;
  pageSize?: number;
  searchTerm?: string;
}

export default async function InterrogationSessionListingPage({ 
  suspectId,
  page,
  pageSize,
  searchTerm
}: InterrogationSessionListingProps) {
  let sessions: InterrogationSessionSummary[] = [];
  let totalCount = 0;

  if (suspectId) {
    const response = await getInterrogationSessionsBySuspectId(suspectId);
    // Map InterrogationSession to InterrogationSessionSummary if needed, 
    // but usually suspect-specific view doesn't need to show suspect name again.
    // However, for type consistency with columns:
    sessions = (response?.data || []).map(s => ({
      ...s,
      suspectFullName: '', // Not needed in suspect view but required by type
      caseTitle: '', // Not needed in suspect view but required by type
    })) as InterrogationSessionSummary[];
    totalCount = sessions.length;
  } else {
    const response = await getInterrogationSessions({
      page,
      pageSize,
      searchTerm
    });
    sessions = response?.data?.items || [];
    totalCount = response?.data?.totalCount || 0;
  }

  return (
    <InterrogationSessionTable
      data={sessions}
      totalItems={totalCount}
      columns={columns as any}
    />
  );
}
