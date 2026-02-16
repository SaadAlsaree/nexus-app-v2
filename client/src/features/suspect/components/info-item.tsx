import React from 'react';

interface InfoItemProps {
  label: string;
  value: any;
  className?: string;
}

export function InfoItem({ label, value, className }: InfoItemProps) {
  return (
    <div className={className}>
      <dt className="text-muted-foreground text-sm font-medium">{label}</dt>
      <dd className="text-foreground mt-1 text-sm">
        {value || 'غير محدد'}
      </dd>
    </div>
  );
}
