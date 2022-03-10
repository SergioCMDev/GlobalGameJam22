using System;
using UnityEngine;

public interface ICloseablePopup
{
    public Action<GameObject> OnClosePopup { get; set; }
}