using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BehaviorTree;
using PlasticGui.Gluon.WorkspaceWindow.Views.IncomingChanges;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Assembly = System.Reflection.Assembly;

namespace Editor.View
{
    public class BehaviourTreeWindows : EditorWindow
    {
        //利用单例来定位
        public static BehaviourTreeWindows windowsRoot;

        public TreeView treeView;
        public InspectorView inspectorView;
        
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/BehaviourTreeWindows")]
        public static void ShowExample()
        {
            BehaviourTreeWindows wnd = GetWindow<BehaviourTreeWindows>();
            wnd.titleContent = new GUIContent("BehaviourTreeWindows");
            
        }

        private void OnDestroy() => Save();

        public void CreateGUI()
        {
            //根据ID去拿取行为树数据，使用接口传输
            int id= BTSetting.GetSetting().TreeID;
            var iGetBt=BTSetting.GetSetting().GetTree();
            
            windowsRoot = this;
            VisualElement root = rootVisualElement;
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/View/BehaviourTreeWindows.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/View/BehaviourTreeWindows.uss");
            visualTree.CloneTree(root);
            
            //根据数据动态加载树
            treeView = root.Q<TreeView>();
            inspectorView = root.Q<InspectorView>();
            if (iGetBt==null)return;
            if (iGetBt.GetTree()?.rootNode==null)return;
            CreatRoot(iGetBt.GetTree().rootNode);
            //调用所有的节点连接自己的子集
            treeView.nodes.OfType<NodeView>().ForEach(n => n.LinkLine());
            
            var tree= iGetBt.GetTree().ViewTransform;
            treeView.viewTransform.position=tree.position;
            treeView.viewTransform.scale=tree.scale;

        }

        #region 快捷键参数

        public  void Save()
        { 
           var tree= BTSetting.GetSetting().GetTree().GetTree().ViewTransform;
          // tree=new GraphViewTransform();
           tree.position=treeView.viewTransform.position;
           tree.scale=treeView.viewTransform.scale;
            
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        #endregion


        private void OnInspectorUpdate()
        {
            treeView.nodes.OfType<NodeView>().ForEach(n => n.UpdateData());
        }

        /// <summary>
        /// 通过创建根节点创建树
        /// </summary>
        /// <param name="rootNode"></param>
        public void CreatRoot(BTNodeBase rootNode)
        {
            if (rootNode==null)return;
            NodeView nodeView = new NodeView(rootNode);
            nodeView.SetPosition(new Rect(rootNode.Position, Vector2.one));
            treeView.AddElement(nodeView);
            treeView.NodeViews.Add(rootNode.Guid,nodeView);
            //treeView.RootNode = nodeView;
            switch (rootNode)
            {
                case BtComposite composite:
                    composite.ChildNodes.ForEach(CreatChild);
                    break;
                case BtPrecondition precondition:
                    CreatChild(precondition.ChildNode);
                    break;
            }
        }
        
        public void CreatChild(BTNodeBase nodeData)
        {
            if (nodeData==null)return;
            NodeView nodeView = new NodeView(nodeData);
            nodeView.SetPosition(new Rect(nodeData.Position, Vector2.one));
            treeView.AddElement(nodeView);
            treeView.NodeViews.Add(nodeData.Guid,nodeView);
            
            switch (nodeData)
            {
                case BtComposite composite:
                    //遍历调用
                    composite.ChildNodes.ForEach(CreatChild);
                    break;
                case BtPrecondition precondition:
                    CreatChild(precondition.ChildNode);
                    break;
            }

        }
        
    }

    public class RightlickMenu : ScriptableObject, ISearchWindowProvider
    {
        //点击层级创建的事件监测委托
        public delegate bool SelectEntryDelegate(SearchTreeEntry searchTreeEntry, SearchWindowContext context);
        public SelectEntryDelegate OnSelectEntryHandler;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(item: new SearchTreeGroupEntry(new GUIContent(text: "创建 节点")));
            entries = AddNodeType<BtComposite>(entries,"组合节点");
            entries = AddNodeType<BtPrecondition>(entries,"条件节点");
            entries = AddNodeType<BtActionNode>(entries,"行为节点");
            return entries;
        }
        
        //狠狠の用反射添加对应的节点到菜单目录
        private List<SearchTreeEntry> AddNodeType<T>(List<SearchTreeEntry> entries,string pathName)
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent(pathName)){level=1});
            List<Type> rootNodeTypes = GetDerivedclasses(typeof(T));
            foreach (var rootType in rootNodeTypes)
            {
                string MenuName = "空";
                FieldInfo field =   rootType.GetField("NodeEditorName", BindingFlags.Static | BindingFlags.Public);
                if (field!=null&& field.FieldType == typeof(string))
                {
                    
                    MenuName=(string)field.GetValue(null);
                }
                entries.Add(new SearchTreeEntry(new GUIContent(MenuName)){level=2,userData = rootType});
            }

            return entries;

        }

        
        

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
            {
                return false;
            }

            return OnSelectEntryHandler(SearchTreeEntry, context);
        }

        public static List<Type> GetDerivedclasses( Type type)
        {
            List<Type> derivedclasses = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    //是类，不是抽象类，类型符合某集合(这里主要是把行为树的节点基类传进去筛选)
                    if (t.IsClass&&!t.IsAbstract&&type.IsAssignableFrom(t))
                    {
                        derivedclasses.Add(t);
                    }
                }
            }

            return derivedclasses;
        }
        
    }
}