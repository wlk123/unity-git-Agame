using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.View
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView,UxmlTraits>{}
        public SplitView()
        {
            
        }
    }

}
