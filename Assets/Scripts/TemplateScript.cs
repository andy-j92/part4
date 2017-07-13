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

    void Start()
    {
        isHidden = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(isWithinBoundary() && isHidden)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else if (!isWithinBoundary())
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mouseRay, Vector2.zero, Mathf.Infinity);

            if (rayHit.collider == null)
            {
                Destroy(gameObject);
                Instantiate(finalObject, transform.position, Quaternion.identity);
            }
        }
	}

    bool isWithinBoundary()
    {
        if (Input.mousePosition.x < 20 || Input.mousePosition.x > 642 || Input.mousePosition.y < 36 || Input.mousePosition.y > 373)
            return false;
        isHidden = true;
        return true;
    }
}
