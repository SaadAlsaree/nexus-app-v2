import { EvidenceList } from '@/features/evidence/components/evidence-list';
import { Metadata } from 'next';

export const metadata: Metadata = {
    title: 'الأدلة والمرفقات',
    description: 'عرض وإدارة جميع الأدلة والمرفقات في النظام',
};

export default function EvidencePage() {
    return (
        <div className="flex-1 space-y-4 p-8 pt-6">
            <div className="flex items-center justify-between space-y-2">
                <h2 className="text-3xl font-bold tracking-tight">الأدلة والمرفقات</h2>
            </div>
            <div className="grid gap-4">
                <EvidenceList />
            </div>
        </div>
    );
}
