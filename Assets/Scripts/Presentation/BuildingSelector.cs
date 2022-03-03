using System;
using App;
using App.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SfxSoundName _soundWhenClicked;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        [SerializeField] private BuildingType _buildingType;
        public event Action<BuildingType> OnBuildingSelected;

        public BuildingType BuildingType { get => _buildingType; protected set => _buildingType = value; }

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            MakeSound();
            OnBuildingSelected.Invoke(BuildingType);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }
        
        private void MakeSound()
        {
            _playSfxEvent.soundName = _soundWhenClicked;
            _playSfxEvent.Fire();
        }
    }
}