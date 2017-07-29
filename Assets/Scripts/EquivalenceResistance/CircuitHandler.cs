using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitHandler : MonoBehaviour {

    private GameObject startingComp;
    private GameObject prevComp;
    private bool hasMultiple = false;
    private bool isAdded = false;
    public static List<DoubleEnded> componentOrder = new List<DoubleEnded>();
    public static List<GameObject> components = new List<GameObject>();

    public static Dictionary<GameObject, List<GameObject>> connectedComponents = new Dictionary<GameObject, List<GameObject>>();
    private Queue<GameObject> connectionQueue = new Queue<GameObject>();
    private List<GameObject> processedComponents = new List<GameObject>();


    public static GameObject selected1;
    public static GameObject selected2;

    public void StartSetUp()
    {
        DisableColliders();
        //var components = connectedComponents.Keys;
        //foreach (var component in components)
        //{
        //    Debug.Log(component.tag);
        //    if(component.tag == "Resistor" || component.tag == "Node" || component.tag == "StartingNode" || component.tag == "EndingNode")
        //        SetUpConnection(component);
        //}
        var startingComp = GameObject.FindGameObjectWithTag("StartingNode");
        connectionQueue.Enqueue(startingComp);
        while (connectionQueue.Count > 0)
        {
            var component = connectionQueue.Dequeue();
            if(!processedComponents.Contains(component))
            {
                SetUpConnection(component);
                processedComponents.Add(component);
            }
        }
        SetPreviousLink();
    }

    public void SetPreviousLink()
    {
        //retrieves next components and sets prev components to itself
        foreach (var deComponent in componentOrder)
        {
            foreach (var comp in deComponent.GetNextComponent())
            {
                var deComp = GetDoubledEndedObject(comp);
                if(deComp.GetNextComponent().Contains(deComponent.GetCurrentComponent()))
                {
                    if(deComp.GetPreviousComponent() == null)
                    {
                        List<GameObject> prevCompList = new List<GameObject>();
                        prevCompList.Add(deComponent.GetCurrentComponent());
                        deComp.SetPreviousComponent(prevCompList);
                    }
                    else
                    {
                        var prevCompList = deComp.GetPreviousComponent();
                        prevCompList.Add(deComponent.GetCurrentComponent());
                        deComp.SetPreviousComponent(prevCompList);
                    }

                    if (deComp.GetNextComponent().Contains(deComponent.GetCurrentComponent()))
                        deComp.GetNextComponent().Remove(deComponent.GetCurrentComponent());
                }
            }
        }
    }

    void SetUpConnection(GameObject component)
    {
        List<GameObject> nextComp = new List<GameObject>();
        DoubleEnded deComponent = new DoubleEnded(component);
        if (componentOrder.Contains(GetDoubledEndedObject(component)))
        {
            return;
        }
        List<GameObject> connectedComp = null;
        connectedComponents.TryGetValue(component, out connectedComp);
        if (connectedComp == null)
        {
            return;
        }
        else if(connectedComp != null)
        {
            foreach (var comp in connectedComp)
            {
                Transform parent;
                //get the connected component of the wire, thereby directly linked two components without saving wires
                if (comp.tag == "Wire")
                {
                    List<GameObject> wireConnectedComp = null;
                    connectedComponents.TryGetValue(comp, out wireConnectedComp);
                    if (wireConnectedComp != null)
                    {
                        foreach (var item in wireConnectedComp)
                        {
                            parent = item.transform.parent;
                            if (parent != null && !nextComp.Contains(parent.gameObject) && parent.gameObject != component)
                            {
                                nextComp.Add(parent.gameObject);
                                connectionQueue.Enqueue(parent.gameObject);
                            }
                        }
                    }
                }
                else if(comp.tag == "Connector" && !nextComp.Contains(comp.transform.parent.gameObject))
                {
                    parent = comp.transform.parent;
                    nextComp.Add(parent.gameObject);
                    connectionQueue.Enqueue(parent.gameObject);
                }
                else
                {
                    nextComp.Add(comp);
                    connectionQueue.Enqueue(comp);
                }
            }
        }

        deComponent.SetNextComponent(nextComp);
        componentOrder.Add(deComponent);
    }

    public void SerialTransform()
    {
        if(selected1 != null && selected2 != null)
        {
            var deComponent1 = GetDoubledEndedObject(selected1);
            var deComponent2 = GetDoubledEndedObject(selected2);

            Debug.Log(CheckSeries(deComponent1, deComponent2));

        }
    }

    public static DoubleEnded GetDoubledEndedObject(GameObject component)
    {
        foreach (var item in componentOrder)
        {
            if (item.GetCurrentComponent() == component)
            {
                return item;
            }
        }
        return null;
    }

    bool CheckSeries(DoubleEnded component1, DoubleEnded component2)
    {
        var nextComp1 = component1.GetNextComponent();
        var nextComp2 = component2.GetNextComponent();

        if (nextComp1.Count > 1 || nextComp2.Count > 1 || nextComp1 == null || nextComp2 == null)
            return false;
        //else if (component1.GetPreviousComponent().tag == component2.GetCurrentComponent().tag || component2.GetPreviousComponent().tag == component1.GetCurrentComponent().tag)
        //{
        //    return true;
        //}
        else if(nextComp1.Count == 1)
        {
            var next1 = nextComp1[0];
            var next2 = nextComp2[0];
            if (next1.tag == "Node" && nextComp2.Count == 1 && next2.tag == "Node")
            {
                while(next2.tag == "Node")
                {
                    var comp = GetDoubledEndedObject(next2);
                    if(comp.GetNextComponent().Count == 1)
                    {
                        next2 = comp.GetNextComponent()[0];
                    }
                    if (next1 == next2)
                        return true;
                }
            }
        }
        else if(nextComp2.Count == 1)
        {
            var next1 = nextComp1[0];
            var next2 = nextComp2[0];
            if (next1.tag == "Node" && nextComp1.Count == 1 && next2.tag == "Node")
            {
                while (next2.tag == "Node")
                {
                    var comp = GetDoubledEndedObject(next1);
                    if (comp.GetNextComponent().Count == 1)
                    {
                        next1 = comp.GetNextComponent()[0];
                    }
                    if (next1 == next2)
                        return true;
                }
            }
        }
        return false;
    }

    void DisableColliders()
    {
        foreach (var key in connectedComponents.Keys)
        {
            if (key.tag != "Resistor")
            {
                key.GetComponent<BoxCollider2D>().enabled = false;
                foreach (var child in key.GetComponentsInChildren<BoxCollider2D>())
                {
                    child.enabled = false;
                }
            }
        }

    }


}
