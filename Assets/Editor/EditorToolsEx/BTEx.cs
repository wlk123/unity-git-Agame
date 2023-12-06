using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Editor.View;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class BTEx 
{
    public static void LinkLineAddData(this Edge edge)
    {
        NodeView outputNode = edge.output.node as NodeView;
        NodeView inputNode = edge.input.node as NodeView;
        switch (outputNode.NodeData)
        {
            case BtComposite composite:
                composite.ChildNodes.Add(inputNode.NodeData);
                return;
            case BtPrecondition precondition:
                precondition.ChildNode = inputNode.NodeData;
                return;
        }
    }

    public static void UnLinkLineDelectData(this Edge edge)
    {
        NodeView outputNode = edge.output.node as NodeView;
        NodeView inputNode = edge.input.node as NodeView;
        switch (outputNode.NodeData)
        {
            case BtComposite composite:
                composite.ChildNodes.Remove(inputNode.NodeData);
                return;
            case BtPrecondition precondition:
                precondition.ChildNode =null;
                return;
        }
    }
    public static void aaa( this Edge edge)
    {
        NodeView outputNode = edge.output.node as NodeView;
        NodeView inputNode = edge.input.node as NodeView;
        switch (outputNode.NodeData)
        {
            case BtComposite composite:
                composite.ChildNodes.Remove(inputNode.NodeData);
                return;
            case BtPrecondition precondition:
                precondition.ChildNode =null;
                return;
        }
    }
   
}
