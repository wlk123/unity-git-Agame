using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.View
{
    /// <summary>
    /// 这个整体视图的相关方法定义在树视图中
    /// </summary>
    public class TreeView : GraphView
    {
        public Dictionary<string,NodeView> NodeViews=new Dictionary<string, NodeView>();
        
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
            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange gvc)
        {
            if (gvc.edgesToCreate != null)
            {
                gvc.edgesToCreate.ForEach(edge =>
                {
                    edge.LinkLineAddData();
                });
            }

            if (gvc.elementsToRemove!=null)
            {
                gvc.elementsToRemove.ForEach(ele =>
                {
                    if (ele is Edge edge)
                    {
                        edge.UnLinkLineDelectData();
                    }
                });
            }

            return gvc;
        }


        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            //在视图右键创建item
            //evt.menu.AppendAction("创建节点",CreateNode);
            
        }

        private void CreateNode(Type type,Vector2 pos)
        {
              BTNodeBase nodeDate = Activator.CreateInstance(type) as BTNodeBase;
              nodeDate.Guid = System.Guid.NewGuid().ToString();
              
              NodeView node=new NodeView(nodeDate);
              node.title = type.GetField("NodeEditorName").GetValue(null).ToString();
              node.SetPosition(new Rect(pos,Vector2.one));
              NodeViews.Add(nodeDate.Guid,node);
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
        
        //覆写GetCompatiblePorts 定义链接规则
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            //筛选可链接端口：条件1端口类型不能相同，例如input端不可链接input端口；条件2 链接目标端口的节点不能是自己
            return ports.Where(endPorts=>
                    endPorts.direction != startAnchor.direction && endPorts.node != startAnchor.node)
                .ToList();
            
        }

        private bool OnMenuSelectEntry(SearchTreeEntry searchtreeentry, SearchWindowContext context)
        {
            //创建item定位
            var windowRoot = BehaviourTreeWindows.windowsRoot.rootVisualElement;
            var windowMousePositon = windowRoot.ChangeCoordinatesTo(windowRoot.parent, 
                context.screenMousePosition - BehaviourTreeWindows.windowsRoot.position.position);
            var graphMousePosition = contentViewContainer.WorldToLocal(windowMousePositon);

            CreateNode((Type) searchtreeentry.userData, graphMousePosition);
            return true;
        }
    } 
}


