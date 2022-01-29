using System;
using Application_;
using Application_.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public abstract class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private SfxSoundName _soundWhenClicked;
        [SerializeField] private PlaySFXEvent _playSfxEvent;
        public event Action<BuildingSelector> OnBuildingSelected;

        public BuildingType BuildingType { get; protected set; }

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            MakeSound();
            OnBuildingSelected.Invoke(this);
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