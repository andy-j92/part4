using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControl : MonoBehaviour {

    public static Queue<GameObject[]> versions = new Queue<GameObject[]>();

    public static void AddVersion(GameObject[] version)
    {
        Debug.Log(version);
        versions.Enqueue(version);
    }

    GameObject[] GetPreviousVersion() 
    {
        return versions.Dequeue();
    }

    void DrawComponents(GameObject[] version) {
        foreach (var item in version)
        {
            Debug.Log("here");
            Instantiate(item);
        }
    }

    public void Undo() {
        Debug.Log("undo button");
        Debug.Log(versions.Count);
        var prevVersion = GetPreviousVersion();
        Debug.Log(prevVersion.Length);
        DrawComponents(prevVersion);
        new CircuitHandler().StartSetUp();
    }
}
