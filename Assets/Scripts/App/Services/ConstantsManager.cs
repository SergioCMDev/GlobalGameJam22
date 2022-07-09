using App.Events;
using UnityEngine;

namespace App.Services
{
    public class ConstantsManager : MonoBehaviour
    {
        [SerializeField] private Constants constants;

        public Constants Constants => constants;
    }
}