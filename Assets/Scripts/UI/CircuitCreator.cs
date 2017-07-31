using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CircuitCreator: MonoBehaviour
{
	public void GameMenu (string name)
	{
		SceneManager.LoadScene ("GameMenu");
	}
}


