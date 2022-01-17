using System;

namespace Presentation.Views
{
    [Serializable]
    public struct TabComponent
    {
        public ButtonView ButtonTabView;
        public PanelTabView PanelTabView;
        public string TabName;
    }
}