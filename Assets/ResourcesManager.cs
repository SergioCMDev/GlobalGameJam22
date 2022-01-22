using Application_;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }


//From Event
    public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
    {
        Debug.Log($"Got resource {playerGetResourceEvent.Type} Q {playerGetResourceEvent.Quantity}");
    }
}