using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Editor.View
{
    /// <summary>
    /// 个体节点所涉及的方法定义在节点视图中
    /// </summary>
    public class NodeView : Node
    {
        public BTNodeBase NodeData;

        public Port InputPort;
        public Port OutputPort;


        public NodeView(BTNodeBase nodeData)
        {
            this.NodeData = nodeData;
            InitNodeView(nodeData);
        }

        public void InitNodeView(BTNodeBase nodeData)
        {
            //这边自定义节点名字和节点类型名需要拆分
            this.title = "【" +
                         nodeData.GetType().GetField("NodeEditorName").GetValue(null)
                         + "】  "+nodeData.NodeName;
            InputPort = + this;
            switch (nodeData)
            {
                //这里尝试使用一个新语法糖，通过重载运输操作符号 +（加号）和-（减号）来加载端口
                //根据重载的参数不同，决定运算符的位置
                case BtComposite composite:
                    OutputPort = this - false;
                    break;
                case BtPrecondition precondition:
                    OutputPort = this - true;
                    break;
                /*case BtActionNode actionNode:
                    break;*/
            }

            InputPort.portName = "输入端";
            if (OutputPort!=null)
                OutputPort.portName = "输出端";
            
            inputContainer.Add(InputPort);
            outputContainer.Add(OutputPort);
            
        }

        public void LinkLine()
        {
            TreeView graphView = BehaviourTreeWindows.windowsRoot.treeView;
            switch (NodeData)
            {
                case BtComposite composite:
                    composite.ChildNodes.ForEach(n=>
                    {
                        graphView.AddElement(PortLink(OutputPort, graphView.NodeViews[n.Guid].InputPort));
                       
                    });
                    break;
                case BtPrecondition precondition:
                    graphView.AddElement(PortLink(OutputPort, graphView.NodeViews[precondition.ChildNode.Guid].InputPort));
                    break;
            }
        }

        public static  Edge PortLink(Port p1, Port p2)
        {
            var tempEdge = new Edge()
            {
                output = p1,
                input = p2
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            return tempEdge;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction( actionName: "设置为根节点", SetRoot);
        }
        private void SetRoot(DropdownMenuAction obj)=>BTSetting.GetSetting().SetRoot(NodeData);

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            NodeData.Position = new Vector2(newPos.xMin, newPos.yMin);
        }
        

        //重载加号运算符
        public static Port operator +(NodeView view)
        {
            Port port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input,
                Port.Capacity.Single, typeof(NodeView));
            return port;
        }

        //重载减号运算符
        public static Port operator -(NodeView view, bool isSingle)
        {
            Port port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output,
                isSingle ? Port.Capacity.Single : Port.Capacity.Multi, typeof(NodeView));
            return port;
        }
    }

    public enum PortType
    {
        无,
        Input,
        Output
    }
}