using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

[Serializable]
public struct ButtonSelectableInfo
{
    public Button button;
    public string sceneToLoad;
}
public class LevelSelectorView : MonoBehaviour
{
    [SerializeField] private List<ButtonSelectableInfo> _buttons;
    public event Action<string> OnStartLevelSelected;

    private void StartScene(string variableSceneToLoad)
    {
        OnStartLevelSelected?.Invoke(variableSceneToLoad);
    }

    public void Init(int lastCompletedLevel)
    {
        foreach (var VARIABLE in _buttons)
        {
            VARIABLE.button.onClick.AddListener(()=>StartScene(VARIABLE.sceneToLoad));
            var id = Utilities.GetNumberOfLevelString(VARIABLE.sceneToLoad);

            if (id > lastCompletedLevel)
            {
                VARIABLE.button.interactable = false;
            }
        }
    }
}