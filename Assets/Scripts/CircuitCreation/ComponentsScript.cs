using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComponentsScript : MonoBehaviour {

    private bool isSelected;
    private bool isTriggered;
    private List<GameObject> nextComponents = new List<GameObject>();
    List<GameObject> components = new List<GameObject>();

    void Start()
    {
        isSelected = false;
        isTriggered = false;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) && isSelected)
        {
            ConnectionHandler.circuitComponents.Remove(gameObject);
            Destroy(gameObject);
        }

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isSelected)
        {
            if (SceneManager.GetActiveScene().name.Equals("EquivalentResistance"))
            {
                if (CircuitHandler.selected1 == null)
                {
                    CircuitHandler.selected1 = gameObject;
                    isSelected = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                }
                else if (CircuitHandler.selected2 == null)
                {
                    CircuitHandler.selected2 = gameObject;
                    isSelected = true;
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                }
                else
                {
                    var feedback = GameObject.FindGameObjectWithTag("Warning");
                    feedback.GetComponent<Text>().text = "You cannot chose more than 2 components at a time.";
                }
            }
            else
            {
                isSelected = true;
                if (gameObject.tag.Equals("Wire"))
                    gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                else
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
            }
        }
        else if (Input.GetMouseButtonDown(0) && isSelected)
        {
            isSelected = false;

            if (CircuitHandler.selected1 == gameObject)
                CircuitHandler.selected1 = null;
            else if (CircuitHandler.selected2 == gameObject)
                CircuitHandler.selected2 = null;
            
            if (gameObject.tag.Equals("Wire"))
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            else
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (SceneManager.GetActiveScene().name.Equals("EquivalentResistance"))
        {
            if (!components.Contains(collider.gameObject))
            {
                var parent = collider.gameObject.transform.parent;
                if (parent != null && !components.Contains(parent.gameObject))
                {
                    components.Add(parent.gameObject);
                }
                else
                {
                    components.Add(collider.gameObject);
                }
            }

            if (!isTriggered)
            {
                foreach (var component in components)
                {

                    if (component.gameObject.tag != "Connector" && component.gameObject.tag != gameObject.tag && !nextComponents.Contains(component.gameObject))
                    {
                        nextComponents.Add(component.gameObject);
                    }
                }

                if (!LoadRandomCircuit.connectedComponents.ContainsKey(gameObject))
                {
                    LoadRandomCircuit.connectedComponents.Add(gameObject, nextComponents);
                }

                isTriggered = true;

            }
        }

    }
}
