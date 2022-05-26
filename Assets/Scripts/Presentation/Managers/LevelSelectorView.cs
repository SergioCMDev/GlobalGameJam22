using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private void Start()
    {
        foreach (var VARIABLE in _buttons)
        {
            VARIABLE.button.onClick.AddListener(()=>StartScene(VARIABLE.sceneToLoad));
        }
    }

    private void StartScene(string variableSceneToLoad)
    {
        OnStartLevelSelected?.Invoke(variableSceneToLoad);
    }
}