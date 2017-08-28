﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PractisePreview: MonoBehaviour
{
    public Image circuit;
    public Button play;
    public Sprite[] circuits = new Sprite [9];
    static int i = 0;

    void Awake()
    {
        circuits[0] = Resources.Load<Sprite>("circuit/circuitOne");
        circuits[1] = Resources.Load<Sprite>("circuit/circuitTwo");
        circuits[2] = Resources.Load<Sprite>("circuit/circuitThree");
        circuits[3] = Resources.Load<Sprite>("circuit/circuitFour");
        circuits[4] = Resources.Load<Sprite>("circuit/circuitFive");
        circuits[5] = Resources.Load<Sprite>("circuit/circuitSix");
        circuits[6] = Resources.Load<Sprite>("circuit/circuitSeven");
        circuits[7] = Resources.Load<Sprite>("circuit/circuitEight");
        circuits[8] = Resources.Load<Sprite>("circuit/circuitNine");
    }

    public void R()
    {
        i++;

        if(i > 8)
        {
            i = 0;
        }
        circuit.sprite = circuits[i];
        play.gameObject.SetActive(false);
    }

    public void L()
    {
        i--;

        if (i < 0)
        {
            i = 8;
        }

        circuit.sprite = circuits[i];
        play.gameObject.SetActive(false);
    }
    public void EQ()
    {
        circuit.sprite = circuits[0];
    }

    public void SELECT()
    {
        play.gameObject.SetActive(true);
    }

    public void PLAY()
    {
        SceneManager.LoadScene("EquivalentResistance");
    }

    public void back()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public static int numCircuit
    {
        get{return i;}
        set { i = value; }
    }
}

