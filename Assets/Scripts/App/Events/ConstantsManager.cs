using App.Events;
using UnityEngine;

namespace Presentation.Managers
{
    public class ConstantsManager : MonoBehaviour
    {
        [SerializeField] private Constants constants;

        public Constants Constants => constants;
    }
}