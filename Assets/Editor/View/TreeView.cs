using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.View
{
    public class TreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<TreeView,UxmlTraits>{}

        public TreeView()
        {
            Insert( 0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            //添加样式
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/View/BehaviourTreeWindows.uss"));
            GraphViewMenu();
        }

      

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            //在视图右键创建item
            //evt.menu.AppendAction("创建节点",CreateNode);
            
        }

        private void CreateNode(DropdownMenuAction obj)
        {
              Node node=new Node();
              node.title = "节点1";
              this.AddElement(node);
        }
        private void GraphViewMenu()
        {
            //具体委托
            var menuWindowProvider = ScriptableObject.CreateInstance<RightlickMenu>();
            menuWindowProvider.OnSelectEntryHandler = OnMenuSelectEntry;

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
            };
        }

        private bool OnMenuSelectEntry(SearchTreeEntry searchtreeentry, SearchWindowContext context)
        {
            return true;
        }
    } 
}


