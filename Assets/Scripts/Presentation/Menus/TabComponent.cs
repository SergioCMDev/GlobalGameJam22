using System;

namespace Presentation.Menus
{
    [Serializable]
    public struct TabComponent
    {
        public ButtonView ButtonTabView;
        public PanelTabView PanelTabView;
        public string TabName;
    }
}