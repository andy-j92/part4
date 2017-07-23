using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitHandler {

    private Dictionary<GameObject, List<GameObject>> _components;
    private GameObject startingComp;
    private GameObject prevComp;
    private List<GameObject> nextComp = new List<GameObject>();
    private bool hasMultiple = false;
    private List<GameObject> connectedComp;
    public static List<DoubleEnded> componentOrder = new List<DoubleEnded>();

    public static GameObject selected1;
    public static GameObject selected2;

    public CircuitHandler(Dictionary<GameObject, List<GameObject>> components)
    {
        _components = components;
        connectedComp = new List<GameObject>();
    }

    public void StartSetUp()
    {

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
}
