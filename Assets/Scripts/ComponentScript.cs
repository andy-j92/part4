﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentScript : MonoBehaviour {

    public GameObject componentTemplate;

	// Update is called once per frame
	void OnMouseUp()
    {
        Instantiate(componentTemplate, Input.mousePosition, Quaternion.identity);
    }
}
