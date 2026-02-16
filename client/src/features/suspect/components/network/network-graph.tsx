'use client';

import { useState, useEffect } from 'react';
import { getSuspectNetwork } from '@/features/suspect/api/suspect.service';
import { NetworkSuspect } from '@/features/suspect/types/suspect';
import NetworkGraph from '@/components/network-graph';
import { Card, CardContent, } from '@/components/ui/card';
import { Loader2 } from 'lucide-react';

export default function NetworkPage({id, theme}: {id?: string, theme?: 'light' | 'dark'}) {
  const [networkData, setNetworkData] = useState<NetworkSuspect | null>(null);
  const [isLoading, setIsLoading] = useState(false);


  useEffect(() => {
    if (!id) return;

    const fetchNetwork = async () => {
      setIsLoading(true);
      try {
        const response = await getSuspectNetwork(id);
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
  }, [id]);

  return (
      <div className='space-y-4'>
        <Card className='overflow-hidden border border-slate-700'>
          <CardContent className='p-0'>
            {networkData ? (
              <NetworkGraph data={networkData} theme={theme} />
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
  );
}
