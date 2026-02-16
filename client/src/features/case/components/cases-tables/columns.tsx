
'use client';
import { Badge } from '@/components/ui/badge';
import { DataTableColumnHeader } from '@/components/ui/table/data-table-column-header';
import { CaseSummary, CaseStatusLabels, PriorityLevelLabels } from '../../types/case';
import { Column, ColumnDef } from '@tanstack/react-table';
import { FileText, BriefcaseBusiness } from 'lucide-react';
import { CellAction } from './cell-action';
import Link from 'next/link';

export const columns: ColumnDef<CaseSummary>[] = [
  {
    accessorKey: 'caseFileNumber',
    header: ({ column }: { column: Column<CaseSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='رقم القضية' />
    ),
    cell: ({ row }) => {
      const id = row.original.id;
      const fileNumber = row.original.caseFileNumber;
      return (
        <Link href={`/cases/${id}`} className='flex items-center gap-1'>
          <BriefcaseBusiness className='h-4 w-4 mr-2' />
          <span className='text-blue-500 hover:text-blue-700 font-medium underline'>
            {fileNumber}
          </span>
        </Link>
      );
    },
    enableColumnFilter: true,
     meta: {
      label: 'رقم القضية',
      placeholder: 'بحث برقم القضية...',
      variant: 'text',
      icon: FileText
    },
  },
  {
    accessorKey: 'title',
    header: ({ column }: { column: Column<CaseSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='عنوان القضية' />
    ),
     meta: {
      label: 'عنوان القضية',
      placeholder: 'بحث بالعنوان...',
      variant: 'text',
      icon: FileText
    },
    enableColumnFilter: true,
  },
  {
    accessorKey: 'status',
    header: ({ column }: { column: Column<CaseSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='الحالة' />
    ),
    cell: ({ row }) => {
      const status = row.original.status;
      return (
        <Badge variant='outline'>
          {CaseStatusLabels[status]}
        </Badge>
      );
    }
  },
  {
    accessorKey: 'priority',
    header: ({ column }: { column: Column<CaseSummary, unknown> }) => (
      <DataTableColumnHeader column={column} title='الأولوية' />
    ),
    cell: ({ row }) => {
      const priority = row.original.priority;
      return (
        <Badge variant={priority >= 3 ? 'destructive' : 'secondary'}>
          {PriorityLevelLabels[priority]}
        </Badge>
      );
    }
  },
   {
    accessorKey: 'investigatingOfficer',
    header: 'ضابط التحقيق',
  },
  {
    accessorKey: 'openDate',
    header: 'تاريخ القضية',
    cell: ({ row }) => new Date(row.original.openDate).toLocaleDateString('en-UK')
  },
  {
    id: 'actions',
    cell: ({ row }) => <CellAction data={row.original} />
  }
];
