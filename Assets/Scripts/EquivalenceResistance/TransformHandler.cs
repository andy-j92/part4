using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformHandler : MonoBehaviour
{
    private static GameObject _wire;
    private static float historyBoxSize;
    public static List<GameObject> actions = new List<GameObject>();

    void Start()
    {
        actions = new List<GameObject>();
        historyBoxSize = 126;
    }

    public static void TransformSeries(GameObject resistor1, GameObject resistor2)
    {
        var deResistor2 = CircuitHandler.GetDoubledEndedObject(resistor2);
        DoubleEnded nextComp2 = null;
        DoubleEnded prevComp2 = null;

        if (deResistor2.GetNextComponent().Count != 0)
        {
            nextComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetNextComponent()[0]);
            prevComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[0]);

            if (prevComp2.GetNextComponent().Contains(resistor2))
            {
                prevComp2.GetNextComponent().Remove(resistor2);
                prevComp2.GetNextComponent().AddRange(deResistor2.GetNextComponent());
            }
            if (nextComp2.GetPreviousComponent().Contains(resistor2))
            {
                nextComp2.GetPreviousComponent().Remove(resistor2);
                nextComp2.GetPreviousComponent().AddRange(deResistor2.GetPreviousComponent());
            }
        }
        else
        {
            nextComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[0]);
            prevComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[1]);

            if (prevComp2.GetNextComponent().Contains(resistor2))
            {
                prevComp2.GetNextComponent().Remove(resistor2);
                foreach (var item in deResistor2.GetPreviousComponent())
                {
                    if (item != prevComp2.GetCurrentComponent())
                        prevComp2.GetNextComponent().Add(item);
                }
            }
            else if(prevComp2.GetPreviousComponent().Contains(resistor2))
            {
                prevComp2.GetPreviousComponent().Remove(resistor2);
                foreach (var item in deResistor2.GetPreviousComponent())
                {
                    if (item != prevComp2.GetCurrentComponent())
                        prevComp2.GetPreviousComponent().Add(item);
                }
            }

            if (nextComp2.GetPreviousComponent().Contains(resistor2))
            {
                nextComp2.GetPreviousComponent().Remove(resistor2);
                foreach (var item in deResistor2.GetPreviousComponent())
                {
                    if (item != nextComp2.GetCurrentComponent())
                        nextComp2.GetPreviousComponent().Add(item);
                }
            }
            else if(nextComp2.GetNextComponent().Contains(resistor2))
            {
                nextComp2.GetNextComponent().Remove(resistor2);
                foreach (var item in deResistor2.GetPreviousComponent())
                {
                    if (item != nextComp2.GetCurrentComponent())
                        nextComp2.GetNextComponent().Add(item);
                }
            }

        }


        //components in series must have a single prev and next or 2 prevs or 2 nexts 
        if (prevComp2.GetNextComponent().Contains(resistor2))
        {
            prevComp2.GetNextComponent().Remove(resistor2);
            prevComp2.GetNextComponent().AddRange(deResistor2.GetNextComponent());
        }
        if (nextComp2.GetPreviousComponent().Contains(resistor2))
        {
            nextComp2.GetPreviousComponent().Remove(resistor2);
            nextComp2.GetPreviousComponent().AddRange(deResistor2.GetPreviousComponent());
        }

        var rotation = resistor2.transform.rotation;
        var position = resistor2.transform.position;

        var actionText = "Series Transformation: \n R(" + resistor1.GetComponentInChildren<TextMesh>().text +
            ") & R(" + resistor2.GetComponentInChildren<TextMesh>().text + ")";

        AddAction(actionText);

        var newWire = Instantiate(_wire);
        newWire.transform.position = position;
        newWire.transform.localScale = new Vector3(3, 1, 1);
        newWire.transform.rotation = rotation;
        CircuitHandler.wires.Add(new Wire(newWire, null, null));
        resistor1.GetComponentInChildren<TextMesh>().text = CalculateSeriesResistance(resistor1, resistor2);
        resistor2.SetActive(false);
        CircuitHandler.equation.Series(resistor1, resistor2);
        TransformComplete(resistor1, resistor2);
    }

    static string CalculateSeriesResistance(GameObject comp1, GameObject comp2)
    {
        double resistance1 = 0;
        double resistance2 = 0;
        double.TryParse(comp1.GetComponentInChildren<TextMesh>().text, out resistance1);
        double.TryParse(comp2.GetComponentInChildren<TextMesh>().text, out resistance2);
        return (resistance1 + resistance2).ToString();
    }

    static string CalculateParallelResistance(GameObject comp1, GameObject comp2)
    {
        double resistance1 = 0.0;
        double resistance2 = 0.0;
        double.TryParse(comp1.GetComponentInChildren<TextMesh>().text, out resistance1);
        double.TryParse(comp2.GetComponentInChildren<TextMesh>().text, out resistance2);
        var result = Math.Round(((resistance1 * resistance2) / (resistance1 + resistance2)), 2);
        return result.ToString();
    }

    public static void TransformParallel(GameObject resistor1, GameObject resistor2)
    {
        var deComp1 = CircuitHandler.GetDoubledEndedObject(resistor1);

        var prevComp1 = deComp1.GetPreviousComponent();
        var nextComp1 = deComp1.GetNextComponent();

        var actionText = "Parallel Transformation: \n R(" + resistor1.GetComponentInChildren<TextMesh>().text +
        ") & R(" + resistor2.GetComponentInChildren<TextMesh>().text + ")";

        List<Wire> wire = new List<Wire>();
        var component = CircuitHandler.GetDoubledEndedObject(prevComp1[0]);
        if (component.GetNextComponent().Contains(resistor1))
        {
            component.GetNextComponent().Remove(resistor1);
        }

        component = CircuitHandler.GetDoubledEndedObject(nextComp1[0]);
        if (component.GetPreviousComponent().Contains(resistor1))
        {
            component.GetPreviousComponent().Remove(resistor1);
        }


        foreach (var item in CircuitHandler.wires)
        {
            var comp1 = item.GetComponent1();
            var comp2 = item.GetComponent2();

            if (comp1 == resistor1 || comp2 == resistor1)
            {
                Destroy(item.GetWireObject());
                CircuitHandler.connectedComponents.Remove(item.GetWireObject());
                wire.Add(item);
            }
        }
        resistor2.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
        resistor1.SetActive(false);

        foreach (var item in wire)
        {
            CircuitHandler.wires.Remove(item);
        }

        AddAction(actionText);

        CircuitHandler.equation.Parallel(resistor1, resistor2);
        RemoveHangingComponents();
        TransformComplete(resistor1, resistor2);
    }

    static void TransformComplete(GameObject resistor1, GameObject resistor2)
    {
        resistor1.GetComponent<ComponentsScript>().SetIsSelected(false);
        resistor2.GetComponent<ComponentsScript>().SetIsSelected(false);
        CircuitHandler.selected1.GetCurrentComponent().GetComponentInChildren<SpriteRenderer>().color = Color.white;
        CircuitHandler.selected2.GetCurrentComponent().GetComponentInChildren<SpriteRenderer>().color = Color.white;
        CircuitHandler.selected1 = null;
        CircuitHandler.selected2 = null;
    }

    static void AddAction(string actionText)
    {
        Debug.Log(actions.Count);
        var tag = actions.Count == 0 ? "Action" : "Action" + actions.Count;
        var newAction = GameObject.FindGameObjectWithTag(tag);
        if (actions.Count > 8)
        {
            var history = GameObject.FindGameObjectWithTag("History");
            history.GetComponent<RectTransform>().offsetMax = new Vector2(history.GetComponent<RectTransform>().offsetMax.x + historyBoxSize, 0);
        }
        newAction.GetComponent<Text>().text = actionText;
        actions.Add(newAction);

    }

    static void RemoveHangingComponents()
    {
        foreach (var item in CircuitHandler.connectedComponents.Keys)
        {
            if(item.activeSelf && item.tag == "Node")
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if(component.GetPreviousComponent().Count == 0 && component.GetNextComponent().Count == 1)
                {
                    var nextComp = CircuitHandler.GetDoubledEndedObject(component.GetNextComponent()[0]);
                    if (nextComp.GetPreviousComponent().Contains(item))
                        nextComp.GetPreviousComponent().Remove(item);
                    else if (nextComp.GetNextComponent().Contains(item))
                        nextComp.GetNextComponent().Remove(item);

                    foreach (var wire in CircuitHandler.wires)
                    {
                        if (wire.GetComponent1() == item || wire.GetComponent2() == item)
                        {
                            wire.GetWireObject().SetActive(false);
                            item.SetActive(false);
                        }
                    }
                }
                else if(component.GetPreviousComponent().Count == 1 && component.GetNextComponent().Count == 0)
                {
                    var prevComp = CircuitHandler.GetDoubledEndedObject(component.GetPreviousComponent()[0]);
                    if (prevComp.GetPreviousComponent().Contains(item))
                        prevComp.GetPreviousComponent().Remove(item);
                    else if (prevComp.GetNextComponent().Contains(item))
                        prevComp.GetNextComponent().Remove(item);

                    foreach (var wire in CircuitHandler.wires)
                    {
                        if (wire.GetComponent1() == item || wire.GetComponent2() == item)
                        {
                            wire.GetWireObject().SetActive(false);
                            item.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public static void SetWireObject(GameObject wire)
    {
        _wire = wire;
    }
}
