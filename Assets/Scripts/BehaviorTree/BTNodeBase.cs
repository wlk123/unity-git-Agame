using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree
{

    public class GraphViewTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public Matrix4x4 matrix;
    }
   
    public class BehaviorTreeData
    {
        public BTNodeBase rootNode;
        public GraphViewTransform ViewTransform=new GraphViewTransform();
    }
    
    public enum BehaviorState
    {
        未执行 = 0,
        成功,
        失败,
        执行中
    }
    
    
    //基础构造
    public abstract class BTNodeBase
    {
        [FoldoutGroup("@NodeName"), LabelText("节点唯一标识")]
        public string Guid;//唯一标识
        [FoldoutGroup("@NodeName"), LabelText("节点位置")]
        public Vector2 Position;
        
        [FoldoutGroup("@NodeName"), LabelText("名称")]
        public string NodeName;

        
        public static string NodeEditorName="基础根节点";

        public abstract BehaviorState Tick();

        public BehaviorState NodeState=BehaviorState.未执行;
        
        public void SetGuid()=> Guid = System.Guid.NewGuid().ToString();
        
    }

    //组成节点
    public abstract class BtComposite : BTNodeBase
    {
        [FoldoutGroup("@NodeName"), LabelText("子节点")]
        public List<BTNodeBase> ChildNodes = new List<BTNodeBase>();

        public new static string NodeEditorName="组合节点";
      
        
    }

    //条件节点
    public abstract class BtPrecondition : BTNodeBase
    {
        [FoldoutGroup("@NodeName"), LabelText("子节点")]
        public BTNodeBase ChildNode;
        public new static string NodeEditorName="条件节点";
       
    }

    //行为节点
    public abstract class BtActionNode : BTNodeBase
    {
        public new static string NodeEditorName="行为节点";
    }
     
    //顺序节点
    public  class Sequence : BtComposite
    {
        private int _index;
        [FoldoutGroup("@NodeName"),SerializeField,LabelText("失败重置")]
        private bool _isReset=true;
        public new static string NodeEditorName="顺序节点";
       
        
        public override BehaviorState Tick()
        {
            var state = ChildNodes[_index].Tick();
            switch (state)
            {
                case BehaviorState.成功:
                    _index++;
                    if (_index>=ChildNodes.Count)
                    {
                        if (_isReset)
                            _index = 0;
                        NodeState = BehaviorState.成功;
                        return NodeState;
                    }
                    NodeState = BehaviorState.执行中;
                    return NodeState;
                case BehaviorState.失败:
                    //重置，失败跳出
                    if (_isReset)
                        _index = 0;
                    NodeState = BehaviorState.失败;
                    return NodeState;
                case BehaviorState.执行中:
                    NodeState = state;
                    return state;
                
            }

            NodeState = BehaviorState.未执行;
            return NodeState;
        }

    }
    
    //选择节点
    public class Selector : BtComposite
    {
        private int _index;

        [FoldoutGroup("@NodeName"),SerializeField,LabelText("失败重置")]
        private bool _isReset=true;
        public new static string NodeEditorName="选择节点";
        
        public override BehaviorState Tick()
        {
            var state = ChildNodes[_index].Tick();
            switch (state)
            {
                case BehaviorState.成功:
                    if (_isReset)
                        _index=0;
                    NodeState = state;
                    return NodeState;
                case BehaviorState.失败:
                    _index++;
                    if (_index>=ChildNodes.Count)
                    {
                        if (_isReset)
                            _index=0;
                        NodeState = BehaviorState.失败;
                        return NodeState;
                    }
                    break;
                default:
                    NodeState = state;
                    return NodeState;
                
            }
            NodeState =  BehaviorState.失败;
            return NodeState;
        }
    }
    
   
    
   
    
}