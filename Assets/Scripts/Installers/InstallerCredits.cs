using Application_.SceneManagement;
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
            ServiceLocator.Instance.RegisterService<SceneChanger>(_sceneChangerInstance.GetComponent<SceneChanger>());
        }
    }
}