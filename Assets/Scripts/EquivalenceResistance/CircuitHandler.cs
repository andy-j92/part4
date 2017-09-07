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

    public static DoubleEnded selected1 = null;
    public static DoubleEnded selected2 = null;

    void Start()
    {
        selected1 = null;
        selected2 = null;
    }

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
                            if (parent != null && parent.gameObject != component)
                            {
                                if(!nextComp.Contains(parent.gameObject))
                                {
                                    nextComp.Add(parent.gameObject);
                                    connectionQueue.Enqueue(parent.gameObject);
                                    if (!LinkExists(component, parent.gameObject))
                                    {
                                        wires.Add(new Wire(comp, component, parent.gameObject));
                                    }
                                }
                                else
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
                TransformHandler.TransformSeries(selected1.GetCurrentComponent(), selected2.GetCurrentComponent());
            }
            else if (CheckSeries(selected2, selected1) != null)
            {
                TransformHandler.TransformSeries(selected2.GetCurrentComponent(), selected1.GetCurrentComponent());
            }
            else
            {
                StartCoroutine(ShowFeedBack("The Resistors are not in series."));
            }
        }
    }

    GameObject CheckSeries(DoubleEnded component1, DoubleEnded component2)
    {
        GameObject currentComponent = component1.GetCurrentComponent();
        //previous component of a resistor is always 1
        GameObject previousComponent = component1.GetPreviousComponent()[0];
        while (currentComponent.tag != "EndingNode")
        {
            List<GameObject> connectedComponents = new List<GameObject>();
            if (currentComponent.tag == "StartingNode")
                break;
            foreach (var item in GetDoubledEndedObject(currentComponent).GetNextComponent())
            {
                    connectedComponents.Add(item);
            }
            foreach (var item in GetDoubledEndedObject(currentComponent).GetPreviousComponent())
            {
                    connectedComponents.Add(item);
            }
           
            if (connectedComponents.Count > 2)
                return null;
            else
            {
                connectedComponents.Remove(previousComponent);
                Debug.Log(connectedComponents.Count);
                if (connectedComponents[0] == component2.GetCurrentComponent())
                    return component1.GetCurrentComponent();
                else
                {
                    previousComponent = currentComponent;
                    currentComponent = connectedComponents[0];
                }
            }

        }
        previousComponent = component1.GetNextComponent()[0];
        currentComponent = component1.GetCurrentComponent();
        while (currentComponent.tag != "EndingNode")
        {
            List<GameObject> connectedComponents = new List<GameObject>();

            foreach (var item in GetDoubledEndedObject(currentComponent).GetNextComponent())
            {
                connectedComponents.Add(item);
            }
            Debug.Log(currentComponent.tag);
            foreach (var item in GetDoubledEndedObject(currentComponent).GetPreviousComponent())
            {
                connectedComponents.Add(item);
            }

            if (connectedComponents.Count > 2)
                return null;
            else
            {
                connectedComponents.Remove(previousComponent);
                Debug.Log(connectedComponents.Count);
                if (connectedComponents[0] == component2.GetCurrentComponent())
                    return component1.GetCurrentComponent();
                else
                {
                    previousComponent = currentComponent;
                    currentComponent = connectedComponents[0];
                }
            }

        }
        return null;
    }

    public void ParallelTransform()
    {
        bool isParallel1 = false;
        bool isParallel2 = false;
        if (selected1 != null && selected2 != null)
        {
            if (CheckParallel(selected1, selected2))
            {
                isParallel1 = true;
            }
            else if (CheckParallel(selected2, selected1))
            {
                isParallel2 = true;
            }
            else
            {
                StartCoroutine(ShowFeedBack("The Resistors are not in parallel."));
            }
        }
        if(isParallel1 || isParallel2)
        {
            var prev1 = selected1.GetPreviousComponent()[0];
            var next1 = selected1.GetNextComponent()[0];
            var prev2 = selected2.GetPreviousComponent()[0];
            var next2 = selected2.GetNextComponent()[0];
            var total1 = GetDoubledEndedObject(prev1).GetPreviousComponent().Count + GetDoubledEndedObject(prev1).GetNextComponent().Count;
            total1 += GetDoubledEndedObject(next1).GetPreviousComponent().Count + GetDoubledEndedObject(next1).GetNextComponent().Count;
            var total2 = GetDoubledEndedObject(prev2).GetPreviousComponent().Count + GetDoubledEndedObject(prev2).GetNextComponent().Count;
            total2 += GetDoubledEndedObject(next2).GetPreviousComponent().Count + GetDoubledEndedObject(next2).GetNextComponent().Count;

            Debug.Log(total1 + " : " + total2);
            if (total1 > total2)
                TransformHandler.TransformParallel(selected1.GetCurrentComponent(), selected2.GetCurrentComponent());
            else if(total2 > total1)
                TransformHandler.TransformParallel(selected2.GetCurrentComponent(), selected1.GetCurrentComponent());
            else
            {
                TransformHandler.TransformParallel(selected2.GetCurrentComponent(), selected1.GetCurrentComponent());
            }
        }
    }

    bool CheckParallel(DoubleEnded component1, DoubleEnded component2)
    {
        //resistors have a single prev and next node
        var prevNode1 = component1.GetPreviousComponent()[0];
        var nextNode1 = component1.GetNextComponent()[0];
        var prevNode2 = component2.GetPreviousComponent()[0];
        var nextNode2 = component2.GetNextComponent()[0];
        bool foundComp2 = false;
        
        Queue<GameObject> nextNodes = new Queue<GameObject>();
        bool backwards = false;
        nextNodes.Enqueue(prevNode1);
        while (nextNodes.Count > 0)
        {
            GameObject nextNode = nextNodes.Dequeue();
            DoubleEnded currentNode = GetDoubledEndedObject(nextNode);
            if(currentNode.GetNextComponent().Count == 0)
            {
                backwards = true;
            }
            if(backwards)
            {
                foreach (var item in currentNode.GetPreviousComponent())
                {
                    if (item.tag == "Node")
                        nextNodes.Enqueue(item);
                }
            }
            else
            {
                foreach (var item in currentNode.GetNextComponent())
                {
                    if (item.tag == "Node")
                        nextNodes.Enqueue(item);
                }
            }

            if (currentNode.GetNextComponent().Contains(component2.GetCurrentComponent()) || currentNode.GetPreviousComponent().Contains(component2.GetCurrentComponent()))
            {
                foundComp2 = true;
                break;
            }
            //go through the nodes, stop when reaching a resistor that is not component2.
            //go through all the possible node paths until component2 is reached.
            //then check if the next node of component 2 meets next node of componen1
        }

        backwards = false;
        nextNodes = new Queue<GameObject>();
        if (foundComp2)
        {
            nextNodes.Enqueue(nextNode2);
            nextNodes.Enqueue(prevNode2);
            while (nextNodes.Count > 0)
            {
                GameObject nextNode = nextNodes.Dequeue();
                DoubleEnded currentNode = GetDoubledEndedObject(nextNode);
                if (currentNode.GetNextComponent().Count == 0)
                {
                    backwards = true;
                }
                if (backwards)
                {
                    foreach (var item in currentNode.GetPreviousComponent())
                    {
                        if (item.tag == "Node")
                            nextNodes.Enqueue(item);
                    }
                }
                else
                {
                    foreach (var item in currentNode.GetNextComponent())
                    {
                        if (item.tag == "Node")
                            nextNodes.Enqueue(item);
                    }
                }

                if (currentNode.GetCurrentComponent() == nextNode1)
                {
                    return true;
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
            if (key != null && !key.tag.Contains("Resistor"))
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
