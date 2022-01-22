using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour, ITouchable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SayHello()
    {
        Debug.Log($"Hello {gameObject}");
    }
}