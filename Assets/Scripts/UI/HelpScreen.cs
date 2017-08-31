using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpScreen: MonoBehaviour
{
    public Text instructions, pagename;
    public int page = 0;
    public Button backbutton, nextbutton;

    public void Help_Back (string name)
	{
		SceneManager.LoadScene ("Startscreen");
	}
    
    public void back()
    {
        page--;

        if(page == 0)
        {
            backbutton.gameObject.SetActive(false);
        }

        if(page < 2)
        {
            nextbutton.gameObject.SetActive(true);
        }
    }

    public void next()
    {
        page++;

        if (page > 0)
        {
            backbutton.gameObject.SetActive(true);
        }

        if(page == 2)
        {
            nextbutton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(page == 0)
        {
            pagename.text = "CIRCUIT CREATE";
            instructions.text = "\n < R >: Rotates the Component \n < Right Click >: Unselects Selected Component \n < Delete Component >: 'CLICK' component + | backspace | or | delete | \n \n note: resistors MUST be connected by a 'NODE' Component \n";
        }
        else if (page == 1)
        {
            pagename.text = "EQUVIALENT RESISTANCE";
            instructions.text = "\n < Directions >: Use MOUSE to Play! \n < RESET >: Reloads current circuit with NEW values \n \n \n note: maximum TWO resistors can be selected at any time \n";
        }
        else
        {
            pagename.text = "MINI GAME";
            instructions.text = "< Objective >: Calculate and Enter the correct equvialent resistance of the circuit. \n <Score System >: Player to answer correctly will < + score >. \n IF, both players answer correctly within the give time, first person to answer will gain extra points. \n \n < EXIT > : | esc |";
        }
    }
}


