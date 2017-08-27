using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonHandler : MonoBehaviour {

	public void MoveToGameMenuScreen()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void MoveToPracticeScreen()
    {
        SceneManager.LoadScene("PractisePage");
    }
}
