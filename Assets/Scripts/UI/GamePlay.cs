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
    public float timer = 20f;
    public Text time, final;
    public int numQ = 0;
    public Image circuit;
    public Sprite[] circuits = new Sprite[9];
    public Transform gameOver, goBack;
    public Boolean pause;

    void Awake()
    {

        one = SelectPlayer.ONE;
        two = SelectPlayer.TWO;
        numplayer = SelectPlayer.PNUM;

        circuits[0] = Resources.Load<Sprite>("circuit/circuitOne");
        circuits[1] = Resources.Load<Sprite>("circuit/circuitTwo");
        circuits[2] = Resources.Load<Sprite>("circuit/circuitThree");
        circuits[3] = Resources.Load<Sprite>("circuit/circuitFour");
        circuits[4] = Resources.Load<Sprite>("circuit/circuitFive");
        circuits[5] = Resources.Load<Sprite>("circuit/circuitSix");
        circuits[6] = Resources.Load<Sprite>("circuit/circuitSeven");
        circuits[7] = Resources.Load<Sprite>("circuit/circuitEight");
        circuits[8] = Resources.Load<Sprite>("circuit/circuitNine");


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

    void Start()
    {
        gameOver.gameObject.SetActive(false);
        circuit.sprite = circuits[0];
        InvokeRepeating("decreaseTimeRemaining", 1 , 1);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            goBack.gameObject.SetActive(true);
            //stopTimer
        }

        if (timer == 0)
        {
            numQ++;

            if (numQ == 2)
            {
                timer = 0;
                pause = true;
                gameOver.gameObject.SetActive(true);

                if (numplayer == 1)
                {
                    final.text = "HIGHSCORE:";
                }

                else
                {
                    final.text = "PLAYER WINS";
                }
            }

            else
            {
                circuit.sprite = circuits[numQ];
                timer = 20;
            }
        }
       
           time.text = timer + "";
        
    }

    void decreaseTimeRemaining()
    {
        timer--;
    }
}
