using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorScript : MonoBehaviour {
    
    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    [Range(1f, 200f)]
    private float drawDistance = 50f;

    private GameObject drawObject;
    private bool drawing = false;
    private bool isRotated = false;
    private Vector3 wireInstantiationPos;
    
    void Start()
    {
        wireInstantiationPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            endDraw();
        }
        else if (drawing)
        {
            whileDrawing();
        }
    }

    void startDraw()
    {
        var instantiattionPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // create a new instance
        drawObject = Instantiate(prefab, wireInstantiationPos, Quaternion.identity) as GameObject;

        // save our draw starting point
        drawObject.transform.position = wireInstantiationPos;

        // allow the Update to call whileDrawing()
        drawing = true;
    }

    void endDraw()
    {
        // forbid the Update do call whileDrawing()
        drawing = false;
    }

    void whileDrawing()
    {
        
        // manipulate the instance in whatever way you like
        float mouseDistance = Vector3.Distance(wireInstantiationPos, Input.mousePosition);
        drawObject.transform.localScale = new Vector3(mouseDistance, 1f, 1f);
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        if (Input.GetMouseButtonDown(0))
        {
            startDraw();
        }
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
