using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.Popups
{
    [CreateAssetMenu(fileName = "PopupList", menuName = "Popup/PopupList")]
    public class PopupList : ScriptableObject
    {
        public List<PopupGenerator.PopupGetter> PopupListEditor;

        public GameObject GetPrefabByType(PopupGenerator.PopupType type)
        {
            return PopupListEditor.Single(x => x.PopupType == type).Prefab;
        }
    }
}