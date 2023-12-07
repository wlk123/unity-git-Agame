using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviorTree
{
    public class InspectorViewData : SerializedScriptableObject
    {
        
        public HashSet<BTNodeBase> DataView=new HashSet<BTNodeBase>();
    }

}
