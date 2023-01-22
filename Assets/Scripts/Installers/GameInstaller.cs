using App.Models;
using Domain;
using Presentation.InputPlayer;
using Presentation.Managers;
using Services.Popups;
using UnityEngine;
using Utils;

namespace Installers
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _readInputPlayerPrefab;
        [SerializeField] private GameObject _soundManagerPrefab;



        private GameObject _readInputPlayerInstance,
            _soundManagerInstance
            ;

        private bool _initialized;



        private void InitCommonLogic()
        {
            _readInputPlayerInstance = Instantiate(_readInputPlayerPrefab);
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            // _timeManagerInstance = Instantiate(_timeManagerPrefab);

            ServiceLocator.Instance.RegisterService(
                _soundManagerInstance.GetComponent<SoundPlayer>());
            ServiceLocator.Instance.RegisterService(
                _readInputPlayerInstance.GetComponent<ReadInputPlayer>());




            //TODO HACERLO PARA N OBJETOS NO SOLO 1

            DontDestroyOnLoad(_readInputPlayerInstance);
            DontDestroyOnLoad(_soundManagerInstance);
        }
    }
}