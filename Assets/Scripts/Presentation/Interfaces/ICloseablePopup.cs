using System;
using UnityEngine;

namespace Presentation.Interfaces
{
    public interface ICloseablePopup
    {
        public Action<GameObject> OnClosePopup { get; set; }
    }
}