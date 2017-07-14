﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsScript : MonoBehaviour {

    private bool isSelected;
    private int childCount;
    // Update is called once per frame
    void Start()
    {
        isSelected = false;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) && isSelected)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isSelected)
        {
            isSelected = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
        }
        else if (Input.GetMouseButtonDown(0) && isSelected)
        {
            isSelected = false;
            if(gameObject.tag.Equals("Wire"))
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            else
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
