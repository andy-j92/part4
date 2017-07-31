using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour {

    public static List<GameObject> circuitComponents;
    public GameObject wire;
    public GameObject node;
    public float multiplier;

    private GameObject newWire;
    private float scale;

    private Vector2 componentPos1;
    private Vector2 componentPos2;
    public static GameObject connector1;
    public static GameObject connector2;
    

	// Use this for initialization
	void Start () {
        circuitComponents = new List<GameObject>();

        Vector3 startNodePos = new Vector3(-7, 3, 0);
        Vector3 endNodePos = new Vector3(-7, -2, 0);

        var startNode = Instantiate(node, startNodePos, Quaternion.identity);
        startNode.tag = "StartingNode";
        circuitComponents.Add(startNode);

        var endNode = Instantiate(node, endNodePos, Quaternion.identity);
        endNode.tag = "EndingNode";
        circuitComponents.Add(endNode);
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
        //var wirePos = new Vector3(pos1.x, pos1.y, 1);
        var wirePos = (connector1.transform.position + connector2.transform.position)/2;
        var parentTag1 = connector1.transform.parent.gameObject.tag;
        var parentTag2 = connector2.transform.parent.gameObject.tag;
        var pos2 = connector2.transform.position;
        var xDiff = Mathf.Round(pos1.x - pos2.x);
        var yDiff = Mathf.Round(pos1.y - pos2.y);

        //if (isNode(parentTag1))
        //    wirePos -= connector1.transform.localPosition;
        
        if(xDiff < 0)
        {
            newWire = Instantiate(wire, wirePos, Quaternion.identity);
            scale = isNode(parentTag1, parentTag2) ? Mathf.Abs(pos1.x - pos2.x) * multiplier + 0.2f : Mathf.Abs(pos1.x - pos2.x) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);

        }
        else if(xDiff > 0)
        {
            newWire = Instantiate(wire, wirePos, Quaternion.Euler(0, 0, 180f));
            scale = isNode(parentTag1, parentTag2) ? Mathf.Abs(pos1.x - pos2.x) * multiplier + 0.2f : Mathf.Abs(pos1.x - pos2.x) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }
        else if(yDiff < 0)
        {
            newWire = Instantiate(wire, wirePos, Quaternion.Euler(0, 0, 90f));
            scale = isNode(parentTag1, parentTag2) ? Mathf.Abs(pos1.y - pos2.y) * multiplier + 0.2f : Mathf.Abs(pos1.y - pos2.y) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }
        else if (yDiff > 0)
        {
            newWire = Instantiate(wire, wirePos, Quaternion.Euler(0, 0, 270f));
            scale = isNode(parentTag1, parentTag2) ? Mathf.Abs(pos1.y - pos2.y) * multiplier + 0.2f : Mathf.Abs(pos1.y - pos2.y) * multiplier;
            newWire.transform.localScale = new Vector3(scale, 1, 1);
        }

        circuitComponents.Add(newWire);
        ResetConnectors(connector1, connector2);

    }

    bool isNode(string parentTag1, string parentTag2)
    {
        if (parentTag1 == "Node" || parentTag1 == "StartingNode" || parentTag1 == "EndingNode" ||
            parentTag2 == "Node" || parentTag2 == "StartingNode" || parentTag2 == "EndingNode")
            return true;
        return false;
    }

    bool isNode(string parentTag)
    {
        if (parentTag == "Node" || parentTag == "StartingNode" || parentTag == "EndingNode")
            return true;
        return false;
    }

    void ResetConnectors(GameObject connector1, GameObject connector2)
    {
        try
        {
            connector1.GetComponent<SpriteRenderer>().color = Color.white;
            connector2.GetComponent<SpriteRenderer>().color = Color.white;

            connector1.GetComponent<SpriteRenderer>().enabled = false;
            connector2.GetComponent<SpriteRenderer>().enabled = false;
        }
        catch (System.Exception)
        {
            
        }
    }
}
