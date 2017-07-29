using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateScript : MonoBehaviour {

    [SerializeField]
    private GameObject finalObject;

    [SerializeField]
    private LayerMask allComponentsLayer;

    private Vector2 mousePosition;
    private bool isHidden;
    private bool isRotated;

    void Start()
    {
        isHidden = false;
        isRotated = false;
    }
	
	// Update is called once per frame
	void Update () {
        isHidden = isWithinBoundary();
        if(isHidden)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mouseRay, Vector2.zero, Mathf.Infinity);

            if (!isHidden) { }
            else if (rayHit.collider == null && isRotated)
            {
                Destroy(gameObject);
                ConnectionHandler.circuitComponents.Add(Instantiate(finalObject, transform.position, Quaternion.Euler(0,0,90f)));
            } else if (rayHit.collider == null && !isRotated)
            {
                Destroy(gameObject);
                ConnectionHandler.circuitComponents.Add(Instantiate(finalObject, transform.position, Quaternion.identity));
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!isRotated)
            {
                gameObject.transform.Rotate(Vector3.forward * 90);
                isRotated = true;
            }
            else
            {
                gameObject.transform.Rotate(Vector3.forward * -90);
                isRotated = false;
            }
        }
	}

    bool isWithinBoundary()
    {

        if (Input.mousePosition.x < 20 || Input.mousePosition.x > 642 || Input.mousePosition.y < 36 || Input.mousePosition.y > 373)
            return false;
        return true;
    }
}
