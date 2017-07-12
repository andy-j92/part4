using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour {

    public int col, row;
	// Use this for initialization
	void Start () {
        var parent = gameObject.GetComponent<RectTransform>();
        var grid = gameObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(parent.rect.width / col, parent.rect.height / row);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
