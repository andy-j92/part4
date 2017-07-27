using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {
	public Transform pON, pOFF;
	int player_count=0;
	int click_number = 0;
	string[] arr1 = new string[5];
	public Button player_back, go, b_cap, b_led, b_res, b_trans;
	public Image cap_one, cap_two, cap_three, cap_four;
	public Image led_one, led_two, led_three, led_four;
	public Image res_one, res_two, res_three, res_four;
	public Image trans_one, trans_two, trans_three, trans_four;
	/*public Image O_ONE, T_ONE, T_TWO, TH_ONE, TH_TWO, TH_THREE, F_ONE, F_TWO, F_THREE, F_FOUR;
	public Text one,two,three,four;*/

	public void Reload (string name)
	{
		SceneManager.LoadScene ("PlayerSelect");
	}

	public void Playscreem (string name)
	{
		SceneManager.LoadScene ("Playscreen");

		/*if (player_count == 1) {
			one.gameObject.SetActive (true);
			O_ONE.gameObject.SetActive (true);
		} else if (player_count == 2) {
			two.gameObject.SetActive (true);
			T_ONE.gameObject.SetActive (true);
			T_TWO.gameObject.SetActive (true);
		} else if (player_count == 3) {
			three.gameObject.SetActive (true);
			TH_ONE.gameObject.SetActive (true);
			TH_TWO.gameObject.SetActive (true);
			TH_THREE.gameObject.SetActive (true);
		} else {
			four.gameObject.SetActive (true);
			F_ONE.gameObject.SetActive (true);
			F_TWO.gameObject.SetActive (true);
			F_THREE.gameObject.SetActive (true);
			F_FOUR.gameObject.SetActive (true);
		}*/
	} 

	public void GameMenu (string name)
	{
			SceneManager.LoadScene ("GameMenu");
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
			//CAP.StopPlayback = false; 
			click_number++; 
			b_cap.interactable = false;

			if (click_number > player_count) {
				click_number = player_count;	//abort out of the loop
			}

			if (click_number == 1) {
				cap_one.enabled = true;
				arr1 [click_number] = "cap_one";
			} else if (click_number == 2) {
				cap_two.enabled = true;
				arr1 [click_number] = "cap_two";
			} else if (click_number == 3) {
				cap_three.enabled = true;
				arr1 [click_number] = "cap_three";
			} else {
				cap_four.enabled = true;
				arr1 [click_number] = "cap_four";
			}

			if (click_number == player_count) 
			{
				go.gameObject.SetActive (true);
				b_cap.interactable = false;
				b_led.interactable = false;
				b_res.interactable = false;
				b_trans.interactable = false;	
			}
		}
	}

	public void led (bool clicked)
	{
		if (clicked == true) 
		{

			//LED.StopPlayback = false;
			click_number++;
			b_led.interactable = false;

			if (click_number > player_count) {
				click_number = player_count;
			}

			if (click_number == 1) {
				led_one.enabled = true;
				arr1 [click_number] = "led_one";
			} else if (click_number == 2) {
				led_two.enabled = true;
				arr1 [click_number] = "led_two";
			} else if (click_number == 3) {
				led_three.enabled = true;
				arr1 [click_number] = "led_three";
			} else {
				led_four.enabled = true;
				arr1 [click_number] = "led_four";
			}

			if (click_number == player_count) 
			{
				go.gameObject.SetActive (true);
				b_cap.interactable = false;
				b_led.interactable = false;
				b_res.interactable = false;
				b_trans.interactable = false;
			}

		}

	}

	public void res (bool clicked)
	{
		if (clicked == true) 
		{

			//RES.StopPlayback = false;
			click_number++;
			b_res.interactable = false;

			if (click_number > player_count) {
				click_number = player_count;
			}

			if (click_number == 1) {
				res_one.enabled = true;
				arr1 [click_number] = "res_one";
			} else if (click_number == 2) {
				res_two.enabled = true;
				arr1 [click_number] = "res_two";
			} else if (click_number == 3) {
				res_three.enabled = true;
				arr1 [click_number] = "res_three";
			} else {
				res_four.enabled = true;
				arr1 [click_number] = "res_four";
			}

			if (click_number == player_count) 
			{
				go.gameObject.SetActive (true);
				b_cap.interactable = false;
				b_led.interactable = false;
				b_res.interactable = false;
				b_trans.interactable = false;
			}

		}

	}

	public void trans(bool clicked)
	{
		if (clicked == true) 
		{
			//TRANS.StopPlayback = false;
			click_number++;
			b_trans.interactable = false;

			if (click_number > player_count) {
				click_number = player_count;
			}

			if (click_number == 1) {
				trans_one.enabled = true;
				arr1 [click_number] = "trans_one";
			} else if (click_number == 2) {
				trans_two.enabled = true;
				arr1 [click_number] = "trans_two";
			} else if (click_number == 3) {
				trans_three.enabled = true;
				arr1 [click_number] = "trans_three";
			} else {
				trans_four.enabled = true;
				arr1 [click_number] = "trans_four";
			}

			if (click_number == player_count) 
			{
				go.gameObject.SetActive (true);
				b_cap.interactable = false;
				b_led.interactable = false;
				b_res.interactable = false;
				b_trans.interactable = false;

			}
		}

	}
}