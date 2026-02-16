'use client'; // ضروري في Next.js

import dynamic from 'next/dynamic';
import { useMemo } from 'react';

// استيراد المكتبة بدون SSR
const ForceGraph2D = dynamic(() => import('react-force-graph-2d'), {
  ssr: false
});

const NetworkGraph = ({ data }) => {
  // data هو الـ JSON القادم من الباكيند (NetworkGraphDto)

  return (
    <div className="bg-slate-900 border border-slate-700 rounded-lg overflow-hidden h-[600px]">
      <ForceGraph2D
        graphData={data}
        nodeLabel="label" // الحقل الذي يظهر عند تحويم الماوس
        nodeAutoColorBy="group" // تلوين تلقائي حسب المجموعة (متهم vs آخرين)
        linkDirectionalArrowLength={3.5} // طول السهم
        linkDirectionalArrowRelPos={1} // مكان السهم
        
        // تخصيص شكل العقدة (رسم دائرة + نص)
        nodeCanvasObject={(node, ctx, globalScale) => {
          const label = node.label;
          const fontSize = 12/globalScale;
          ctx.font = `${fontSize}px Sans-Serif`;
          const textWidth = ctx.measureText(label).width;
          const bckgDimensions = [textWidth, fontSize].map(n => n + fontSize * 0.2);

          // رسم الدائرة
          ctx.beginPath();
          ctx.arc(node.x, node.y, 5, 0, 2 * Math.PI, false);
          ctx.fillStyle = node.group === "1" ? '#ef4444' : '#3b82f6'; // أحمر للمتهم، أزرق للبقية
          ctx.fill();

          // رسم النص
          ctx.textAlign = 'center';
          ctx.textBaseline = 'middle';
          ctx.fillStyle = 'rgba(255, 255, 255, 0.8)';
          ctx.fillText(label, node.x, node.y + 8);
        }}
        
        onNodeClick={node => {
          // هنا تفتح تفاصيل المتهم عند الضغط
          console.log("Clicked node:", node);
        }}
      />
    </div>
  );
};

export default NetworkGraph;