﻿using System.Collections;
using UnityEngine;

public class DC_prac : MonoBehaviour {
	public Transform gameMenu, dc, preview_er,preview_er2,play,prac; 

	public void DC(bool clicked)
	{
		if (clicked == true) 
		{
			dc.gameObject.SetActive (clicked);
			gameMenu.gameObject.SetActive (false);			
		}

		else
		{
			dc.gameObject.SetActive(clicked);
			gameMenu.gameObject.SetActive(true);
		}
	}

	public void R(bool clicked)
	{
		if (clicked == true) {
			preview_er.gameObject.SetActive (clicked);
			preview_er2.gameObject.SetActive (false);			
		}

		else 
		{
			preview_er.gameObject.SetActive (clicked);
			preview_er2.gameObject.SetActive (true);
		}	

	}

	public void L(bool clicked)
	{
		if (clicked == true) 
		{
			preview_er.gameObject.SetActive (false);
			preview_er2.gameObject.SetActive (clicked);			
		}

		else 
		{
			preview_er.gameObject.SetActive (clicked);
			preview_er2.gameObject.SetActive (true);
		}	


	}

	public void EQ(bool clicked)
	{
		if (clicked == true) 
		{
			preview_er.gameObject.SetActive (clicked);
		} 

		else 
		{
			preview_er.gameObject.SetActive (clicked);
			preview_er2.gameObject.SetActive (clicked);
			play.gameObject.SetActive(false);
		}
	}

	public void SELECT (bool clicked)
	{
		if (clicked == true) 
		{
			play.gameObject.SetActive (clicked);
		} 

		else 
		{
			play.gameObject.SetActive(false);
		}
	}

	public void PLAY (bool clicked)
	{
		if (clicked == true) 
		{
			prac.gameObject.SetActive (clicked);
		} 

		else 
		{
			prac.gameObject.SetActive(false);
		}
	}

} 


