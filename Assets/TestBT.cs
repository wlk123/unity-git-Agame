using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using BehaviorTree;


public class TestBT : SerializedMonoBehaviour,IGetBt
{
    [OdinSerialize] 
    public BTNodeBase rootNode;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rootNode?.Tick();
    }

#if UNITY_EDITOR
    
    [Button]
    public void OpenView()
    {
        BTSetting.GetSetting().TreeID = GetInstanceID();
        UnityEditor.EditorApplication.ExecuteMenuItem("Tools/BehaviourTreeWindows");
    }
    
#endif

    public BTNodeBase GetRoot() => rootNode;
    public void SetRoot(BTNodeBase rootNode) => this.rootNode=rootNode;

}

public interface IGetBt
{
    BTNodeBase GetRoot();
    void SetRoot(BTNodeBase rootNode);
}
