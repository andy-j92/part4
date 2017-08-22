using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitHandler : MonoBehaviour {

    private GameObject startingComp;
    private GameObject prevComp;
    private bool hasMultiple = false;
    private bool isAdded = false;
    public static List<DoubleEnded> componentOrder = new List<DoubleEnded>();
    public static List<GameObject> components = new List<GameObject>();
    public static List<Wire> wires = new List<Wire>();

    public static Dictionary<GameObject, List<GameObject>> connectedComponents = new Dictionary<GameObject, List<GameObject>>();
    private Queue<GameObject> connectionQueue = new Queue<GameObject>();
    private List<GameObject> processedComponents = new List<GameObject>();

    public static DoubleEnded selected1;
    public static DoubleEnded selected2;

    public void StartSetUp()
    {
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
        DisableColliders();
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
                        deComp.GetPreviousComponent().Add(deComponent.GetCurrentComponent());

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

                                if (!LinkExists(component, parent.gameObject))
                                {
                                    wires.Add(new Wire(comp, component, parent.gameObject));
                                }
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

    bool LinkExists(GameObject component1, GameObject component2)
    {
        foreach (var item in wires)
        {
            var comp1 = item.GetComponent1();
            var comp2 = item.GetComponent2();

            if ((comp1 == component1 && comp2 == component2) || (comp1 == component2 && comp2 == component1))
                return true;
        }
        return false;
    }

    public void SerialTransform()
    {
        if (selected1 != null && selected2 != null)
        {
            if (CheckSeries(selected1, selected2) != null)
            {
                TransformHandler.TransformSeries(selected1.GetCurrentComponent(), selected2.GetCurrentComponent(), CheckSeries(selected1, selected2));
            }
            else if (CheckSeries(selected2, selected1) != null)
            {
                TransformHandler.TransformSeries(selected2.GetCurrentComponent(), selected1.GetCurrentComponent(), CheckSeries(selected2, selected1));
            }
            else
            {
                StartCoroutine(ShowFeedBack("The Resistors are not in series."));
            }
        }
    }

    GameObject CheckSeries(DoubleEnded component1, DoubleEnded component2)
    {
        var nextComp1 = component1.GetNextComponent();
        var prevComp1 = component2.GetPreviousComponent();

        if (nextComp1.Count != 1)
            return null;
        else if (nextComp1.Count == 1 && nextComp1.Contains(component2.GetCurrentComponent()))
            return component1.GetCurrentComponent();
        else if (nextComp1.Count == 1 && GetDoubledEndedObject(nextComp1[0]).GetPreviousComponent().Contains(component2.GetCurrentComponent()))
            return component1.GetCurrentComponent();
        else
        {
            var component = component1.GetNextComponent()[0];
            var nextComp = GetDoubledEndedObject(component).GetNextComponent();
            while (nextComp.Count == 1)
            {
                if (nextComp.Contains(component2.GetCurrentComponent()))
                    return component;
                else
                {
                    component = nextComp[0];
                    nextComp = GetDoubledEndedObject(component).GetNextComponent();
                }
            }
        }
        return null;
    }

    public void ParallelTransform()
    {
        if (selected1 != null && selected2 != null)
        {
            if (CheckParallel(selected1, selected2))
            {
                TransformHandler.TransformParallel(selected1.GetCurrentComponent(), selected2.GetCurrentComponent());
            }
            else if (CheckParallel(selected2, selected1))
            {
                TransformHandler.TransformParallel(selected2.GetCurrentComponent(), selected1.GetCurrentComponent());
            }
        }
    }

    bool CheckParallel(DoubleEnded component1, DoubleEnded component2)
    {
        var prevComp1 = component1.GetPreviousComponent();
        var nextComp1 = component1.GetNextComponent();
        var prevComp2 = component2.GetPreviousComponent();
        var nextComp2 = component2.GetNextComponent();

        DoubleEnded prevNode1 = null;
        GameObject nextNode1 = null;
        GameObject prevNode2 = null;
        DoubleEnded nextNode2 = null;

        foreach (var prev in prevComp1)
        {
            if (prev.tag == "Node")
                prevNode1 = GetDoubledEndedObject(prev);
        }
        foreach (var next in nextComp1)
        {
            if (next.tag == "Node")
                nextNode1 = next;
        }
        foreach (var prev in prevComp2)
        {
            if (prev.tag == "Node")
                prevNode2 = prev;
        }
        foreach (var next in nextComp2)
        {
            if (next.tag == "Node")
                nextNode2 = GetDoubledEndedObject(next);
        }

        if (nextComp1.Contains(component2.GetCurrentComponent()) && component2.GetPreviousComponent()[0] == component1.GetCurrentComponent())
        {
            return true;
        }
        else if(prevNode1.GetNextComponent().Count > 1)
        {
            //R w R w case
            if(prevNode1.GetNextComponent().Contains(prevNode2) && nextNode2.GetPreviousComponent().Contains(nextNode1))
            {
                return true;
            }
            //R R w w case
            else if (prevNode1.GetNextComponent().Contains(component2.GetCurrentComponent()))
            {
                DoubleEnded nextNode = null;
                if(nextNode2.GetNextComponent().Count == 1 && nextNode2.GetNextComponent()[0].tag == "Node")
                {
                    nextNode = GetDoubledEndedObject(nextNode2.GetNextComponent()[0]);
                     if (nextNode.GetPreviousComponent().Count != 0 && nextNode.GetPreviousComponent().Contains(nextNode1))
                    {
                        return true;
                    }
                }
            }
            //R w w R case
            else if (GetDoubledEndedObject(nextNode1).GetNextComponent().Contains(component2.GetCurrentComponent()))
            {
                DoubleEnded nextNode = null;
                foreach (var item in prevNode1.GetNextComponent())
                {
                    if (item.tag == "Node")
                        nextNode = GetDoubledEndedObject(item);
                }

                if(nextNode != null && nextNode.GetNextComponent().Count == 1)
                {
                    if (nextNode.GetNextComponent()[0] = prevNode2)
                    {
                        return true;
                    }
                }
            }
        }
        

        return false;
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

    IEnumerator ShowFeedBack(string feedbackText)
    {
        var feedback = GameObject.FindGameObjectWithTag("Warning");
        feedback.GetComponent<Text>().text = feedbackText;
        yield return new WaitForSeconds(2);
        feedback.GetComponent<Text>().text = "";

    }
}
