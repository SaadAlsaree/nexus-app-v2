'use client';
import { Badge } from '@/components/ui/badge';
import { DataTableColumnHeader } from '@/components/ui/table/data-table-column-header';
import { Suspect } from '../../types/suspect';
import { Column, ColumnDef } from '@tanstack/react-table';
import { User, Text } from 'lucide-react';
import Image from 'next/image';
import { CellAction } from './cell-action';
import { STATUS_OPTIONS } from './options';
import Link from 'next/link';

export const columns: ColumnDef<Suspect>[] = [
  {
    id: 'suspect',
    header: ({ column }: { column: Column<Suspect, unknown> }) => (
      <DataTableColumnHeader column={column} title='المتهم' />
    ),
    cell: ({ row }) => {
      const photoUrl = row.original.photoUrl;
      const fullName = row.original.fullName;
      const id = row.original.id;

      return (
        <div className='flex items-center gap-3'>
          <div className='relative aspect-square h-10 w-10 shrink-0'>
            <Image
              src={photoUrl || '/placeholder.png'}
              alt={fullName}
              width={40}
              height={40}
              className='rounded-lg object-cover'
              unoptimized={!!photoUrl?.includes('localhost')}
            />
          </div>
          <Link href={`/suspect/${id}`}>
            <h1 className='text-blue-500 hover:text-blue-700 transition-colors duration-200 cursor-pointer font-medium'>
              {fullName}
            </h1>
          </Link>
        </div>
      );
    },
    meta: {
      label: 'المتهم',
      placeholder: 'بحث المتهم...',
      variant: 'text',
      icon: User
    },
    enableColumnFilter: true,
    accessorFn: (row) => row.fullName
  },
  {
    id: 'kunya',
    accessorKey: 'kunya',
    header: ({ column }: { column: Column<Suspect, unknown> }) => (
      <DataTableColumnHeader column={column} title='الكنية' />
    ),
    cell: ({ cell }) => <div>{cell.getValue<string>()}</div>,
    meta: {
      label: 'الكنية',
      placeholder: 'بحث الكنية...',
      variant: 'text',
      icon: Text
    },
    enableColumnFilter: true
  },
  {
    id: 'currentStatus',
    accessorKey: 'currentStatusName',
    header: ({ column }: { column: Column<Suspect, unknown> }) => (
      <DataTableColumnHeader column={column} title='الحالة' />
    ),
    cell: ({ row }) => {
      const status = row.original.currentStatusName;
      return (
        <Badge variant='outline' className='capitalize'>
          {status}
        </Badge>
      );
    },
    enableColumnFilter: true,
    meta: {
      label: 'الحالة',
      variant: 'multiSelect',
      options: STATUS_OPTIONS
    }
  },
  {
    accessorKey: 'placeOfBirth',
    header: 'مكان الميلاد',
    cell: ({ cell }) => <span>{cell.getValue<string>()}</span>
  },
  {
    accessorKey: 'legalArticle',
    header: 'المادة القانونية/ملاحظات'
  },
  {
    id: 'الإجراءات',
    cell: ({ row }) => <CellAction data={row.original} />
  }
];
