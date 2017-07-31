using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandler : MonoBehaviour {

    private static GameObject _wire;
    
	public static void TransformSeries(GameObject resistor1, GameObject resistor2, GameObject comp)
    {
        var deComp1 = CircuitHandler.GetDoubledEndedObject(comp);
        var deComp2 = CircuitHandler.GetDoubledEndedObject(resistor2);
        var deComp3 = CircuitHandler.GetDoubledEndedObject(deComp2.GetNextComponent()[0]);

        deComp1.GetNextComponent().Remove(resistor2);
        deComp1.GetNextComponent().Add(deComp3.GetCurrentComponent());
        deComp3.GetPreviousComponent().Remove(resistor2);
        deComp3.GetPreviousComponent().Add(comp);

        var rotation = resistor2.transform.rotation;
        var position = resistor2.transform.position;

        Destroy(resistor2);

        var newWire = Instantiate(_wire);
        newWire.transform.position = position;
        newWire.transform.localScale = new Vector3(3, 1, 1);
        newWire.transform.rotation = rotation;
        resistor1.GetComponentInChildren<TextMesh>().text = CalculateNewResistance(resistor1, resistor2);
    }

    static string CalculateNewResistance(GameObject comp1, GameObject comp2)
    {
        int resistance1 = 0;
        int resistance2 = 0;
        int.TryParse(comp1.GetComponentInChildren<TextMesh>().text, out resistance1);
        int.TryParse(comp2.GetComponentInChildren<TextMesh>().text, out resistance2);
        return (resistance1 + resistance2).ToString();
    }

    public static void SetWireObject(GameObject wire)
    {
        _wire = wire;
    }
}
