using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour {
	public Transform gameMenu, playerSelect, pON, pOFF;
	int player_count=0;
	int click_number = 0;
	public Button player_back;
	public Button play;

	public void PlayerSelect(bool clicked)
	{
		if (clicked == true) 
		{
			playerSelect.gameObject.SetActive (clicked);
			gameMenu.gameObject.SetActive (false);	
			pOFF.gameObject.SetActive (false);
			pON.gameObject.SetActive (true);
		}

		else
		{
			playerSelect.gameObject.SetActive(clicked);
			gameMenu.gameObject.SetActive(true);
			pOFF.gameObject.SetActive (false);
			pON.gameObject.SetActive (false);
		}
	}

	public void P1(bool clicked)
	{
		if (clicked == true) 
		{
			pOFF.gameObject.SetActive (clicked);
			pON.gameObject.SetActive (false);
			player_back.interactable = true;
			player_count = 1;
		}

		else
		{
			pOFF.gameObject.SetActive (clicked);
			pON.gameObject.SetActive (true);
			player_back.interactable = false;
		}

	}

	public void P2(bool clicked)
	{
		if (clicked == true) 
		{
			pOFF.gameObject.SetActive (clicked);
			pON.gameObject.SetActive (false);
			player_back.interactable = true;
			player_count = 2;
		}

	}

	public void P3(bool clicked)
	{
		if (clicked == true) 
		{
			pOFF.gameObject.SetActive (clicked);
			pON.gameObject.SetActive (false);
			player_back.interactable = true;
			player_count = 3;
		}

	}

	public void P4(bool clicked)
	{
		if (clicked == true) 
		{
			pOFF.gameObject.SetActive (clicked);
			pON.gameObject.SetActive (false);
			player_back.interactable = true;
			player_count = 4;
		}

	}

	public void cap (bool clicked)
	{
		if (clicked == true) 
		{
			click_number++; 
			if (click_number == player_count) 
			{
				play.interactable = true;			
			}
		}
	}

	public void led (bool clicked)
	{
		if (clicked == true) 
		{
			click_number++; 
			if (click_number == player_count) 
			{
				play.interactable = true;
			}
				
		}
		
	}

	public void res (bool clicked)
	{
		if (clicked == true) 
		{
			click_number++; 
			if (click_number == player_count) 
			{
				play.interactable = true;
			}
		}
		
	}

	public void trans(bool clicked)
	{
		if (clicked == true) 
		{
			click_number++; 
			if (click_number == player_count) 
			{
				play.interactable = true;
			}
		}
		
	}
}