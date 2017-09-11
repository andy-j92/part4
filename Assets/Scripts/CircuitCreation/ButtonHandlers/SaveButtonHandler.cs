using System.Collections;
using System.Collections.Generic;
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
    private GameObject failed;
    private StringBuilder sb;
    public static int count;
    

    void Start()
    {
        count = 0;
        sb = new StringBuilder();
        filenameInput = GameObject.FindGameObjectWithTag("FilenameInput");
        warning = GameObject.FindGameObjectWithTag("Warning");
        noTitleWanring = GameObject.FindGameObjectWithTag("NoTitleWarning");
        saveModal = GameObject.FindGameObjectWithTag("SavePanel");
        succesful = GameObject.FindGameObjectWithTag("SuccessfulFeedback");
        failed = GameObject.FindGameObjectWithTag("FailedSaveFeedback");

        warning.SetActive(false);
        noTitleWanring.SetActive(false);
        saveModal.SetActive(false);
        succesful.SetActive(false);
        failed.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && saveModal.activeSelf)
            SaveCircuit();
        else if (Input.GetKeyDown(KeyCode.Escape) && saveModal.activeSelf)
            CloseModal();
    }

    public void OpenModal()
    {
        if (ConnectionChecker())
        {
            saveModal.SetActive(true);
            saveModal.transform.SetAsFirstSibling();
        }
        else
            StartCoroutine(ShowFailureFeedback());
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
            if (component.tag.Equals("Resistor"))
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
        sb = new StringBuilder();
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

        if (File.Exists(filePath + filename))
        {
            warning.SetActive(true);
            return;
        }
        else if(filename == "")
        {
            noTitleWanring.SetActive(true);
            return;
        }

        var file = File.CreateText(filePath + filename);
        file.WriteLine(sb.ToString());
        file.Close();

        saveModal.SetActive(false);
        StartCoroutine(ShowFeedback());
    }

    bool ConnectionChecker()
    {
        var components = ConnectionHandler.circuitComponents;
        var wires = ConnectionHandler.wires;

        Debug.Log(components.Count);
        Debug.Log(wires.Count);

        List<bool> result = new List<bool>();
        foreach (var component in components)
        {
            if (component.tag != "Wire")
            {
                foreach (var wire in wires)
                {
                    if (wire.GetComponent1() == component || wire.GetComponent2() == component)
                    {
                        result.Add(true);
                        break;
                    }
                }
            }
        }
        return result.Count == components.Count - wires.Count ? true:false;

    }

    IEnumerator ShowFeedback()
    {
        succesful.SetActive(true);
        yield return new WaitForSeconds(2);
        succesful.SetActive(false);
    }

    IEnumerator ShowFailureFeedback()
    {
        failed.GetComponent<Text>().text = "Unable to save circuit. Circuit in incomplete.";
        failed.SetActive(true);
        yield return new WaitForSeconds(2);
        failed.SetActive(false);
    }
}
