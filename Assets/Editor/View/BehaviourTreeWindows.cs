using System;
using System.Collections.Generic;
using System.Reflection;
using BehaviorTree;
using PlasticGui.Gluon.WorkspaceWindow.Views.IncomingChanges;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Assembly = System.Reflection.Assembly;

namespace Editor.View
{
    public class BehaviourTreeWindows : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem("Tools/BehaviourTreeWindows")]
        public static void ShowExample()
        {
            BehaviourTreeWindows wnd = GetWindow<BehaviourTreeWindows>();
            wnd.titleContent = new GUIContent("BehaviourTreeWindows");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/View/BehaviourTreeWindows.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/View/BehaviourTreeWindows.uss");
            visualTree.CloneTree(root);
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