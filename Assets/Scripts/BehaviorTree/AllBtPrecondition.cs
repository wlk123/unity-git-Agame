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
        public override BehaviorState Tick()
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= timer)
            {
                _currentTimer = 0f;
                ChildNode.Tick();
                return BehaviorState.成功;
            }

            return  BehaviorState.执行中;
        }
    }
    
    public class So : BtPrecondition
    {
        [LabelText("是否活动"),FoldoutGroup("@NodeName"),SerializeField]
        private  bool _isActive;

        public override BehaviorState Tick()
        {
            if (_isActive)
            {
                return ChildNode.Tick();
            }
            else
            {
                return BehaviorState.失败;
            }
            
        }
    }
    
    
    
}