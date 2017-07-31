using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpScreen: MonoBehaviour
{
	public void Help_Back (string name)
	{
		SceneManager.LoadScene ("Startscreen");
	}
}


