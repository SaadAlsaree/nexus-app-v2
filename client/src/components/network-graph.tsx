'use client';

import React, { useMemo, useRef } from 'react';
import dynamic from 'next/dynamic';
import { darkTheme } from 'reagraph';
import { Button } from '@/components/ui/button';
import { Printer } from 'lucide-react';


// Dynamically import GraphCanvas to avoid SSR issues with WebGL
const GraphCanvas = dynamic(
  () => import('reagraph').then((mod) => mod.GraphCanvas),
  { ssr: false }
);

interface CustomNetworkGraphProps {
  data: {
    nodes: any[];
    links: any[];
  };
  theme?: 'light' | 'dark';
}

const NetworkGraph = ({ data, theme = 'light' }: CustomNetworkGraphProps) => {
  const graphRef = useRef<any>(null);

  // 1. Transform Nodes
  const nodes = useMemo(() => {
    if (!data?.nodes) return [];
    return data.nodes.map((node) => ({
      id: String(node.id),
      label: node.label,
          // Custom data
      data: { ...node },
      // Styling  
      fill: node.group === '1' ? '#ef4444' : '#3b82f6', // Red for main target, Blue for others
    }));
  }, [data]);

  // 2. Transform Edges
  const edges = useMemo(() => {
    if (!data?.links) return [];
    return data.links.map((link, index) => {
      // Handle source/target as object or string
      const source = typeof link.source === 'object' ? link.source.id : link.source;
      const target = typeof link.target === 'object' ? link.target.id : link.target;
      
      return {
        id: `e-${index}-${source}-${target}`,
        source: String(source),
        target: String(target),
        label: link.label,
        size: 1,
      };
    });
  }, [data]);

  const handleExport = () => {
    if (graphRef.current) {
      const dataUrl = graphRef.current.exportCanvas();
      const link = document.createElement('a');
      link.download = `network-graph-${new Date().getTime()}.png`;
      link.href = dataUrl;
      link.click();
    }
  };

  // console.log('Reagraph Nodes:', nodes);
  // console.log('Reagraph Edges:', edges);

  return (
    <div className="relative h-[calc(100vh-20rem)] w-full rounded-lg overflow-hidden border border-slate-200 dark:border-slate-800 bg-slate-100 dark:bg-slate-950">
      <div className="absolute top-4 left-4 z-20 flex gap-2">
        <Button 
          variant="secondary" 
          size="sm" 
          onClick={handleExport}
          className="bg-white/90 dark:bg-slate-900/90 backdrop-blur-md border border-slate-200 dark:border-slate-700 shadow-lg hover:bg-slate-100 dark:hover:bg-slate-800"
        >
          <Printer className="w-4 h-4 mr-2" />
          طباعة الجراف
        </Button>
      </div>

      <GraphCanvas
        ref={graphRef}
        nodes={nodes}
        edges={edges}

        labelType="all"
        edgeLabelPosition="natural"
        edgeArrowPosition="end"
        theme={ theme === 'dark' ? darkTheme : {
          // Simplified theme to prevent undefined access errors
          canvas: {
            background: 'transparent',
            fog: '#ffffff'
          },
          node: {
            fill: '#7b7b7b',
            activeFill: '#1DE9AC',
            opacity: 1,
            selectedOpacity: 1,
            inactiveOpacity: 0.2,
            label: {
              color: '#1e293b',
              stroke: '#ffffff',
              activeColor: '#1DE9AC'
            }
          },
          edge: {
            fill: '#64748b',
            activeFill: '#1DE9AC',
            opacity: 0.6,
            selectedOpacity: 1,
            inactiveOpacity: 0.1,
            label: {
              color: '#1e293b',
              stroke: '#ffffff',
              activeColor: '#1DE9AC'
            }
          },
          arrow: {
            fill: '#64748b',
            activeFill: '#1DE9AC',
          
          },
          ring: {
            fill: '#D8E6EA',
            activeFill: '#1DE9AC'
          },
          lasso: {
            background: 'rgba(59, 130, 246, 0.1)',
            border: '#2563eb'
          }
        }}
        layoutType="forceDirected2d"
        draggable={true} 
      />
      
      {/* Legend Overlay */}
      <div className="absolute top-4 right-4 bg-white/90 dark:bg-slate-900/90 p-3 rounded-lg border border-slate-200 dark:border-slate-700 backdrop-blur-md shadow-lg pointer-events-none z-10">
        <div className="flex flex-col gap-2">
          <div className="flex items-center gap-2">
            <div className="w-3 h-3 rounded-full bg-rose-500" />
            <span className="text-xs font-bold text-slate-700 dark:text-slate-200">هدف رئيسي</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-3 h-3 rounded-full bg-blue-500" />
            <span className="text-xs font-bold text-slate-700 dark:text-slate-200">ارتباط</span>
          </div>
        </div>
      </div>
      
       {/* Debug Overlay */}
       <div className="absolute bottom-4 left-4 bg-white/90 dark:bg-slate-900/90 p-3 rounded-lg border border-slate-200 dark:border-slate-700 backdrop-blur-md shadow-lg pointer-events-none z-10">
          <div className="text-xs font-mono text-slate-700 dark:text-slate-200">
            <div>العقد: {nodes.length}</div>
            <div>الروابط: {edges.length}</div>
          </div>
        </div>
    </div>
  );
};

export default NetworkGraph;