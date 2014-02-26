using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {

	public GameObject deck;
	public GameObject hand;

	// Use this for initialization
	void Start () {
		deck.GetComponent<Deck> ().InitializeWithRandomCards ();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			Card card = deck.GetComponent<Deck>().Draw();
			hand.GetComponent<Hand>().Cards.Add (card);
		}
	}
}
