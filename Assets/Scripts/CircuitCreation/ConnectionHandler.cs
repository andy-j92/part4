﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour {

    public static List<GameObject> circuitComponents;
    public GameObject wire;
    private GameObject newWire;
    private float multiplier;
    private float scale;

    private Vector2 componentPos1;
    private Vector2 componentPos2;
    public static GameObject connector1;
    public static GameObject connector2;
    

	// Use this for initialization
	void Start () {
        multiplier = 1.26f;
        circuitComponents = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(connector1 != null && connector2 != null)
        {
            ConnectComponents(connector1, connector2);

            connector1 = null;
            connector2 = null;
        }
	}

    void ConnectComponents(GameObject connector1, GameObject connector2)
    {
        var pos1 = connector1.transform.position;
        var pos2 = connector2.transform.position;
        var xDiff = Mathf.Round(pos1.x - pos2.x);
        var yDiff = Mathf.Round(pos1.y - pos2.y);

        if(xDiff < 0)
        {
            newWire = Instantiate(wire, pos1, Quaternion.identity);
            scale = Mathf.Abs(pos1.x - pos2.x) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);

        }
        else if(xDiff > 0)
        {
            Debug.Log(pos1.x);
            Debug.Log(pos2.x);
            newWire = Instantiate(wire, pos1, Quaternion.Euler(0, 0, 180f));
            scale = Mathf.Abs(pos1.x - pos2.x) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }
        else if(yDiff < 0)
        {
            newWire = Instantiate(wire, pos1, Quaternion.Euler(0, 0, 90f));
            scale = Mathf.Abs(pos1.y - pos2.y) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }
        else if (yDiff > 0)
        {
            newWire = Instantiate(wire, pos1, Quaternion.Euler(0, 0, 270f));
            scale = Mathf.Abs(pos1.y - pos2.y) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }


        ConnectionHandler.circuitComponents.Add(newWire);
        connector1.GetComponent<SpriteRenderer>().color = Color.white;
        connector2.GetComponent<SpriteRenderer>().color = Color.white;

    }
}