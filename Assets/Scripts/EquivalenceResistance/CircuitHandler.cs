using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitHandler : MonoBehaviour {

    private Dictionary<GameObject, List<GameObject>> _components;
    private GameObject startingComp;
    private GameObject prevComp;
    private List<GameObject> nextComp = new List<GameObject>();
    private bool hasMultiple = false;
    private List<GameObject> connectedComp;
    public static List<DoubleEnded> componentOrder;

    public static GameObject selected1;
    public static GameObject selected2;

    public CircuitHandler(Dictionary<GameObject, List<GameObject>> components)
    {
        _components = components;
        connectedComp = new List<GameObject>();
    }

    public void StartSetUp()
    {
        componentOrder = new List<DoubleEnded>();

        var components = _components.Keys;

        foreach (var component in components)
        {
            SetUpConnection(component);
        }
    }

    void SetUpConnection(GameObject component)
    {
        DoubleEnded deComponent = new DoubleEnded(component);

        if (prevComp != null)
        {
            deComponent.SetPreviousComponent(prevComp);
        }

        _components.TryGetValue(component, out connectedComp);

        if (connectedComp == null || component.tag.Equals("EndingNode"))
        {
            return;
        }
        else
        {
            foreach (var comp in connectedComp)
            {
                nextComp.Add(comp);
            }
        }

        deComponent.SetNextComponent(nextComp);
        nextComp = new List<GameObject>();
        componentOrder.Add(deComponent);
        prevComp = component;
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

    DoubleEnded GetDoubledEndedObject(GameObject component)
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
        else if (component1.GetPreviousComponent().tag == component2.GetCurrentComponent().tag || component2.GetPreviousComponent().tag == component1.GetCurrentComponent().tag)
        {
            return true;
        }
        else if (nextComp1.Count == 1)
        {
            if (nextComp1[0].tag == "Node")
            {
                GameObject nextComp = nextComp1[0];
                while (nextComp.tag == "Node")
                {
                    Debug.Log("HEREE");
                    var deComponent = GetDoubledEndedObject(nextComp);
                    if (deComponent.GetNextComponent().Count > 1 || deComponent.GetNextComponent().Count == 0)
                        return false;
                    else if (deComponent.GetNextComponent().Count == 1)
                    {
                        nextComp = deComponent.GetNextComponent()[0];
                        if (nextComp.tag == "Resistor")
                            return true;
                    }
                }
            }
            else if (nextComp1[0].tag == "Resistor")
            {
                return true;
            }
        }
        return false;
    }
}
