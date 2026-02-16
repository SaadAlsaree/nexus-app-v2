'use client';

import { useMemo, useCallback, useState, useEffect } from 'react';
import ReactFlow, {
  Node,
  Edge,
  Background,
  Controls,
  useNodesState,
  useEdgesState,
  NodeTypes,
  ReactFlowProvider,
  useReactFlow
} from 'reactflow';
import 'reactflow/dist/style.css';
import FamilyTreeNode from './family-tree-node';
import { SuspectDetails, RelativesAndAssociate } from '../../types/suspect';
import { RelationshipType } from '../../types/relationship';
import { Card } from '@/components/ui/card';

interface FamilyTreeFlowProps {
  suspect: SuspectDetails;
}

const nodeTypes: NodeTypes = {
  familyNode: FamilyTreeNode
};

function FlowContent({ suspect }: FamilyTreeFlowProps) {
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);
  const { fitView } = useReactFlow();

  const processData = useCallback(() => {
    const flowNodes: Node[] = [];
    const flowEdges: Edge[] = [];

    // 1. Add Main Suspect
    const mainSuspectId = suspect.id;
    flowNodes.push({
      id: mainSuspectId,
      type: 'familyNode',
      position: { x: 0, y: 0 },
      data: {
        fullName: suspect.fullName,
        relationshipName: 'الشخص المعني',
        isMainSuspect: true,
        currentStatus: suspect.currentStatusName,
      }
    });

    const relatives = suspect.relativesAndAssociates || [];
    
    // Categorize by Generation
    const genNeg1 = relatives.filter(r => [
        RelationshipType.Father, 
        RelationshipType.Mother, 
        RelationshipType.Uncle, 
        RelationshipType.Aunt, 
        RelationshipType.MaternalUncle
    ].includes(r.relationship as RelationshipType));

    const gen0 = [
        ...relatives.filter(r => [
            RelationshipType.Brother, 
            RelationshipType.Sister, 
            RelationshipType.Spouse, 
            RelationshipType.Cousin, 
            RelationshipType.MaternalCousin, 
            RelationshipType.MaternalCousinFemale
        ].includes(r.relationship as RelationshipType))
    ];

    const gen1 = relatives.filter(r => [
        RelationshipType.Son, 
        RelationshipType.Daughter
    ].includes(r.relationship as RelationshipType));

    const others = relatives.filter(r => ![
        RelationshipType.Father, RelationshipType.Mother, RelationshipType.Uncle, RelationshipType.Aunt, 
        RelationshipType.MaternalUncle, RelationshipType.Brother, RelationshipType.Sister, 
        RelationshipType.Spouse, RelationshipType.Cousin, RelationshipType.MaternalCousin, 
        RelationshipType.MaternalCousinFemale, RelationshipType.Son, RelationshipType.Daughter
    ].includes(r.relationship as RelationshipType));

    const X_SPACING = 280;
    const Y_LEVEL_SPACING = 200;

    // 2. Position Generation -1 (Parents/Uncles)
    genNeg1.forEach((rel, index) => {
      const xPos = (index - (genNeg1.length - 1) / 2) * X_SPACING;
      flowNodes.push({
        id: rel.id,
        type: 'familyNode',
        position: { x: xPos, y: -Y_LEVEL_SPACING },
        data: {
          fullName: rel.fullName,
          relationshipName: rel.relationshipName,
          currentStatus: rel.currentStatus
        }
      });

      flowEdges.push({
        id: `e-${rel.id}-${mainSuspectId}`,
        source: rel.id,
        target: mainSuspectId,
        type: 'smoothstep',
        style: { stroke: '#94a3b8', strokeWidth: 2 }
      });
    });

    // 3. Position Generation 0 (Siblings, Spouses, Cousins) around Main Suspect
    // Split into left and right of main suspect
    const leftSide = gen0.slice(0, Math.ceil(gen0.length / 2));
    const rightSide = gen0.slice(Math.ceil(gen0.length / 2));

    leftSide.forEach((rel, index) => {
        const xPos = -(index + 1) * X_SPACING;
        flowNodes.push({
            id: rel.id,
            type: 'familyNode',
            position: { x: xPos, y: 0 },
            data: {
              fullName: rel.fullName,
              relationshipName: rel.relationshipName,
              currentStatus: rel.currentStatus
            }
        });

        flowEdges.push({
            id: `e-${mainSuspectId}-${rel.id}`,
            source: mainSuspectId,
            target: rel.id,
            type: 'smoothstep',
            style: { 
                stroke: rel.relationship === RelationshipType.Spouse ? '#f472b6' : '#94a3b8', 
                strokeWidth: 2,
                strokeDasharray: rel.relationship === RelationshipType.Spouse ? '5,5' : '0'
            }
        });
    });

    rightSide.forEach((rel, index) => {
        const xPos = (index + 1) * X_SPACING;
        flowNodes.push({
            id: rel.id,
            type: 'familyNode',
            position: { x: xPos, y: 0 },
            data: {
              fullName: rel.fullName,
              relationshipName: rel.relationshipName,
              currentStatus: rel.currentStatus
            }
        });

        flowEdges.push({
            id: `e-${mainSuspectId}-${rel.id}`,
            source: mainSuspectId,
            target: rel.id,
            type: 'smoothstep',
            style: { 
                stroke: rel.relationship === RelationshipType.Spouse ? '#f472b6' : '#94a3b8', 
                strokeWidth: 2,
                strokeDasharray: rel.relationship === RelationshipType.Spouse ? '5,5' : '0'
            }
        });
    });

    // 4. Generation 1 (Children)
    gen1.forEach((rel, index) => {
      const xPos = (index - (gen1.length - 1) / 2) * X_SPACING;
      flowNodes.push({
        id: rel.id,
        type: 'familyNode',
        position: { x: xPos, y: Y_LEVEL_SPACING },
        data: {
          fullName: rel.fullName,
          relationshipName: rel.relationshipName,
          currentStatus: rel.currentStatus
        }
      });

      flowEdges.push({
        id: `e-${mainSuspectId}-${rel.id}`,
        source: mainSuspectId,
        target: rel.id,
        type: 'smoothstep',
        style: { stroke: '#3b82f6', strokeWidth: 2 }
      });
    });

    // 5. Others (Generation 2)
    others.forEach((rel, index) => {
        const xPos = (index - (others.length - 1) / 2) * X_SPACING;
        flowNodes.push({
          id: rel.id,
          type: 'familyNode',
          position: { x: xPos, y: Y_LEVEL_SPACING * 2 },
          data: {
            fullName: rel.fullName,
            relationshipName: rel.relationshipName,
            currentStatus: rel.currentStatus
          }
        });

        flowEdges.push({
          id: `e-${mainSuspectId}-${rel.id}`,
          source: mainSuspectId,
          target: rel.id,
          type: 'smoothstep',
          style: { stroke: '#cbd5e1', strokeWidth: 1 }
        });
    });

    setNodes(flowNodes);
    setEdges(flowEdges);

    // Initial fit view
    setTimeout(() => fitView({ padding: 0.2, duration: 800 }), 100);
  }, [suspect, setNodes, setEdges, fitView]);

  useEffect(() => {
    processData();
  }, [processData]);

  return (
    <div className="h-150 w-full border rounded-xl overflow-hidden bg-zinc-50 dark:bg-zinc-900/50">
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        nodeTypes={nodeTypes}
        fitView
        minZoom={0.2}
        maxZoom={1.5}
        attributionPosition="bottom-right"
      >
        <Background gap={20} color="#e5e7eb" />
        <Controls />
      </ReactFlow>
    </div>
  );
}

export function FamilyTreeFlow({ suspect }: FamilyTreeFlowProps) {
  return (
    <ReactFlowProvider>
      <FlowContent suspect={suspect} />
    </ReactFlowProvider>
  );
}
