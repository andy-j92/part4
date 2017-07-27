using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu: MonoBehaviour 
{
	public Transform optionMenu,music_on,music_off,sound_on,sound_off, music_volume, sound_volume;
	bool m_state = true;
	bool s_state = true;

	//AudioSource bgm;
	//float bgm_volume = 1.0f;

	public void GameMenu(string name)
	{
		SceneManager.LoadScene ("GameMenu");
	}

	public void OptionMenu(bool clicked)
	{
		if (clicked == true) 
		{
			optionMenu.gameObject.SetActive (clicked);
			music_on.gameObject.SetActive (m_state);
			music_off.gameObject.SetActive (!m_state);
			sound_on.gameObject.SetActive (s_state);
			sound_off.gameObject.SetActive (!s_state);
			//bgm = GetComponent<AudioSource>();
			//bgm.volume = bgm_volume;
		}

		else
		{
			optionMenu.gameObject.SetActive(clicked);
		}

	}

	public void music()
	{
			m_state = !m_state;
			music_off.gameObject.SetActive (!m_state);

		if (m_state == true) {	
			//bgm.mute = false;
		} else {
			//bgm.mute = true;
		}
	}

	public void sound(bool clicked)
	{
		if (clicked == true) 
		{
			s_state = !s_state;
		}

		else
		{
			s_state = !s_state;
		}

		sound_on.gameObject.SetActive (s_state);
		sound_off.gameObject.SetActive (!s_state);
	}

	public void Music_volume()
	{
		//bgm_volume = GameObject.Find ("Music_slider").GetComponent<Slider> ().value;
		//bgm.volume = bgm_volume;
	}
}


