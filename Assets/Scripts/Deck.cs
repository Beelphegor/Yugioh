using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Deck : MonoBehaviour {

	public List<Card> Cards = new List<Card>();
	public GameObject CardPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter(){
		Debug.Log ("your deck has " + Cards[0].cardMetadata.name + " cards" );
	}

	public Card Draw ()
	{
		var drawedCard = Cards.First();
		Cards.Remove (drawedCard);
		drawedCard.moveCardToHand();
		return drawedCard;
	}

	public void InitializeWithRandomCards ()
	{
		for(int i = 0; i < 10; i++ ){
			var card = ((GameObject)Instantiate(CardPrefab, new Vector3(18, -1, 0), Quaternion.Euler(new Vector3()))).GetComponent<Card>();
			card.cardMetadata = new LOB002();
			Cards.Add (card);
		}

	}
}
