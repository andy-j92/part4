﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomCircuit : MonoBehaviour {

    public GameObject node;
    public GameObject resistor;
    public GameObject wire;
    public GameObject action;

    private int currentCircuitIndex = 0;
    private FileInfo[] circuits;
    private List<GameObject> resistors;
    public static string filename;

    void Start()
    {
        wire.GetComponent<BoxCollider2D>().isTrigger = true;
        circuits = new DirectoryInfo("Circuits").GetFiles("*.txt");
        TransformHandler.SetWireObject(wire);
        TransformHandler.SetActionObject(action);
        StartCoroutine(DrawCircuit(circuits[Random.Range(0, circuits.Length)]));
    }

    IEnumerator DrawCircuit(FileInfo file)
    {
        StreamReader reader = file.OpenText();
        filename = file.Name;
        resistors = new List<GameObject>();
        string text;
        int resistorCount = 1;
        while((text = reader.ReadLine()) != "")
        {
            var info = text.Split(' ');
            var type = info[0];
            Vector3 position = GetPosition(info);
            Vector3 scale = GetScale(info);
            Quaternion rotation = GetRotation(info);

            GameObject component = null;
            if(type.Equals("Resistor"))
            {
                component = Instantiate(resistor, position, Quaternion.identity);
                component.tag = "Resistor" + resistorCount;
                resistorCount++;
                resistors.Add(component);
            }
            else if(type.Equals("Node"))
            {
                component = Instantiate(node, position, Quaternion.identity);

                if (position.x == -7 && position.y == 3 && position.z == 0)
                {
                    component.tag = "StartingNode";
                }
                else if (position.x == -7 && position.y == -2 && position.z == 0)
                {
                    component.tag = "EndingNode";
                }

            }
            else if(type.Equals("Wire"))
            {
                component = Instantiate(wire, position, Quaternion.identity);
            }

            component.transform.localScale = scale;
            component.transform.localRotation = rotation;

            CircuitHandler.components.Add(component);

        }
        reader.Close();
        
        yield return new WaitForSeconds(0.1f);
        new CircuitHandler().StartSetUp();
        DisableScripts();

        var equationFile = new DirectoryInfo("Equations").GetFiles(filename + ".txt");
        if (equationFile.Length != 0)
            Debug.Log(new Equation(resistorCount).Calculate(new DirectoryInfo("Equations").GetFiles(filename + ".txt")[0]));
    }

    Vector3 GetPosition(string[] info)
    {
        var xPos = float.Parse(info[1]);
        var yPos = float.Parse(info[2]);
        var zPos = float.Parse(info[3]);
        return new Vector3(xPos, yPos, zPos);
    }

    Vector3 GetScale(string[] info)
    {
        var xScale = float.Parse(info[4]);
        var yScale = float.Parse(info[5]);
        var zScale = float.Parse(info[6]);
        return new Vector3(xScale, yScale, zScale);
    }

    Quaternion GetRotation(string[] info)
    {
        var xRot = float.Parse(info[7]);
        var yRot = float.Parse(info[8]);
        var zRot = float.Parse(info[9]);
        var w = float.Parse(info[10]);
        return new Quaternion(xRot, yRot, zRot, w);
    }

    void DisableScripts()
    {
        var componentScripts = FindObjectsOfType<ComponentsScript>();
        var connectorScripts = FindObjectsOfType<ConnectorScript>();

        foreach (var script in componentScripts)
        {
            script.enabled = false;
        }

        foreach (var script in connectorScripts)
        {
            script.enabled = false;
        }
    }

    public void ResetCircuit()
    {
        LoadNewCircuit();
        StartCoroutine(DrawCircuit(circuits[currentCircuitIndex]));
    }

    public void PrevCircuit()
    {
        LoadNewCircuit();
        currentCircuitIndex -= 1;
        if (currentCircuitIndex < 0)
            currentCircuitIndex = circuits.Length-1;
        StartCoroutine(DrawCircuit(circuits[currentCircuitIndex]));
    }

    public void NextCircuit()
    {
        LoadNewCircuit();
        currentCircuitIndex += 1;
        if (currentCircuitIndex >= circuits.Length)
            currentCircuitIndex = 0;
        StartCoroutine(DrawCircuit(circuits[currentCircuitIndex]));
    }

    public void LoadNewCircuit()
    {
        var nodes = GameObject.FindGameObjectsWithTag("Node");
        var wires = GameObject.FindGameObjectsWithTag("Wire");

        foreach (var resistor in resistors)
        {
            Destroy(resistor);
        }

        foreach (var node in nodes)
        {
            Destroy(node);
        }
        Destroy(GameObject.FindGameObjectsWithTag("StartingNode")[0]);
        Destroy(GameObject.FindGameObjectsWithTag("EndingNode")[0]);
        foreach (var wire in wires)
        {
            Destroy(wire);
        }

        foreach (var action in TransformHandler.actions)
        {
            Destroy(action);
        }

        CircuitHandler.selected1 = null;
        CircuitHandler.selected2 = null;
        CircuitHandler.connectedComponents = new Dictionary<GameObject, List<GameObject>>();
        CircuitHandler.components = new List<GameObject>();
        CircuitHandler.wires = new List<Wire>();
    }


}
