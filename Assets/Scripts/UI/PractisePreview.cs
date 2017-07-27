﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PractisePreview : MonoBehaviour {
	bool image1 = true;
	bool image2 = false;
	public Transform preview_er,preview_er2; 
	public Button play;

	public void R()
	{
		image1 = !image1;
		image2 = !image2;
		preview_er.gameObject.SetActive (image1);
		preview_er2.gameObject.SetActive (image2);
		play.gameObject.SetActive (false);
	}

	public void L()
	{
		image1 = !image1;
		image2 = !image2;
		preview_er.gameObject.SetActive (image1);
		preview_er2.gameObject.SetActive (image2);			
		play.gameObject.SetActive (false);
	}

	public void EQ()
	{
			preview_er.gameObject.SetActive (true);
	}

	public void SELECT ()
	{
		play.gameObject.SetActive (true);
	}

	public void PLAY (string name)
	{
		SceneManager.LoadScene ("EquivalentResistance");
	}

	public void back (string name)
	{
		SceneManager.LoadScene ("GameMenu");
	}
} 

