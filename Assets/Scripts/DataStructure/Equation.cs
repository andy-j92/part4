using System.Text;
using UnityEngine;

public class Equation {

    private StringBuilder equation;

    public Equation()
    {
        equation = new StringBuilder();
    }

    public void Series(GameObject resistor1, GameObject resistor2)
    {
        equation.AppendLine(resistor1.tag + "," + resistor2.tag + ",series");
    }

    public void Parallel(GameObject resistor1, GameObject resistor2)
    {
        equation.AppendLine(resistor1.tag + "," + resistor2.tag + ",parallel");
    }

    public string GetEquation()
    {
        return equation.ToString();
    }
}
