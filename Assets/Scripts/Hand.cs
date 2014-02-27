using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
	public List<Card> Cards = new List<Card>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter(){
		foreach (Card card in Cards) {
			//Debug.Log ("your hand has " + card.cardMetadata.name + " cards" );
		}
	}
}
