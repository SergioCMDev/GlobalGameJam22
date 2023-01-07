using System.Collections.Generic;
using System.Linq;
using App.Models;
using Domain;
using Services.Utils;
using UnityEngine;
using Utils;

namespace Services
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> loadablesSO;
        [SerializeField] private CoroutineExecutioner coroutineExecutioner;
        private CoroutineExecutioner coroutineExecutionerInstance;
        private Dictionary<ILoadable, bool> loadableStatus;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            loadableStatus = new Dictionary<ILoadable, bool>();
            InstantiateModels();
            RegisterDomainServices();
            ExecuteComponents(loadablesSO);
            coroutineExecutionerInstance = Instantiate(coroutineExecutioner);
            ServiceLocator.Instance.RegisterService(coroutineExecutionerInstance);
        }

        private void RegisterDomainServices()
        {
            ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
        }

        private void InstantiateModels()
        {
            ServiceLocator.Instance.RegisterModel<IPlayerModel>(new PlayerModel());
            ServiceLocator.Instance.RegisterModel<ISceneModel>(new SceneModel());

            ServiceLocator.Instance.RegisterModel<IResourcesModel>(new ResourcesModel());
            ServiceLocator.Instance.RegisterModel<IBuildingStatusModel>(new BuildingStatusModel());
        }

        private void ExecuteComponents(List<ScriptableObject> list)
        {
            List<ILoadable> loadables = new();
            foreach (var loadable in list)
            {
                var component = (ILoadable)loadable;
                if (component.Dependencies.Contains(loadable))
                {
                    RemoveInnerSelfComponent(component, loadable);
                }

                loadables.Add(component);
            }

            foreach (var loadable in loadables)
            {
                if (loadableStatus.ContainsKey(loadable) && loadableStatus[loadable]) continue;
                if (loadable.Dependencies is { Count: > 0 })
                {
                    ExecuteComponents(loadable.Dependencies);
                }

                loadable.Execute();

                loadableStatus.Add(loadable, true);
                var objectType = loadable.GetType();
                if (objectType.GetInterfaces().Any(x => x != typeof(ILoadable)))
                {
                    var interfaz = objectType.GetInterfaces().Single(x => x != typeof(ILoadable));
                    ServiceLocator.Instance.RegisterService(interfaz, loadable);
                    continue;
                }

                ServiceLocator.Instance.RegisterService(objectType, loadable);
            }
        }

        private static void RemoveInnerSelfComponent(ILoadable component, ScriptableObject loadable)
        {
            var itselfComponent = component.Dependencies.FindAll(x => x == loadable);
            foreach (var sameComponent in itselfComponent)
            {
                component.Dependencies.Remove(sameComponent);
            }
        }
    }
}