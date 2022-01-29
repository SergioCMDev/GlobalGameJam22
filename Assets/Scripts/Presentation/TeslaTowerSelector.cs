using Application_;
using UnityEngine;

namespace Presentation
{
    public class TeslaTowerSelector : BuildingSelector
    {
        private void Awake()
        {
            BuildingType = BuildingType.Tesla;
        }

        protected override void BuildingSelected()
        {
            Debug.Log("F");
        }
        public override void MakeSound()
        {
            throw new System.NotImplementedException();
        }
    }
}