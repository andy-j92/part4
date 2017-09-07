using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation {

    private string equation;

    public Equation()
    {
        equation = "";
    }

    public void Series(GameObject resistor1, GameObject resistor2)
    {
        if (equation.Equals(""))
            equation += resistor1.tag + " + " + resistor2.tag;
        else
            equation += " + " + resistor1.tag + " + " + resistor2.tag;
    }

    public void Parallel(GameObject resistor1, GameObject resistor2)
    {
        if (equation.Equals(""))
            equation += "(" + resistor1.tag + " + " + resistor2.tag;
        else
            equation += " + " + resistor1.tag + " + " + resistor2.tag;
    }
}
