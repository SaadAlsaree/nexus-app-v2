
'use client';
import { Badge } from '@/components/ui/badge';
import { DataTableColumnHeader } from '@/components/ui/table/data-table-column-header';
import { InterrogationSessionSummary, InterrogationTypeLabels, InterrogationOutcomeLabels } from '../../types/interrogation-session';
import { Column, ColumnDef } from '@tanstack/react-table';
import { FileText, Calendar, User, Briefcase } from 'lucide-react';
import { CellAction } from './cell-action';
import Link from 'next/link';

export const columns: ColumnDef<InterrogationSessionSummary>[] = [
  {
    accessorKey: 'suspectFullName',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='المتهم' />
    ),
    cell: ({ row }) => (
      <Link href={`/suspect/${row.original.suspectId}`} className="flex items-center gap-2 hover:underline text-blue-600">
        <User className="h-4 w-4" />
        {row.original.suspectFullName}
      </Link>
    ),
  },
  {
    accessorKey: 'caseTitle',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='القضية' />
    ),
    cell: ({ row }) => (
      <Link href={`/cases/${row.original.caseId}`} className="flex items-center gap-2 hover:underline text-blue-600">
        <Briefcase className="h-4 w-4" />
        {row.original.caseTitle}
      </Link>
    ),
  },
  {
    accessorKey: 'sessionDate',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='تاريخ الجلسة' />
    ),
    cell: ({ row }) => {
      const date = row.original.sessionDate;
      return (
        <div className='flex items-center gap-1'>
          <Calendar className='h-4 w-4 mr-2' />
          <span>{new Date(date).toLocaleDateString('ar-EG')}</span>
        </div>
      );
    },
    meta: {
      label: 'تاريخ الجلسة',
      placeholder: 'بحث بالتاريخ...',
      variant: 'text',
      icon: Calendar
    },
  },
  {
    accessorKey: 'sessionTypeName',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='نوع الجلسة' />
    ),
    cell: ({ row }) => {
      const sessionType = row.original.sessionType;
      return (
        <Badge variant='outline'>
          {InterrogationTypeLabels[sessionType]}
        </Badge>
      );
    }
  },
  {
    accessorKey: 'interrogatorName',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='المحقق' />
    ),
    meta: {
      label: 'المحقق',
      placeholder: 'بحث باسم المحقق...',
      variant: 'text',
      icon: FileText
    },
  },
  {
    accessorKey: 'location',
    header: 'الموقع',
  },
  {
    accessorKey: 'outcomeName',
    header: ({ column }: { column: Column<InterrogationSessionSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='النتيجة' />
    ),
    cell: ({ row }) => {
      const outcome = row.original.outcome;
      return (
        <Badge variant={outcome === 1 ? 'default' : outcome === 3 ? 'destructive' : 'secondary'}>
          {InterrogationOutcomeLabels[outcome]}
        </Badge>
      );
    }
  },
  {
    accessorKey: 'isRatifiedByJudge',
    header: 'مصدق قضائياً',
    cell: ({ row }) => (
      <Badge variant={row.original.isRatifiedByJudge ? 'default' : 'outline'}>
        {row.original.isRatifiedByJudge ? 'نعم' : 'لا'}
      </Badge>
    )
  },
  {
    id: 'actions',
    cell: ({ row }) => <CellAction data={row.original} />
  }
];
