using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomCircuit : MonoBehaviour {

    public GameObject node;
    public GameObject resistor;
    public GameObject wire;

    void Start()
    {
        var circuits = new DirectoryInfo("Circuits").GetFiles("*.txt");
        var index = Random.Range(0, circuits.Length);
        TransformHandler.SetWireObject(wire);
        StartCoroutine(DrawCircuit(circuits[index]));
    }

    IEnumerator DrawCircuit(FileInfo file)
    {
        StreamReader reader = file.OpenText();

        string text;

        while((text = reader.ReadLine()) != null)
        {
            if (text == "")
            {
                break;
            }

            var info = text.Split(' ');
            var type = info[0];
            Vector3 position = GetPosition(info);
            Vector3 scale = GetScale(info);
            Quaternion rotation = GetRotation(info);

            GameObject component = null;
            if(type.Equals("Resistor"))
            {
                component = Instantiate(resistor, position, Quaternion.identity);
                component.transform.localScale = scale;
                component.transform.localRotation = rotation;
            }
            else if(type.Equals("Node"))
            {
                component = Instantiate(node, position, Quaternion.identity);
                component.transform.localScale = scale;
                component.transform.localRotation = rotation;

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
                component.transform.localScale = scale;
                component.transform.localRotation = rotation;
            }
            CircuitHandler.components.Add(component);

        }
        reader.Close();
        yield return new WaitForSeconds(0.1f);
        new CircuitHandler().StartSetUp();
        DisableScripts();

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

    public void LoadNewCircuit()
    {
        foreach (var component in CircuitHandler.connectedComponents.Keys)
        {
            Destroy(component);
        }
        CircuitHandler.connectedComponents = new Dictionary<GameObject, List<GameObject>>();
        CircuitHandler.components = new List<GameObject>();
        Start();

    }


}
