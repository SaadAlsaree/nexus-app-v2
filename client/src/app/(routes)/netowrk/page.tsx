'use client';

import { useState, useEffect } from 'react';
import PageContainer from '@/components/layout/page-container';
import { getSuspectNetwork, getSuspects } from '@/features/suspect/api/suspect.service';
import { NetworkSuspect, Suspect } from '@/features/suspect/types/suspect';
import NetworkGraph from '@/components/network-graph';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Loader2 } from 'lucide-react';
import { useTheme } from 'next-themes';

export default function NetworkPage() {
  const [suspects, setSuspects] = useState<Suspect[]>([]);
  const [selectedSuspectId, setSelectedSuspectId] = useState<string>('');
  const [networkData, setNetworkData] = useState<NetworkSuspect | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isInitialLoading, setIsInitialLoading] = useState(true);
  const { resolvedTheme } = useTheme();
  const theme = resolvedTheme === 'dark' ? 'dark' : 'light';

  useEffect(() => {
    const fetchSuspects = async () => {
      try {
        const response = await getSuspects({ page: 1, pageSize: 50 });
        if (response.data && response.data.items) {
          setSuspects(response.data.items);
          if (response.data.items.length > 0) {
            setSelectedSuspectId(response.data.items[0].id);
          }
        }
      } catch (error) {
        console.error('Failed to fetch suspects', error);
      } finally {
        setIsInitialLoading(false);
      }
    };
    fetchSuspects();
  }, []);

  useEffect(() => {
    if (!selectedSuspectId) return;

    const fetchNetwork = async () => {
      setIsLoading(true);
      try {
        const response = await getSuspectNetwork(selectedSuspectId);
        console.log('Network response:', response);
        if (response.succeeded && response.data) {
          console.log('Setting network data:', response.data);
          setNetworkData(response.data);
        }
      } catch (error) {
        console.error('Failed to fetch network', error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchNetwork();
  }, [selectedSuspectId]);

  return (
    <PageContainer scrollable={true} pageTitle='شبكة العلاقات' pageDescription='عرض العلاقات والارتباطات بين المتهمين'>
      <div className='space-y-4'>
        <Card className='bg-slate-50/50 border-slate-200'>
          <CardHeader>
            <CardTitle className='text-sm font-medium'>اختر المتهم لعرض شبكته</CardTitle>
          </CardHeader>
          <CardContent>
            <div className='flex items-center gap-4'>
              <Select
                value={selectedSuspectId}
                onValueChange={setSelectedSuspectId}
                disabled={isInitialLoading}
              >
                <SelectTrigger className='w-full md:w-[400px]'>
                  <SelectValue placeholder='اختر متهم...' />
                </SelectTrigger>
                <SelectContent>
                  {suspects.map((s) => (
                    <SelectItem key={s.id} value={s.id}>
                      {s.fullName} {s.kunya ? `(${s.kunya})` : ''}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              {isLoading && <Loader2 className='h-4 w-4 animate-spin text-primary' />}
            </div>
          </CardContent>
        </Card>

        <Card className='overflow-hidden border border-slate-700'>
          <CardContent className='p-0'>
            {networkData ? (
              <NetworkGraph data={networkData} theme={theme}/>
            ) : (
              <div className='flex h-[600px] items-center justify-center bg-slate-900 text-slate-400'>
                {isLoading ? (
                  <div className='flex flex-col items-center gap-2'>
                    <Loader2 className='h-8 w-8 animate-spin' />
                    <span>جاري تحميل البيانات...</span>
                  </div>
                ) : (
                  'لم يتم اختيار متهم أو لا توجد بيانات'
                )}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </PageContainer>
  );
}
