using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetButtonHandler : MonoBehaviour {

	public void DeleteAllComponents()
    {
        var components = ConnectionHandler.circuitComponents;
        foreach (GameObject component in components)
        {
            Destroy(component);
        }
    }
}
