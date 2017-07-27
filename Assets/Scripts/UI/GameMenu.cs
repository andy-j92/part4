using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu: MonoBehaviour
	{
		public void MainGame (string name)
		{
			SceneManager.LoadScene ("PlayerSelect");
		}

		public void DC_Practise (string name)
		{
			SceneManager.LoadScene ("PractisePage");
		}

		public void Draw (string name)
		{
			SceneManager.LoadScene ("CircuitCreator");
		}

	public void Start (string name)
	{
		SceneManager.LoadScene ("Startscreen");
	}
}

