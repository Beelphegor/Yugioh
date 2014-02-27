using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject hand;
	public GameObject deck;
	public GameObject field;

	// Use this for initialization
	void Start () {
		deck.GetComponent<Deck> ().InitializeWithRandomCards ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			Card card = deck.GetComponent<Deck>().Draw();
			card.moveCardToHand(hand.GetComponent<Hand>().Cards.Count);
			hand.GetComponent<Hand>().Cards.Add (card);
		}
	}
}
