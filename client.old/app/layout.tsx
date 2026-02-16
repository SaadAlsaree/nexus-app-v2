import SignalRProvider from '@/providers/signalR-provider';

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="ar" dir="rtl">
      <body>
         {/* الآن كل مكونات التطبيق يمكنها الوصول للاتصال */}
        <SignalRProvider>
          {children}
          {/* يمكن وضع مكون Toast هنا لعرض التنبيهات عالمياً */}
          <GlobalAlertToast /> 
        </SignalRProvider>
      </body>
    </html>
  );
}