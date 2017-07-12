using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    public GameObject text;
    public float x;
    public float y;
    private Transform parent;
    private Vector3 vector;
    // Use this for initialization
    void Start () {
        parent = GetComponent<RectTransform>();
        vector = new Vector3(x, y);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKey(KeyCode.Mouse0))
        {
            var history = Instantiate(text, vector, Quaternion.identity);
            history.transform.SetParent(parent);
            vector = new Vector3(vector.x + x, vector.y);
        }


    }
}
