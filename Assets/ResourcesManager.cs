using Application_;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    //TODO Resource Model to store quantities of each resource
    
    // Start is called before the first frame update
    void Start()
    {
    }


//From Event
    public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
    {
        Debug.Log($"Got resource {playerGetResourceEvent.Type} Q {playerGetResourceEvent.Quantity}");
        //IF player Gets resource check if any building can be updated based on a scriptable object with level-resource quantities and type of upgrade
    }
}