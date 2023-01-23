using App.Events;
using UnityEngine;

namespace Services.ConstantsManager
{
    [CreateAssetMenu(fileName = "ConstantsManagerService",
        menuName = "Loadable/Services/ConstantsManagerService")]
    public class ConstantsManagerService : LoadableComponent
    {
        public Constants constants;

        public Constants Constants => constants;
        public override void Execute()
        {
            Debug.Log($"Execute ConstantsManagerService");

        }
    }
}
