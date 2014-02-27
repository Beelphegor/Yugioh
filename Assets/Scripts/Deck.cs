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
		return drawedCard;
	}

	public void CreateDeckForPlayerOne (List<CardMetadata> cardMetadataList)
	{
		foreach(var cardMetadata in cardMetadataList){
			var card = ((GameObject)Instantiate(CardPrefab, transform.position, Quaternion.Euler(new Vector3()))).GetComponent<Card>();
			card.cardMetadata = cardMetadata;
			card.isFirstPlayerCard = true;
			Cards.Add (card);
		}
	}
	public void CreateDeckForPlayerTwo (List<CardMetadata> cardMetadataList)
	{
		foreach(var cardMetadata in cardMetadataList){
			var card = ((GameObject)Instantiate(CardPrefab, transform.position, Quaternion.Euler(new Vector3()))).GetComponent<Card>();
			card.cardMetadata = cardMetadata;
			card.isFirstPlayerCard = false;
			Cards.Add (card);
		}
	}
}
