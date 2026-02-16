'use client';

import { useState } from 'react';
import { EvidenceList } from '../components/evidence-list';
import { EvidenceUploadForm } from '../components/evidence-upload-form';
interface EvidenceTabProps {
    suspectId: string;
}

export function EvidenceTab({ suspectId }: EvidenceTabProps) {
    const [isUploadOpen, setIsUploadOpen] = useState(false);
    const [refreshKey, setRefreshKey] = useState(0);

    const handleUploadSuccess = () => {
        setIsUploadOpen(false);
        setRefreshKey(prev => prev + 1);
    };

    return (
        <div className="space-y-4">
            {isUploadOpen ? (
                <div className="rounded-lg border p-4 bg-white dark:bg-zinc-950">
                    <div className="flex items-center justify-between mb-4">
                        <h3 className="text-lg font-semibold">رفع دليل جديد</h3>
                    </div>
                    <EvidenceUploadForm 
                        suspectId={suspectId} 
                        onSuccess={handleUploadSuccess}
                        onCancel={() => setIsUploadOpen(false)}
                    />
                </div>
            ) : null}

            <EvidenceList 
                key={refreshKey}
                suspectId={suspectId} 
                onAddEvidence={() => setIsUploadOpen(true)} 
            />
        </div>
    );
}
