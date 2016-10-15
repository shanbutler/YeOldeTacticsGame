using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIControl : MonoBehaviour {

    public GameObject gameController; // assign game manager object
    public GameObject charStatusPanel;
    public GameObject charActionPanel;
    public GameObject selectPanel;
    public Text selectText;
    public bool panelOn; // controls action panel and character stats panel
    public bool selectPanelOn; // panel that tells when to select a character or enemy

	// Use this for initialization
	void Start () {
        //panelOn = true; // default is on, set main character as default
        panelOn = false; // default, all panels are off
        //selectPanelOn = false;
        selectPanel.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {

        if (panelOn)
        {
            charStatusPanel.SetActive(true);
            charActionPanel.SetActive(true);
        }
        else
        {
            charStatusPanel.SetActive(false);
            charActionPanel.SetActive(false);
        }
        if (gameController.GetComponent<StateMachineScript>().selectPhase)
        {
            selectPanel.SetActive(true);
        }
        else
        {
            selectPanel.SetActive(false);
        }
        if (gameController.GetComponent<StateMachineScript>().isMove)
        {
            selectPanel.SetActive(true);
            selectText.text = "Select Move";
        }


    }

}
