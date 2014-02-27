using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;

	// Use this for initialization
	void Start () {
		SetPlayersTurn ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		float buttonHeight = 25;
		float buttonWidth = 40;
		
		var recCoordinates = Camera.main.WorldToScreenPoint(transform.position);
		Rect buttonRect = new Rect(recCoordinates.x, Screen.height - recCoordinates.y, buttonWidth, buttonHeight);
		if (GUI.Button (buttonRect, "Next")) {
			SetPlayersTurn();
		}
	}

	void SetPlayersTurn(){
		if (!player1.GetComponent<Player> ().isPlayerTurn) {
			player1.GetComponent<Player> ().isPlayerTurn = true;
			player2.GetComponent<Player> ().isPlayerTurn = false;
		} else {
			player1.GetComponent<Player> ().isPlayerTurn = false;
			player2.GetComponent<Player> ().isPlayerTurn = true;
		}
	}
}
