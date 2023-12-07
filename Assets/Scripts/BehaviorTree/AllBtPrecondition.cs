using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviorTree
{
    public class AllBtPrecondition
    {
        //所有条件节点（装饰节点）逻辑写这
    }

    public class Delay : BtPrecondition
    {
        [LabelText("延时"),SerializeField,FoldoutGroup("@NodeName")]
        public float timer;
        
        private float _currentTimer;
        
        public new static string NodeEditorName="（节点）延迟";

        public override BehaviorState Tick()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= timer)
            {
                _currentTimer = 0f;
                ChildNode.Tick();
                NodeState = BehaviorState.成功;
                return NodeState;
            }

            NodeState = BehaviorState.执行中;
            return NodeState;
        }
    }
    
    public class So : BtPrecondition
    {
        [LabelText("是否活动"),FoldoutGroup("@NodeName"),SerializeField]
        private  bool _isActive;
        public new static string NodeEditorName="（节点）定选";
        
        public override BehaviorState Tick()
        {
            if (_isActive)
            {
                 NodeState =ChildNode.Tick();
                 return NodeState;
            }
            else
            {
                NodeState = BehaviorState.失败;
                return NodeState;
            }
            
        }
    }
    
    
    
}