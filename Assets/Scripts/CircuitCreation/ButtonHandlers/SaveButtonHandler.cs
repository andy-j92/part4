using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonHandler : MonoBehaviour {

    private GameObject saveModal;
    private GameObject warning;
    private GameObject noTitleWanring;
    private GameObject filenameInput;
    private GameObject succesful;
    private StringBuilder sb;
    

    void Start()
    {
        sb = new StringBuilder();
        filenameInput = GameObject.FindGameObjectWithTag("FilenameInput");
        warning = GameObject.FindGameObjectWithTag("Warning");
        noTitleWanring = GameObject.FindGameObjectWithTag("NoTitleWarning");
        saveModal = GameObject.FindGameObjectWithTag("SavePanel");
        succesful = GameObject.FindGameObjectWithTag("SuccessfulFeedback");

        warning.SetActive(false);
        noTitleWanring.SetActive(false);
        saveModal.SetActive(false);
        succesful.SetActive(false);

    }

    public void OpenModal()
    {
        saveModal.SetActive(true);
        saveModal.transform.SetAsFirstSibling();
    }

    public void CloseModal()
    {
        saveModal.SetActive(false);
    }

	public void SaveCircuit()
    {
        
        var filename = filenameInput.GetComponent<InputField>().text;

        foreach (GameObject component in ConnectionHandler.circuitComponents)
        {
            if(component.tag.Equals("Resistor"))
            {
                sb.Append("Resistor ");
            }
            else if (component.tag.Equals("Node") || component.tag.Equals("StartingNode") || component.tag.Equals("EndingNode"))
            {
                sb.Append("Node ");
            }
            else if (component.tag.Equals("Wire"))
            {
                sb.Append("Wire ");
            }
            AppendContents(component);
        }

        CreateAndWriteToFile(filename);

    }

    void AppendContents(GameObject component)
    {
        sb.Append(component.transform.position.x + " ");
        sb.Append(component.transform.position.y + " ");
        sb.Append(component.transform.position.z + " ");
        sb.Append(component.transform.localScale.x + " ");
        sb.Append(component.transform.localScale.y + " ");
        sb.Append(component.transform.localScale.z + " ");
        sb.Append(component.transform.localRotation.x + " ");
        sb.Append(component.transform.localRotation.y + " ");
        sb.Append(component.transform.localRotation.z + " ");
        sb.Append(component.transform.localRotation.w + " ");
        sb.AppendLine();

    }

    void CreateAndWriteToFile(string filename)
    {
        var filePath = "Circuits/";

        if (File.Exists(filePath + filename + ".txt"))
        {
            warning.SetActive(true);
            return;
        }
        else if(filename == "")
        {
            noTitleWanring.SetActive(true);
            return;
        }

        var file = File.CreateText(filePath + filename + ".txt");
        file.WriteLine(sb.ToString());
        file.Close();

        saveModal.SetActive(false);
        StartCoroutine(ShowFeedback());
    }

    IEnumerator ShowFeedback()
    {
        succesful.SetActive(true);
        yield return new WaitForSeconds(2);
        succesful.SetActive(false);
    }
}
