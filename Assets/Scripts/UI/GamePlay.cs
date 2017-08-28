using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public Button playerOne, playerTwo;
    public int one, two, numplayer;
    public Image pOne, pTwo;
    public Animator aOne, aTwo;

    void Awake()
    {
        one = SelectPlayer.ONE;
        two = SelectPlayer.TWO;
        numplayer = SelectPlayer.PNUM;

        if (numplayer == 1)
        {
            playerTwo.gameObject.SetActive(false);
            switch (one)
            {
                case 1:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/CAP");
                    //playerOne.gameObject.GetComponent<Animator>() = Resources.Load<Animator>("cahra.cao");
                    break;
                case 2:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/LED");
                    break;
                case 3:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/RES");
                    break;
                case 4:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/TRANS");
                    break;
            }
        }
        else
        {
            playerTwo.gameObject.SetActive (true);
            switch (one)
            {
                case 1:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/CAP");
                    //playerOne.gameObject.GetComponent<Animator>() = Resources.Load<Animator>("cahra.cao");
                    break;
                case 2:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/LED");
                    break;
                case 3:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/RES");
                    break;
                case 4:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/TRANS");
                    break;
            }

            switch (two)
            {
                case 1:
                    playerTwo.image.sprite = Resources.Load<Sprite>("chara/CAP");
                    //playerOne.gameObject.GetComponent<Animator>() = Resources.Load<Animator>("cahra.cao");
                    break;
                case 2:
                    playerTwo.image.sprite = Resources.Load<Sprite>("chara/LED");
                    break;
                case 3:
                    playerTwo.image.sprite = Resources.Load<Sprite>("chara/RES");
                    break;
                case 4:
                    playerTwo.image.sprite = Resources.Load<Sprite>("chara/TRANS");
                    break;
            }

        }
    }
    
}
