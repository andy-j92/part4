using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    public Text help;
    public Transform helpScreen;
    public Boolean state = false;
    public Button playerOne, playerTwo;
    public int one, two, numplayer;
    public Image pOne, pTwo;
    public Animator aOne, aTwo;
    public float timer = 20f;
    public Text time, final;
    public int numQ = 0;
    public Image circuit;
    public Transform gameOver, goBack;
    public Boolean pause;
    public InputField p1ans, p2ans;
    public LoadRandomCircuit load = new LoadRandomCircuit();

    void Awake()
    {

        one = SelectPlayer.ONE;
        two = SelectPlayer.TWO;
        numplayer = SelectPlayer.PNUM;

        if (numplayer == 1)
        {
            playerTwo.gameObject.SetActive(false);
            p2ans.gameObject.SetActive(false);
            switch (one)
            {
                case 1:
                    playerOne.image.sprite = Resources.Load<Sprite>("chara/CAP");
                    //playerOne.gameObject.GetComponents<Animator> = (Resources.Load<Animator>("cahra.cao"));
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
            p2ans.gameObject.SetActive(true);
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
        help.text = " < Objective >: Calculate and Enter the correct equvialent resistance of the circuit. \n < Score System >: Player to answer correct will < +score >. \n IF, both players answer correct within the give time, first person to answer will gain extra points. \n \n<EXIT> : | esc | ";
        InvokeRepeating("decreaseTimeRemaining", 1, 1);
    }

    private void Update()
    {
        helpScreen.gameObject.SetActive(state);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goBack.gameObject.SetActive(true);
            CancelInvoke();
        }

        if (timer == 0)
        {
            numQ++;

            if (numQ == 2)
            {
                timer = 0;
                gameOver.gameObject.SetActive(true);
                CancelInvoke();

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
                load.NextCircuit();
                timer = 20;
            }
        }
       
           time.text = timer + "";
        
    }

    void decreaseTimeRemaining()
    {
        timer--;
    }

    public void HELP()
    {
        state = !state;
    }

    public void buttonYes()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void buttonNo()
    {
        goBack.gameObject.SetActive(false);
        InvokeRepeating("decreaseTimeRemaining", 1, 1);
    }
}
