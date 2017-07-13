using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsScript : MonoBehaviour {

    private bool isSelected;
    // Update is called once per frame
    void Start()
    {
        isSelected = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSelected)
        {
            Debug.Log("here??");
            isSelected = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
        }
        else if (Input.GetMouseButtonDown(0) && isSelected)
        {
            isSelected = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
