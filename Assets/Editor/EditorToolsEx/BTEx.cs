using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using Editor.View;
using Sirenix.Serialization;
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
/// <summary>
/// 用奥丁去序列化克隆选择的节点
/// </summary>
/// <param name="nodes"></param>
/// <returns></returns>
    public static List<BTNodeBase> CloneData(this List<BTNodeBase> nodes)
    {
        byte[] nodeBytes= SerializationUtility.SerializeValue(nodes, DataFormat.Binary);
        var toNode = SerializationUtility.DeserializeValue<List<BTNodeBase>>(nodeBytes ,DataFormat.Binary);
       
        //删掉未复制的子数据 并随机新的Guid 位置向右下偏移
        for (int i = 0; i < toNode.Count; i++)
        {
            toNode[i].Guid = System.Guid.NewGuid().ToString();
            switch (toNode[i])
            {
                case BtComposite composite:
                    if (composite.ChildNodes .Count==0)break;
                    composite.ChildNodes = composite.ChildNodes.Intersect(toNode).ToList();
                    break;
                case BtPrecondition precondition:
                    if (precondition.ChildNode == null) break;
                    if (!toNode.Exists(n=>n==precondition.ChildNode))
                    {
                        precondition.ChildNode = null;
                    }
                    break;
            }

            toNode[i].Position += Vector2.one * 30;
        }

        return toNode;
    }
}
