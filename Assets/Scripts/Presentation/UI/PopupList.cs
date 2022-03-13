using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Presentation
{
    [CreateAssetMenu(fileName = "PopupList",
        menuName = "Popup/PopupList")]
    public class PopupList : ScriptableObject
    {
        public List<PopupGetter> PopupListEditor;

        public GameObject GetPrefabByType(PopupType type)
        {
            return PopupListEditor.Single(x => x.PopupType == type).Prefab;
        }
    }
}