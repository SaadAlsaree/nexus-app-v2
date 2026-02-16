'use client';

import { useState, useMemo, useCallback } from 'react';
import ReactFlow, {
  Node,
  Edge,
  Background,
  Controls,
  MiniMap,
  useNodesState,
  useEdgesState,
  NodeTypes,
  Panel,
  ReactFlowProvider
} from 'reactflow';
import 'reactflow/dist/style.css';
import OrgUnitFlowNode from './org-unit-flow-node';
import { Card } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import {
  IconRefresh,
  IconLayoutGrid,
  IconCircles,
  IconArrowsDiagonal
} from '@tabler/icons-react';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select';
import { OrgUnitHierarchyNode, OrgUnitLevelLabels } from '../types/organization';

type LayoutType = 'hierarchical' | 'horizontal' | 'radial';

interface OrgUnitsFlowProps {
  initialData: OrgUnitHierarchyNode[];
}

// Main component wrapped with ReactFlowProvider
export function OrgUnitsFlow({ initialData }: OrgUnitsFlowProps) {
  return (
    <ReactFlowProvider>
      <FlowContent initialData={initialData} />
    </ReactFlowProvider>
  );
}

// The actual flow content component
function FlowContent({ initialData }: OrgUnitsFlowProps) {
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);
  const [selectedNode, setSelectedNode] = useState<Node | null>(null);
  const [reactFlowInstance, setReactFlowInstance] = useState<any>(null);
  const [layoutType, setLayoutType] = useState<LayoutType>('hierarchical');

  // Define custom node types
  const nodeTypes: NodeTypes = useMemo(
    () => ({
      orgUnit: OrgUnitFlowNode
    }),
    []
  );

  // Helper function to calculate tree dimensions for hierarchical layout
  const calculateTreeWidth = useCallback(
    (node: OrgUnitHierarchyNode): number => {
      if (!node || !node.subUnits || node.subUnits.length === 0) {
        return 250;
      }

      let totalWidth = 0;
      node.subUnits.forEach((child) => {
        totalWidth += calculateTreeWidth(child);
      });

      const childSpacing = Math.max(80, 20 * node.subUnits.length);
      return Math.max(
        totalWidth + (node.subUnits.length - 1) * childSpacing,
        250
      );
    },
    []
  );

  // Helper function to calculate tree height for horizontal layout
  const calculateTreeHeight = useCallback(
    (node: OrgUnitHierarchyNode): number => {
      if (!node || !node.subUnits || node.subUnits.length === 0) {
        return 180;
      }

      let maxHeight = 0;
      node.subUnits.forEach((child) => {
        maxHeight = Math.max(maxHeight, calculateTreeHeight(child));
      });

      const childSpacing = Math.max(60, 15 * node.subUnits.length);
      return Math.max(
        maxHeight + (node.subUnits.length - 1) * childSpacing,
        180
      );
    },
    []
  );

  // Process the org data into nodes and edges for React Flow
  const processOrgData = useCallback(
    (data: OrgUnitHierarchyNode[], layout: LayoutType) => {
      const nodes: Node[] = [];
      const edges: Edge[] = [];

      if (!data || !Array.isArray(data) || data.length === 0) {
        return { nodes, edges };
      }

      const MIN_HORIZONTAL_SPACING = 100;
      const MIN_VERTICAL_SPACING = 250;
      const MIN_HORIZONTAL_LEVEL_SPACING = 400;

      function processNodeHierarchical(
        node: OrgUnitHierarchyNode,
        level: number,
        xOffset: number
      ) {
        if (!node || !node.id) return;

        const verticalSpacing = MIN_VERTICAL_SPACING + level * 20;

        nodes.push({
          id: node.id,
          type: 'orgUnit',
          position: { x: xOffset, y: level * verticalSpacing },
          data: {
            unitName: node.unitName || '',
            level: node.level || 0,
            levelName: OrgUnitLevelLabels[node.level] || '',
            path: node.path || '',
            subUnitsCount: node.subUnits?.length || 0
          }
        });

        if (node.subUnits && node.subUnits.length > 0) {
          const childWidths = node.subUnits.map((child) =>
            calculateTreeWidth(child)
          );
          const totalWidth = childWidths.reduce((sum, width) => sum + width, 0);

          const childSpacing = Math.max(
            MIN_HORIZONTAL_SPACING,
            30 + node.subUnits.length * 10
          );
          const totalSpacing = (node.subUnits.length - 1) * childSpacing;
          const totalRequiredWidth = totalWidth + totalSpacing;

          let currentX = xOffset - totalRequiredWidth / 2;

          node.subUnits.forEach((child, index) => {
            if (!child || !child.id) return;

            const childTreeWidth = childWidths[index];
            const childCenterX = currentX + childTreeWidth / 2;

            edges.push({
              id: `e-${node.id}-${child.id}`,
              source: node.id,
              target: child.id,
              type: 'smoothstep',
              style: { stroke: '#b1b1b7', strokeWidth: 2 }
            });

            processNodeHierarchical(child, level + 1, childCenterX);
            currentX += childTreeWidth + childSpacing;
          });
        }
      }

      function processNodeHorizontal(
        node: OrgUnitHierarchyNode,
        level: number,
        yOffset: number
      ) {
        if (!node || !node.id) return;

        const horizontalSpacing = MIN_HORIZONTAL_LEVEL_SPACING + level * 50;

        nodes.push({
          id: node.id,
          type: 'orgUnit',
          position: { x: level * horizontalSpacing, y: yOffset },
          data: {
            unitName: node.unitName || '',
            level: node.level || 0,
            levelName: OrgUnitLevelLabels[node.level] || '',
            path: node.path || '',
            subUnitsCount: node.subUnits?.length || 0
          }
        });

        if (node.subUnits && node.subUnits.length > 0) {
          const childHeights = node.subUnits.map((child) =>
            calculateTreeHeight(child)
          );
          const totalHeight = childHeights.reduce(
            (sum, height) => sum + height,
            0
          );

          const childSpacing = Math.max(80, 20 + node.subUnits.length * 8);
          const totalSpacing = (node.subUnits.length - 1) * childSpacing;
          const totalRequiredHeight = totalHeight + totalSpacing;

          let currentY = yOffset - totalRequiredHeight / 2;

          node.subUnits.forEach((child, index) => {
            if (!child || !child.id) return;

            const childTreeHeight = childHeights[index];
            const childCenterY = currentY + childTreeHeight / 2;

            edges.push({
              id: `e-${node.id}-${child.id}`,
              source: node.id,
              target: child.id,
              type: 'smoothstep',
              style: { stroke: '#b1b1b7', strokeWidth: 2 }
            });

            processNodeHorizontal(child, level + 1, childCenterY);
            currentY += childTreeHeight + childSpacing;
          });
        }
      }

      function processNodeRadial(
        node: OrgUnitHierarchyNode,
        level: number,
        angle: number,
        radius: number
      ) {
        if (!node || !node.id) return;

        const baseRadius = 350;
        const radiusMultiplier = radius + level * 0.3;
        const dynamicRadius = baseRadius * radiusMultiplier;

        const x = Math.cos(angle) * dynamicRadius;
        const y = Math.sin(angle) * dynamicRadius;

        nodes.push({
          id: node.id,
          type: 'orgUnit',
          position: { x, y },
          data: {
            unitName: node.unitName || '',
            level: node.level || 0,
            levelName: OrgUnitLevelLabels[node.level] || '',
            path: node.path || '',
            subUnitsCount: node.subUnits?.length || 0
          }
        });

        if (node.subUnits && node.subUnits.length > 0) {
          const angleStep = (2 * Math.PI) / node.subUnits.length;
          const radiusIncrement = 1.2 + (node.subUnits.length > 5 ? 0.3 : 0);

          node.subUnits.forEach((child, index) => {
            if (!child || !child.id) return;

            edges.push({
              id: `e-${node.id}-${child.id}`,
              source: node.id,
              target: child.id,
              type: 'smoothstep',
              style: { stroke: '#b1b1b7', strokeWidth: 2 }
            });

            const childAngle = angle + angleStep * index;
            processNodeRadial(
              child,
              level + 1,
              childAngle,
              radius + radiusIncrement
            );
          });
        }
      }

      // Process based on layout type
      data.forEach((rootNode, index) => {
        switch (layout) {
          case 'horizontal': {
            let yOffset = 0;
            for (let i = 0; i < index; i++) {
              const prevTreeHeight = calculateTreeHeight(data[i]);
              yOffset += prevTreeHeight + 300;
            }
            processNodeHorizontal(rootNode, 0, yOffset);
            break;
          }
          case 'radial': {
            const rootAngle = (index * 2 * Math.PI) / data.length;
            processNodeRadial(rootNode, 0, rootAngle, 1.5);
            break;
          }
          default: // hierarchical
            let xOffset = 0;
            for (let i = 0; i < index; i++) {
              const prevTreeWidth = calculateTreeWidth(data[i]);
              xOffset += prevTreeWidth + 400;
            }
            processNodeHierarchical(rootNode, 0, xOffset);
        }
      });

      return { nodes, edges };
    },
    [calculateTreeWidth, calculateTreeHeight]
  );

  // Initialize and update the flow with the selected layout
  useMemo(() => {
    if (
      !initialData ||
      !Array.isArray(initialData) ||
      initialData.length === 0
    ) {
      setNodes([]);
      setEdges([]);
      return;
    }

    const { nodes: flowNodes, edges: flowEdges } = processOrgData(
      initialData,
      layoutType
    );
    setNodes(flowNodes);
    setEdges(flowEdges);

    if (reactFlowInstance && flowNodes.length > 0) {
      setTimeout(() => {
        reactFlowInstance.fitView({
          duration: 600,
          padding: 0.25,
          includeHiddenNodes: false,
          minZoom: 0.1,
          maxZoom: 1.5
        });
      }, 150);
    }
  }, [
    initialData,
    processOrgData,
    setNodes,
    setEdges,
    layoutType,
    reactFlowInstance
  ]);

  const onNodeClick = useCallback((event: React.MouseEvent, node: Node) => {
    setSelectedNode(node);
  }, []);

  const resetView = useCallback(() => {
    if (!reactFlowInstance) return;
    reactFlowInstance.fitView({
      duration: 800,
      padding: 0.3,
      includeHiddenNodes: false,
      minZoom: 0.1,
      maxZoom: 1.2
    });
    setSelectedNode(null);
  }, [reactFlowInstance]);

  const onLayoutChange = useCallback(
    (newLayout: LayoutType) => {
      setLayoutType(newLayout);
      setTimeout(() => {
        if (reactFlowInstance) {
          reactFlowInstance.fitView({
            duration: 800,
            padding: 0.2,
            includeHiddenNodes: false,
            minZoom: 0.1,
            maxZoom: 1.5
          });
        }
      }, 100);
    },
    [reactFlowInstance]
  );

  return (
    <Card className='h-[700px] overflow-hidden'>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        nodeTypes={nodeTypes}
        onNodeClick={onNodeClick}
        onInit={setReactFlowInstance}
        fitView
        fitViewOptions={{
          padding: 0.25,
          includeHiddenNodes: false,
          minZoom: 0.05,
          maxZoom: 1.5
        }}
        attributionPosition='bottom-right'
        minZoom={0.05}
        maxZoom={2.5}
      >
        <Controls />
        <MiniMap />
        <Background color='#f0f0f0' gap={16} size={1} />

        <Panel position='top-right' className='flex flex-col gap-3'>
          <Card className='p-3'>
            <div className='mb-2 text-sm font-medium'>أدوات التحكم</div>
            <div className='flex flex-col gap-2'>
              <Select
                value={layoutType}
                onValueChange={(value: LayoutType) => onLayoutChange(value)}
              >
                <SelectTrigger className='w-[200px]'>
                  <SelectValue placeholder='اختر نوع العرض' />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value='hierarchical'>
                    <div className='flex items-center gap-2'>
                      <IconLayoutGrid size={16} />
                      <span>عرض هرمي</span>
                    </div>
                  </SelectItem>
                  <SelectItem value='horizontal'>
                    <div className='flex items-center gap-2'>
                      <IconArrowsDiagonal size={16} />
                      <span>عرض أفقي</span>
                    </div>
                  </SelectItem>
                  <SelectItem value='radial'>
                    <div className='flex items-center gap-2'>
                      <IconCircles size={16} />
                      <span>عرض دائري</span>
                    </div>
                  </SelectItem>
                </SelectContent>
              </Select>

              <Button
                onClick={resetView}
                variant='outline'
                size='sm'
                className='flex items-center gap-1'
              >
                <IconRefresh size={16} />
                إعادة تعيين العرض
              </Button>
            </div>
          </Card>

          {selectedNode && (
            <Card className='p-3'>
              <div className='mb-2 text-sm font-medium'>تفاصيل الوحدة</div>
              <div className='space-y-1'>
                <div className='font-medium'>{selectedNode.data.unitName}</div>
                <div className='text-xs text-gray-600'>
                  المستوى: {selectedNode.data.levelName}
                </div>
                <div className='text-xs text-gray-600'>
                  المسار: {selectedNode.data.path}
                </div>
                {selectedNode.data.subUnitsCount > 0 && (
                  <div className='text-xs text-gray-600'>
                    الوحدات الفرعية: {selectedNode.data.subUnitsCount}
                  </div>
                )}
              </div>
            </Card>
          )}
        </Panel>
      </ReactFlow>
    </Card>
  );
}
