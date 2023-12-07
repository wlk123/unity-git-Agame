using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviorTree
{
    public class BTSetting : SerializedScriptableObject
    {
        public int TreeID;

        public static BTSetting GetSetting()
        {
            return Resources.Load<BTSetting>("BTSetting");
        }

#if UNITY_EDITOR
        public IGetBt GetTree() => UnityEditor.EditorUtility.InstanceIDToObject(TreeID) as IGetBt;
        public void SetRoot(BTNodeBase rootNode) => GetTree().SetRoot(rootNode);
#endif
    }
}