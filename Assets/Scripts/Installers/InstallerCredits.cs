using App.SceneManagement;
using Services.Popups;
using Services.ScenesChanger;
using UnityEngine;
using Utils;

namespace Installers
{
    public class InstallerCredits : MonoBehaviour
    {
        [SerializeField] private GameObject _sceneChangerPrefab;

        private GameObject _sceneChangerInstance;

        private void Awake()
        {
            _sceneChangerInstance = Instantiate(_sceneChangerPrefab);
            ServiceLocator.Instance.RegisterService<SceneChangerService>(_sceneChangerInstance.GetComponent<SceneChangerService>());
        }
    }
}