﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformHandler : MonoBehaviour
{

    // public GameObject action;

    private static GameObject _wire;
    private static GameObject _action;
    public static List<GameObject> actions = new List<GameObject>();

    public static void TransformSeries(GameObject resistor1, GameObject resistor2)
    {
        var deResistor2 = CircuitHandler.GetDoubledEndedObject(resistor2);
        //Components in series can only have a single previous component
        var nextComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetNextComponent()[0]);
        var prevComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[0]);

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

        var newAction = Instantiate(_action);
        newAction.transform.SetParent(GameObject.FindGameObjectWithTag("History").transform);
        newAction.GetComponent<Text>().text = actionText;
        newAction.GetComponent<Text>().fontSize = 18;
        newAction.transform.localScale = new Vector3(1, 1, 1);
        newAction.GetComponent<RectTransform>().position = new Vector3(newAction.GetComponent<RectTransform>().position.x, newAction.GetComponent<RectTransform>().position.y, 1);
        actions.Add(newAction);

        var newWire = Instantiate(_wire);
        newWire.transform.position = position;
        newWire.transform.localScale = new Vector3(3, 1, 1);
        newWire.transform.rotation = rotation;
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
        return ((resistance1 + resistance2) / 2).ToString();
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
                item.GetWireObject().SetActive(false);
                wire.Add(item);
            }
        }
        resistor2.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
        resistor1.SetActive(false);

        foreach (var item in wire)
        {
            CircuitHandler.wires.Remove(item);
        }

        var newAction = Instantiate(_action);
        newAction.transform.SetParent(GameObject.FindGameObjectWithTag("History").transform);
        newAction.GetComponent<Text>().text = actionText;
        newAction.transform.localScale = new Vector3(1, 1, 1);
        newAction.GetComponent<RectTransform>().position = new Vector3(newAction.GetComponent<RectTransform>().position.x, newAction.GetComponent<RectTransform>().position.y, 1);
        actions.Add(newAction);

        CircuitHandler.equation.Parallel(resistor1, resistor2);
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

    public static void SetWireObject(GameObject wire)
    {
        _wire = wire;
    }

    public static void SetActionObject(GameObject action)
    {
        _action = action;
    }
}
