using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomCircuit : MonoBehaviour {

    public GameObject node;
    public GameObject resistor;
    public GameObject wire;

    public static Dictionary<GameObject, List<GameObject>> connectedComponents;

    void Start()
    {
        connectedComponents = new Dictionary<GameObject, List<GameObject>>();

        var circuits = new DirectoryInfo("Circuits").GetFiles("*.txt");
        var index = Random.Range(0, circuits.Length);
        DrawCircuit(circuits[index]);

        StartCoroutine("SetUpCircuit");
    }

    void DrawCircuit(FileInfo file)
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

            if(type.Equals("Resistor"))
            {
                var r = Instantiate(resistor, position, Quaternion.identity);
                r.transform.localScale = scale;
                r.transform.localRotation = rotation;
            }
            else if(type.Equals("Node"))
            {
                var n = Instantiate(node, position, Quaternion.identity);
                n.transform.localScale = scale;
                n.transform.localRotation = rotation;

                if (position.x == -7 && position.y == 2 && position.z == 0)
                {
                    n.tag = "StartingNode";
                }
                else if (position.x == -7 && position.y == -2 && position.z == 0)
                {
                    n.tag = "EndingNode";
                }

            }
            else if(type.Equals("Wire"))
            {
                var w = Instantiate(wire, position, Quaternion.identity);
                w.transform.localScale = scale;
                w.transform.localRotation = rotation;
            }

        }
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

    void DisableColliders()
    {
        Debug.Log(connectedComponents.Count);
        foreach (var key in connectedComponents.Keys)
        {
            if (key.gameObject.tag != "Resistor")
            {
                key.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                foreach (var child in key.GetComponentsInChildren<BoxCollider2D>())
                {
                    child.enabled = false;
                }
            }
        }
        
    }

    IEnumerator SetUpCircuit()
    {
        var startingComp = GameObject.FindGameObjectWithTag("StartingNode");
        yield return new WaitForSeconds(0.1f);
        DisableColliders();
        var ch = new CircuitHandler(connectedComponents);
        ch.StartSetUp();
    }


}
