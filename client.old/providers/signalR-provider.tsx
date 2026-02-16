'use client'; // ضروري جداً لأن SignalR يعمل في المتصفح فقط

import React, { createContext, useContext, useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import { SystemAlert } from '@/types/alert';

interface SignalRContextType {
  connection: signalR.HubConnection | null;
  isConnected: boolean;
  latestAlert: SystemAlert | null; // آخر تنبيه وصل (لعرضه في Toast)
  unreadCount: number;
}

const SignalRContext = createContext<SignalRContextType>({
  connection: null,
  isConnected: false,
  latestAlert: null,
  unreadCount: 0,
});

export const useSignalR = () => useContext(SignalRContext);

export default function SignalRProvider({ children }: { children: React.ReactNode }) {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [latestAlert, setLatestAlert] = useState<SystemAlert | null>(null);
  const [unreadCount, setUnreadCount] = useState(0);

  useEffect(() => {
    // 1. إعداد الاتصال
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_API_URL}/hubs/notifications`, {
        // جلب التوكن من LocalStorage أو NextAuth Session
        accessTokenFactory: () => localStorage.getItem('token') || '', 
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect() // إعادة الاتصال تلقائياً عند انقطاع النت
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      // 2. بدء الاتصال
      connection
        .start()
        .then(() => {
          console.log('✅ SignalR Connected');
          setIsConnected(true);
          
          // تسجيل المستمعين (Event Listeners)
          
          // أ. استقبال تنبيه أمني
          connection.on('ReceiveSecurityAlert', (alert: SystemAlert) => {
            console.warn('🚨 SECURITY ALERT:', alert);
            setLatestAlert(alert);
            setUnreadCount((prev) => prev + 1);
            
            // تشغيل صوت تنبيه إذا كان الخطر مرتفعاً
            if (alert.level >= 3) {
               playAlertSound();
            }
          });

          // ب. تحديث العداد
          connection.on('UpdateUnreadCount', (count: number) => {
            setUnreadCount(count);
          });
        })
        .catch((err) => console.error('❌ SignalR Connection Error: ', err));

      // تنظيف الاتصال عند إغلاق التطبيق
      return () => {
        connection.stop();
      };
    }
  }, [connection]);

  const playAlertSound = () => {
    const audio = new Audio('/sounds/alert.mp3');
    audio.play().catch(e => console.log("Audio play blocked", e));
  };

  return (
    <SignalRContext.Provider value={{ connection, isConnected, latestAlert, unreadCount }}>
      {children}
    </SignalRContext.Provider>
  );
}