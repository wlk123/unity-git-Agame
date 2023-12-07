using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.View
{
    public class InspectorView : VisualElement
    {
        public IMGUIContainer inspectorBar;
        public InspectorViewData ViewData;
        
        public new class UxmlFactory : UxmlFactory<InspectorView,UxmlTraits>{}
        
        public InspectorView()
        {
            inspectorBar =new IMGUIContainer(){ name="检查台"};
            inspectorBar.style.flexGrow = 1;
            CreateInspectorView();
            Add(inspectorBar);
        }

        private async void CreateInspectorView()
        {
            ViewData = Resources.Load<InspectorViewData>("InspectorViewData");
            await Task.Delay(100);
            var odinEditor = UnityEditor.Editor.CreateEditor(ViewData);
            inspectorBar.onGUIHandler += () =>
            {
                odinEditor.OnInspectorGUI();
            };
        }

        public void UpdateViewDate()
        {
            HashSet<BTNodeBase> nodes = Enumerable.ToHashSet(BehaviourTreeWindows.windowsRoot.treeView.selection.OfType<NodeView>()
                    .Select(n=>n.NodeData));
            
            ViewData.DataView.Clear();
            foreach (var node in nodes)
            {
                ViewData.DataView.Add(node);
            }
        }
        
    } 
}


