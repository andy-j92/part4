using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectorScript : MonoBehaviour {

    private bool isSelected;

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            if(ConnectionHandler.connector1 == null)
            {
                ConnectionHandler.connector1 = gameObject;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                isSelected = true;

            }
            else if(ConnectionHandler.connector1 != null && ConnectionHandler.connector1.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                ConnectionHandler.connector1 = null;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                isSelected = false;
            }
            else if(ConnectionHandler.connector2 == null)
            {
                ConnectionHandler.connector2 = gameObject;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 1);
                isSelected = true;

            }
            else if(ConnectionHandler.connector2 != null && ConnectionHandler.connector2.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                ConnectionHandler.connector2 = null;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                isSelected = false;
            }
        }
    }

    void OnMouseExit()
    {
        if(!isSelected)
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }
}
