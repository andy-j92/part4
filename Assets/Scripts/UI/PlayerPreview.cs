using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPreview: MonoBehaviour
{
	public void PlayerSelect (string name)
	{
		SceneManager.LoadScene ("PlayerSelect");

	}
}

