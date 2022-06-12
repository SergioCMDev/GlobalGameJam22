using System;
using UnityEngine;

namespace Presentation.Interfaces
{
    public interface ICloseablePopup
    {
        //To close Popup
        public Action<GameObject> HasToClosePopup { get; set; }
        //When popup has been closed
        public Action PopupHasBeenClosed { get; set; }
    }
}