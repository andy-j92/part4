using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PractisePreview: MonoBehaviour
{
    public void PLAY()
    {
        SceneManager.LoadScene("EquivalentResistance");
    }

    public void back()
    {
        SceneManager.LoadScene("GameMenu");
    }

}

