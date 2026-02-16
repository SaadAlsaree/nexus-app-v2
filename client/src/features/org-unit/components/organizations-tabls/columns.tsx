'use client';
import { Badge } from '@/components/ui/badge';
import { DataTableColumnHeader } from '@/components/ui/table/data-table-column-header';
import { OrgUnitListItem, OrgUnitLevelLabels } from '../../types/organization';
import { Column, ColumnDef } from '@tanstack/react-table';
import { Building2, Text } from 'lucide-react';
import { CellAction } from './cell-action';
import { LEVEL_OPTIONS } from './options';


export const columns : ColumnDef<OrgUnitListItem>[] = [
  {
    id: 'unitName',
    header: ({ column }: { column: Column<OrgUnitListItem, unknown> }) => (
      <DataTableColumnHeader column={column} title='اسم الوحدة' />
    ),
    cell: ({ row }) => {
      const unitName = row.original.unitName;
      const id = row.original.id;

      return (
        <div className='flex items-center gap-3'>
          <Building2 className='h-5 w-5 text-muted-foreground' />
          <h1 className='font-medium'>
            {unitName}
          </h1>
        </div>
      );
    },
    meta: {
      label: 'اسم الوحدة',
      placeholder: 'بحث الوحدة...',
      variant: 'text',
      icon: Building2
    },
    enableColumnFilter: true,
    accessorFn: (row) => row.unitName
  },
  {
    id: 'level',
    accessorKey: 'level',
    header: ({ column }: { column: Column<OrgUnitListItem, unknown> }) => (
      <DataTableColumnHeader column={column} title='المستوى' />
    ),
    cell: ({ row }) => {
      const level = row.original.level;
      return (
        <Badge variant='outline' className='capitalize'>
          {OrgUnitLevelLabels[level]}
        </Badge>
      );
    },
    enableColumnFilter: true,
    meta: {
      label: 'المستوى',
      variant: 'multiSelect',
      options: LEVEL_OPTIONS
    }
  },
  {
    id: 'path',
    accessorKey: 'path',
    header: ({ column }: { column: Column<OrgUnitListItem, unknown> }) => (
      <DataTableColumnHeader column={column} title='المسار التنظيمي' />
    ),
    cell: ({ cell }) => (
      <div className='max-w-md truncate' title={cell.getValue<string>()}>
        {cell.getValue<string>()}
      </div>
    ),
    meta: {
      label: 'المسار',
      placeholder: 'بحث المسار...',
      variant: 'text',
      icon: Text
    },
    enableColumnFilter: true
  },
  {
    accessorKey: 'subUnitsCount',
    header: ({ column }: { column: Column<OrgUnitListItem, unknown> }) => (
      <DataTableColumnHeader column={column} title='عدد الوحدات الفرعية' />
    ),
    cell: ({ cell }) => <Badge variant='secondary'>{cell.getValue<number>()}</Badge>
  },
  {
    accessorKey: 'createdAt',
    header: ({ column }: { column: Column<OrgUnitListItem, unknown> }) => (
      <DataTableColumnHeader column={column} title='تاريخ الإنشاء' />
    ),
    cell: ({ cell }) => {
      const date = cell.getValue<string>();
      if (!date) return 'غير محدد';
      return new Date(date).toLocaleDateString('en-EG', {
        year: 'numeric',
        month: 'numeric',
        day: 'numeric'
      });
    }
  },
  {
    id: 'الإجراءات',
    cell: ({ row }) => <CellAction data={row.original} />
  }
];
