using System;
using UnityEngine;

namespace Presentation
{
    public interface ICloseablePopup
    {
        public Action<GameObject> OnClosePopup { get; set; }
    }
}