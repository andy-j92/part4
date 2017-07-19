using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitHandler : MonoBehaviour {

    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll);
    }
}
