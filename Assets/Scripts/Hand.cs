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
		Debug.Log ("your hand has " + Cards[0].cardMetadata.name + " cards" );
	}
}
