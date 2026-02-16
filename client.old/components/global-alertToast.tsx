// components/GlobalAlertToast.tsx
'use client';

import { useSignalR } from '@/providers/signalR-provider';
import { useEffect } from 'react';
import { toast } from 'react-hot-toast'; // مكتبة عرض تنبيهات مشهورة

export default function GlobalAlertToast() {
  const { latestAlert } = useSignalR();

  useEffect(() => {
    if (latestAlert) {
      // تخصيص اللون حسب الخطورة
      const isCritical = latestAlert.level >= 3;
      
      toast(latestAlert.message, {
        icon: isCritical ? '🔥' : '⚠️',
        style: {
          border: isCritical ? '2px solid red' : '1px solid orange',
          padding: '16px',
          color: '#333',
        },
        duration: isCritical ? 10000 : 4000, // التنبيه الخطير يبقى 10 ثواني
      });
    }
  }, [latestAlert]);

  return null; // هذا المكون لا يرسم شيئاً في الصفحة، هو وظيفي فقط
}