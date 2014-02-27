using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FusionMonsterZone : MonoBehaviour {
	public List<Card> Cards = new List<Card>();
	public GameObject CardPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateDeck (List<CardMetadata> cardMetadataList)
	{
		foreach(var cardMetadata in cardMetadataList){
			var card = ((GameObject)Instantiate(CardPrefab, transform.position, Quaternion.Euler(new Vector3()))).GetComponent<Card>();
			card.cardMetadata = cardMetadata;
			Cards.Add (card);
		}
	}
}
