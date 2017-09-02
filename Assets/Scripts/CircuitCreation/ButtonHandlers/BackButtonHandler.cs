using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour {

	public void MoveToGameMenuScreen()
    {
        SceneManager.LoadScene("GameMenu");
        CircuitHandler.selected1 = null;
        CircuitHandler.selected2 = null;
    }

    public void MoveToPracticeScreen()
    {
        SceneManager.LoadScene("PractisePage");
    }
}
