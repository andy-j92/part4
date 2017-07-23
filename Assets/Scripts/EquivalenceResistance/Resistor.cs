using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var value = gameObject.GetComponent<TextMesh>();
        value.text = Random.Range(5, 25).ToString();
	}
}
