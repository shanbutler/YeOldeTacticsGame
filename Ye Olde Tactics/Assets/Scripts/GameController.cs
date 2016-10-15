using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject activeChar;
    //public GameObject gameController; // set as game manager
    private bool selectPhase;
    public bool test;

	// Use this for initialization
	void Start () {
        selectPhase = false;
        test = false;
	}
	
	// Update is called once per frame
	void Update () {

        selectPhase = gameObject.GetComponent<StateMachineScript>().selectPhase;


        if (selectPhase)
        {
            if (Input.GetMouseButtonDown(0))
            {
                test = true;
                GameObject obj = GetClickedObject();
                if (obj.tag == "Player")
                {

                    //Destroy(obj); // test - has worked
                    obj.tag = "ActiveChar";
                    gameObject.GetComponent<StateMachineScript>().setNextState();
                    gameObject.GetComponent<StateMachineScript>().currentPlayer = obj;
                }

            }
        }
        


        
	
	}

    // if player phase
    // on click of character, switch active character
    // get the clicked gameobject
    public GameObject GetClickedObject()
    {
        
        GameObject selectedObject = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // get point on plane where clicked
        if (Physics.Raycast(ray, out hit))
        {
            selectedObject = hit.transform.gameObject;
            //Destroy(selectedObject); // test - has worked
            
        }
        return selectedObject;
    }
}
