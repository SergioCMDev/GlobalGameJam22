using UnityEngine;

namespace Presentation
{
    public class TeslaTowerSelector : BuildingSelector
    {
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