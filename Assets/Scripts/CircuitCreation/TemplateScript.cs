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
        ConnectionHandler.templateActive = true;
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
        transform.position = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 1.0f);

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
            RaycastHit2D rayHit = Physics2D.Raycast(mouseRay, Vector2.zero, Mathf.Infinity);

            if (!isHidden) { }
            else if (rayHit.collider == null && isRotated)
            {
                Destroy(gameObject);
                ConnectionHandler.circuitComponents.Add(Instantiate(finalObject, transform.position, Quaternion.Euler(0,0,90f)));
                ConnectionHandler.templateActive = false;
            }
            else if (rayHit.collider == null && !isRotated)
            {
                Destroy(gameObject);
                ConnectionHandler.circuitComponents.Add(Instantiate(finalObject, transform.position, Quaternion.identity));
                ConnectionHandler.templateActive = false;
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
            ConnectionHandler.templateActive = false;
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
        var circuitPanel = GameObject.FindGameObjectWithTag("circuit_panel");
        if (RectTransformUtility.RectangleContainsScreenPoint(circuitPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
            return true;
        return false;
    }
}
