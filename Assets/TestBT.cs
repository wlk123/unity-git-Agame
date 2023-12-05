using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class TestBT : SerializedMonoBehaviour
{
    [OdinSerialize] public BTNodeBase rootNode;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rootNode?.Tick();
    }
}
