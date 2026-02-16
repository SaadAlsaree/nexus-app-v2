'use client';

import { useState, useEffect } from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import OrgUnitsTablePage from './org-units-table';
import OrgUnitsHierarchy from './org-units-hierarchy';
import { OrgUnitsFlow } from './org-units-flow';
import { getOrganizationalUnitsHierarchy } from '../api/organization.service';
import { OrgUnitHierarchyNode } from '../types/organization';
import { toast } from 'sonner';

export default function OrgUnitsPage() {
    const [hierarchy, setHierarchy] = useState<OrgUnitHierarchyNode[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchHierarchy = async () => {
        try {
            setLoading(true);
            const response = await getOrganizationalUnitsHierarchy();
            if (response.succeeded && response.data) {
                setHierarchy(response.data);
            } else {
                toast.error('فشل في تحميل الهيكل التنظيمي');
            }
        } catch (error) {
            console.error('Error fetching hierarchy:', error);
            toast.error('حدث خطأ أثناء تحميل البيانات');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchHierarchy();
    }, []);

    return (
        <div className='space-y-4'>
            <Tabs defaultValue="hierarchy">
                <TabsList className='w-full'>
                    {/* <TabsTrigger value="units">الوحدات التنظيمية</TabsTrigger> */}
                    <TabsTrigger value="hierarchy">الهيكل التنظيمي (قائمة)</TabsTrigger>
                    <TabsTrigger value="flow">الهيكل التنظيمي (مخطط)</TabsTrigger>
                </TabsList>
                <div className='w-full min-h-[calc(100vh-16rem)]'>
                    {/* <TabsContent value="units" className="space-y-4 min-h-[calc(100vh-16rem)]">
                        <OrgUnitsTablePage />
                    </TabsContent> */}
                    <TabsContent value="hierarchy" className="space-y-4 min-h-[calc(100vh-16rem)]">
                        <OrgUnitsHierarchy hierarchy={hierarchy} onRefresh={fetchHierarchy} />
                    </TabsContent>
                    <TabsContent value="flow" className="space-y-4 min-h-[calc(100vh-16rem)]">
                        {loading ? (
                            <div className="flex items-center justify-center">
                                <div className="text-muted-foreground">جاري التحميل...</div>
                            </div>
                        ) : (
                            <OrgUnitsFlow initialData={hierarchy} />
                        )}
                    </TabsContent>
                </div>
            </Tabs>
        </div>
    );
}