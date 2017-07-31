using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	public Transform startScreen, gameMenu, draw; 
	/*public void LoadScene(string name)
	{
		Application.LoadLevel (name);
	}*/

	public void GameMenu(bool clicked)
	{
		if (clicked == true) 
		{
			gameMenu.gameObject.SetActive (clicked);
			startScreen.gameObject.SetActive (false);			
		}
			
		else
		{
			gameMenu.gameObject.SetActive(clicked);
			startScreen.gameObject.SetActive(true);
		}

	}

	public void DRAW(bool clicked)
	{
		if (clicked == true) 
		{
			draw.gameObject.SetActive (clicked);
			gameMenu.gameObject.SetActive (false);			
		}

		else
		{
			draw.gameObject.SetActive(clicked);
			gameMenu.gameObject.SetActive(true);
		}
	}
		
}

