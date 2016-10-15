using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

    public Node location;
    // put this generic behavior on all players
    public bool isAttack; // is in attack phase/attack button was pressed
    public GameObject attackSelector; // individual prefab for attack selection i.e. Marlowe has close range attack assigned
    public GameObject moveSelector; // prefab for move range selection
    public GameObject enemyTarget; // enemy to attack this turn
    public GameObject[] inRangeEnemies; // stores all enemies in range
    //public GameObject gameManager;
    public GameObject textController;
    public Text selectText; // "Select Enemy" or "Select Player"
    private bool isCreated; // to only instantiate once
    public bool isMove;

	// Use this for initialization
	void Start () {
        
        isCreated = false;
        isAttack = false;
        isMove = false;


    }
	
	// Update is called once per frame
	void Update () {

        if(isAttack)
        {
            isCreated = true;
            if(isCreated)
            {
                // turn on range selector
                Instantiate(attackSelector, new Vector3(gameObject.transform.position.x, 3.01f, gameObject.transform.position.z), Quaternion.identity);
                SetTargets();
                textController.GetComponent<UIControl>().selectPanel.SetActive(true);
                selectText.text = "Select Target";
                isCreated = false;
                isAttack = false;
            }

        }
        if(isMove)
        {
            isCreated = true;
            if(isCreated)
            {
                Instantiate(moveSelector, new Vector3(gameObject.transform.position.x, 3.01f, gameObject.transform.position.z), Quaternion.identity);
                isCreated = false;

            }
            isMove = false;
        }
	
	}
    private void CreateSelection() // obj to instantiate
    {
        
    }

    private void SetTargets()
    {
        // find viable enemies
        Collider[] hitColliders = Physics.OverlapSphere(attackSelector.transform.position, 0.5f); // (center, radius)
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].tag == "Enemy")
            {
                inRangeEnemies[i] = hitColliders[i].gameObject;
            }
            i++;
        }

    }
}
