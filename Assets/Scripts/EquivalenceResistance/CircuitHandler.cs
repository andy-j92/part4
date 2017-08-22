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

    public static Dictionary<GameObject, List<GameObject>> connectedComponents = new Dictionary<GameObject, List<GameObject>>();
    private Queue<GameObject> connectionQueue = new Queue<GameObject>();
    private List<GameObject> processedComponents = new List<GameObject>();
    private TransformHandler transformHandler = new TransformHandler();

    public static DoubleEnded selected1;
    public static DoubleEnded selected2;

    public void StartSetUp()
    {
        DisableColliders();
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
        if (selected1 != null && selected2 != null)
        {
            if (CheckSeries(selected1, selected2) != null)
            {
                TransformHandler.TransformSeries(selected1.GetCurrentComponent(), selected2.GetCurrentComponent(), CheckSeries(selected1, selected2));
                selected2 = null;
            }
            else if (CheckSeries(selected2, selected1) != null)
            {
                TransformHandler.TransformSeries(selected2.GetCurrentComponent(), selected1.GetCurrentComponent(), CheckSeries(selected2, selected1));
                selected1 = null;
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

        if (nextComp1.Count > 1 || nextComp1.Count <= 0)
            return null;
        else if (nextComp1.Count == 1 && nextComp1.Contains(component2.GetCurrentComponent()))
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
                Debug.Log("here");
                selected2 = null;
            }
            else if (CheckParallel(selected2, selected1))
            {
                Debug.Log("here");
                selected1 = null;
            }
        }
    }

    bool CheckParallel(DoubleEnded component1, DoubleEnded component2)
    {
        GameObject prevNode = null;
        GameObject nextNode = null;

        var prevComp = component1.GetPreviousComponent();
        var nextComp = component1.GetNextComponent();

        foreach (var comp in prevComp)
        {
            if (comp.tag == "Resistor")
                return false;
            else if (comp.tag == "Node")
                prevNode = comp;
        }

        foreach (var comp in nextComp)
        {
            if (comp.tag == "Resistor")
                return false;
            else if (comp.tag == "Node")
                nextNode = comp;
        }

        if (component2.GetPreviousComponent().Contains(prevNode) && component2.GetNextComponent().Contains(nextNode))
            return true;

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
            if (key != null && key.tag != "Resistor")
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
