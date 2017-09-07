using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Equation {

    private StringBuilder equation;
    private double[] resistances;

    public Equation()
    {
        equation = new StringBuilder();
    }

    public Equation(int resistorCount)
    {
        resistances = new double[resistorCount];
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

    private double SeriesCalc(double resistance1, double resistance2)
    {
        return resistance1 + resistance2;
    }

    private double ParallelCalc(double resistance1, double resistance2)
    {
        return (resistance1 + resistance2) / 2;
    }

    private int GetResistorNumber(GameObject resistor)
    {
        var tag = resistor.tag;
        var index = tag.IndexOf('r',2);
        int num = 0;
        int.TryParse(tag.Substring(index + 1), out num);
        return num-1;
    }

    public double Calculate(FileInfo fileInfo)
    {
        StreamReader reader = fileInfo.OpenText();
        string text;
        double result = 0;
        while ((text = reader.ReadLine()) != "")
        {
            var split = text.Split(',');
            var action = split[2];
            var resistor1 = GameObject.FindGameObjectWithTag(split[0]);
            var resistor2 = GameObject.FindGameObjectWithTag(split[1]);
            var num1 = GetResistorNumber(resistor1);
            var num2 = GetResistorNumber(resistor2);
            double resistance1 = 0;
            double resistance2 = 0;

            if (resistances[num1] == 0)
            {
                double.TryParse(resistor1.GetComponentInChildren<TextMesh>().text, out resistance1);
                resistances[num1] = resistance1;
            }
            else
            {
                resistance1 = resistances[num1];
            }

            if (resistances[num2] == 0)
            {
                double.TryParse(resistor2.GetComponentInChildren<TextMesh>().text, out resistance2);
                resistances[num2] = resistance2;
            }
            else
            {
                resistance2 = resistances[num2];
            }

            foreach (var item in resistances)
            {
                Debug.Log(item);
            }
            Debug.Log("\n");
            if (action.Equals("series"))
            {
                result = SeriesCalc(resistance1, resistance2);
                resistances[num1] = result;
                resistances[num2] = result;
            }
            else if (action.Equals("parallel"))
            {
                result = ParallelCalc(resistance1, resistance2);
                resistances[num1] = result;
                resistances[num2] = result;
            }
            //Debug.Log(result);
        }
        return result;
    }
}
