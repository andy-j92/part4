using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformHandler : MonoBehaviour {

   // public GameObject action;

    private static GameObject _wire;
    private static GameObject _action;
    public static List<GameObject> actions = new List<GameObject>();

    public static void TransformSeries(GameObject resistor1, GameObject resistor2)
    {
        var deResistor1 = CircuitHandler.GetDoubledEndedObject(resistor1);
        var deResistor2 = CircuitHandler.GetDoubledEndedObject(resistor2);
        //Components in series can only have a single previous component
        var prevComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[0]);

        if(deResistor2.GetNextComponent().Count == 0)
        {
            var nextComp1 = CircuitHandler.GetDoubledEndedObject(deResistor1.GetNextComponent()[0]);
            nextComp1.GetNextComponent().Remove(resistor2);
            deResistor2.GetPreviousComponent().Remove(nextComp1.GetCurrentComponent());
            nextComp1.GetPreviousComponent().Add(deResistor2.GetPreviousComponent()[0]);

            var prevNode2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetPreviousComponent()[0]);
            prevNode2.GetNextComponent().Remove(resistor2);
            prevNode2.GetNextComponent().Add(nextComp1.GetCurrentComponent());
        }
        else
        {
            var nextComp2 = CircuitHandler.GetDoubledEndedObject(deResistor2.GetNextComponent()[0]);
            prevComp2.GetNextComponent().Remove(resistor2);
            prevComp2.GetNextComponent().AddRange(deResistor2.GetNextComponent());
        }
        

        var rotation = resistor2.transform.rotation;
        var position = resistor2.transform.position;

        var actionText = "Series Transformation: \n R(" + resistor1.GetComponentInChildren<TextMesh>().text +
            ") & R(" + resistor2.GetComponentInChildren<TextMesh>().text + ")";

        var newAction = Instantiate(_action);
        newAction.transform.parent = GameObject.FindGameObjectWithTag("History").transform;
        newAction.GetComponent<Text>().text = actionText;
        newAction.transform.localScale = new Vector3(1, 1, 1);
        newAction.GetComponent<RectTransform>().position = new Vector3(newAction.GetComponent<RectTransform>().position.x, newAction.GetComponent<RectTransform>().position.y, 1);
        actions.Add(newAction);

        var newWire = Instantiate(_wire);
        newWire.transform.position = position;
        newWire.transform.localScale = new Vector3(3, 1, 1);
        newWire.transform.rotation = rotation;
        resistor1.GetComponentInChildren<TextMesh>().text = CalculateSeriesResistance(resistor1, resistor2);
        Destroy(resistor2);
        TransformComplete(resistor1, resistor2);
    }

    static string CalculateSeriesResistance(GameObject comp1, GameObject comp2)
    {
        int resistance1 = 0;
        int resistance2 = 0;
        int.TryParse(comp1.GetComponentInChildren<TextMesh>().text, out resistance1);
        int.TryParse(comp2.GetComponentInChildren<TextMesh>().text, out resistance2);
        return (resistance1 + resistance2).ToString();
    }

    static string CalculateParallelResistance(GameObject comp1, GameObject comp2)
    {
        double resistance1 = 0.0;
        double resistance2 = 0.0;
        double.TryParse(comp1.GetComponentInChildren<TextMesh>().text, out resistance1);
        double.TryParse(comp2.GetComponentInChildren<TextMesh>().text, out resistance2);
        return ((resistance1 + resistance2)/2).ToString();
    }

    public static void TransformParallel(GameObject resistor1, GameObject resistor2)
    {
        var deComp1 = CircuitHandler.GetDoubledEndedObject(resistor1);
        var deComp2 = CircuitHandler.GetDoubledEndedObject(resistor2);

        var prevComp1 = deComp1.GetPreviousComponent();
        var nextComp1 = deComp1.GetNextComponent();
        var prevComp2 = deComp2.GetPreviousComponent();
		var nextComp2 = deComp2.GetNextComponent();

		List<Wire> wire = new List<Wire>();
        if(prevComp1.Count == 2)
        {
            foreach (var item in prevComp2)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetNextComponent().Contains(resistor2))
                {
                    component.GetNextComponent().Remove(resistor2);
                }
            }
            foreach (var item in nextComp2)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetPreviousComponent().Contains(resistor2))
                {
                    component.GetPreviousComponent().Remove(resistor2);
                }
            }

            foreach (var item in CircuitHandler.wires)
            {
                var comp1 = item.GetComponent1();
                var comp2 = item.GetComponent2();

                if (comp1 == resistor2 || comp2 == resistor2)
                {
                    Destroy(item.GetWireObject());
                    wire.Add(item);
                }
            }
            resistor1.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
            Destroy(resistor2);
        }
        else if (prevComp2.Count == 2)
        {
            foreach (var item in prevComp1)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetNextComponent().Contains(resistor1))
                {
                    component.GetNextComponent().Remove(resistor1);
                }
            }
            foreach (var item in nextComp1)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetPreviousComponent().Contains(resistor1))
                {
                    component.GetPreviousComponent().Remove(resistor1);
                }
            }

            foreach (var item in CircuitHandler.wires)
            {
                var comp1 = item.GetComponent1();
                var comp2 = item.GetComponent2();

                if (comp1 == resistor1 || comp2 == resistor1)
                {
                    Destroy(item.GetWireObject());
                    wire.Add(item);
                }
            }
            resistor2.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
            Destroy(resistor1);
        }
        else if (nextComp1.Count == 0)
        {
            deComp2.GetNextComponent().Remove(resistor1);
            foreach (var item in CircuitHandler.wires)
            {
                var comp1 = item.GetComponent1();
                var comp2 = item.GetComponent2();

                if ((comp1 == resistor1 && comp2 == resistor2) || (comp1 == resistor2 && comp2 == resistor1))
                {
                    Destroy(item.GetWireObject());
                    wire.Add(item);
                }
            }
            resistor2.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
            Destroy(resistor1);
        }
        else if (nextComp2.Count == 0)
		{
			deComp1.GetNextComponent().Remove(resistor2);
			foreach (var item in CircuitHandler.wires)
			{
				var comp1 = item.GetComponent1();
				var comp2 = item.GetComponent2();

				if ((comp1 == resistor1 && comp2 == resistor2) || (comp1 == resistor2 && comp2 == resistor1))
				{
					Destroy(item.GetWireObject());
					wire.Add(item);
				}
			}
            resistor1.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
            Destroy(resistor2);
        }
        else
        {
            foreach (var item in prevComp1)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetNextComponent().Contains(resistor1))
                {
                    component.GetNextComponent().Remove(resistor1);
                }
            }
            foreach (var item in nextComp1)
            {
                var component = CircuitHandler.GetDoubledEndedObject(item);
                if (component.GetPreviousComponent().Contains(resistor1))
                {
                    component.GetPreviousComponent().Remove(resistor1);
                }
            }

            foreach (var item in CircuitHandler.wires)
            {
                var comp1 = item.GetComponent1();
                var comp2 = item.GetComponent2();

                if (comp1 == resistor1 || comp2 == resistor1)
                {
                    Destroy(item.GetWireObject());
                    wire.Add(item);
                }
            }
            resistor2.GetComponentInChildren<TextMesh>().text = CalculateParallelResistance(resistor1, resistor2);
            Destroy(resistor1);
        }

        foreach (var item in wire)
        {
            CircuitHandler.wires.Remove(item);
        }
        var actionText = "Parallel Transformation: \n R(" + resistor1.GetComponentInChildren<TextMesh>().text +
            ") & R(" + resistor2.GetComponentInChildren<TextMesh>().text + ")";

        var newAction = Instantiate(_action);
        newAction.transform.parent = GameObject.FindGameObjectWithTag("History").transform;
        newAction.GetComponent<Text>().text = actionText;
        newAction.transform.localScale = new Vector3(1, 1, 1);
        newAction.GetComponent<RectTransform>().position = new Vector3(newAction.GetComponent<RectTransform>().position.x, newAction.GetComponent<RectTransform>().position.y, 1);
        actions.Add(newAction);

        TransformComplete(resistor1, resistor2);
    }

    static void  TransformComplete(GameObject resistor1, GameObject resistor2)
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
