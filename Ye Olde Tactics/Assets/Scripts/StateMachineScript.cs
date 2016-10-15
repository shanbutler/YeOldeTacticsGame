using UnityEngine;
using System.Collections;
	
public class StateMachineScript : MonoBehaviour {
	private BattleStates currentState;//this is our current state
	public GameObject currentPlayer;//current player
	private bool actionPerformed;
    public GameObject textController; // access text

    public bool selectPhase; // whether you can select a character or not
    private bool phaseTextOn; // if scrolling phase text is on or finished
    public bool isMove;
	
	public enum BattleStates {//States for our state machine
		StartTurn,
        SelectPhase,
		CharacterTurn,
        EnemyTurn,
		EndScreenTurn
	}

	void Start () {
        //currentState = BattleStates.EnemyTurn;
		currentState = BattleStates.StartTurn;
		actionPerformed = false;
        selectPhase = false;
        isMove = false;
        
	}
	
	void Update () {
		//Debug.Log(currentState);//output current state for testing
        phaseTextOn = textController.GetComponent<ScrollText>().textOn;
        

		switch(currentState) {
			case (BattleStates.StartTurn):
                // show enemy turn scrolling text
                textController.GetComponent<ScrollText>().StartText(0); // 0 refers to player phase
                //actionPerformed = true;
                //code goes here to make move and attack buttons invisible at first
                if (!phaseTextOn) setNextState();
                break;
            case (BattleStates.SelectPhase):
                //code goes here
                selectPhase = true;
                if (actionPerformed) setNextState();
                break;
            case (BattleStates.CharacterTurn):
                // turn on character action and stat panels
                textController.GetComponent<UIControl>().panelOn = true;
                textController.GetComponent<UIControl>().selectPanel.SetActive(false);
                if (actionPerformed) setNextState();
                break;
            case (BattleStates.EnemyTurn):
                //code goes here
                // show enemy turn scrolling text
                textController.GetComponent<ScrollText>().StartText(1); // 1 refers to enemy phase
                if (actionPerformed) setNextState();
                break;
            case (BattleStates.EndScreenTurn):
				//code goes here
				if(actionPerformed) setNextState();
				break;
		}
		
		//if(currentPlayer.getHealth() <= 0) setNextState();
	}

	public void MoveButtonPressed() {
        //Move();
        isMove = true;
        currentPlayer.GetComponent<CharacterBehavior>().isMove = true;
        //actionPerformed = true;
    }
	public void AttackButtonPressed() {
		//Attack();
        currentPlayer.GetComponent<CharacterBehavior>().isAttack = true;
		//actionPerformed = true;
	}
	public void StartButtonPressed() { actionPerformed = true; }
	
	public void setNextState() {
		actionPerformed = false;
		switch(currentState) {
			case BattleStates.StartTurn: 
					currentState = BattleStates.SelectPhase;
				break;
            case BattleStates.SelectPhase:
                currentState = BattleStates.CharacterTurn;
                break;
            case BattleStates.CharacterTurn: 
					currentState = BattleStates.SelectPhase;
				break;
            case BattleStates.EndScreenTurn:
				break;
			default:
				break;
		}
	}
}
