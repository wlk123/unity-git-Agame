using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TreeEditor;
using UnityEngine;

public class TestBT : SerializedMonoBehaviour,IGetBt
{
    [OdinSerialize] 
    private BehaviorTreeData TreeData=new BehaviorTreeData();
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TreeData.rootNode?.Tick();
    }

#if UNITY_EDITOR
    
    [Button]
    public void OpenView()
    {
        BTSetting.GetSetting().TreeID = GetInstanceID();
        UnityEditor.EditorApplication.ExecuteMenuItem("Tools/BehaviourTreeWindows");
    }

    [Button]
    public void InitGuid()
    {
        if ( TreeData.rootNode != null)
        {
             SetNodeGuid(TreeData.rootNode);
             Debug.Log("开始初始化");
        }
           
    }
   
    
#endif
    public void SetNodeGuid(BTNodeBase Node)
    {
        Debug.Log(Node.GetType().ToString()+"开始"+Node.Guid);
        if (string.IsNullOrEmpty(Node.Guid))
        {
            Node.SetGuid();
            Debug.Log(Node.GetType().ToString()+"结束"+Node.Guid);
        }
        switch (Node)
        {
            case BtComposite composite:
                composite.ChildNodes.ForEach(n=>
                {
                    SetNodeGuid(n);
                });
                break;
            case BtPrecondition precondition:
                SetNodeGuid(precondition.ChildNode);
                break;
            
        }
    }

    public BehaviorTreeData GetTree() => TreeData;
    public void SetRoot(BTNodeBase rootNode) => this.TreeData.rootNode=rootNode;

}

public interface IGetBt
{
    BehaviorTreeData GetTree();
    void SetRoot(BTNodeBase rootNode);
}
