using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("FC");
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Vector3Int gridPosition = mapManager.GetGridPosition(Input.mousePosition);
            Debug.Log("FD");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FD");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}