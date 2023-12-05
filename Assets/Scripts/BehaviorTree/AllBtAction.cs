using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviorTree
{
    public class AllBtAction 
    {
        //所有行为节点逻辑写这
    }

    
    public class SetObjectActive : BtActionNode
    {
        [LabelText("是否启用"),SerializeField,FoldoutGroup("@NodeName")]
        private bool _isActive;
        [LabelText("启用对象"),SerializeField,FoldoutGroup("@NodeName")]
        private GameObject _gameObject;
        public new static string NodeEditorName="（节点）设置显示";
       
        public override BehaviorState Tick()
        {
            _gameObject.SetActive(_isActive);
            Debug.Log(message: NodeName + (_isActive?"节点 启用了":"节点 禁用了"));
            return BehaviorState.成功;
        }
    }
}