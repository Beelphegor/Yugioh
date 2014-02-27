using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {
	public GameObject hand;
	public GameObject deck;
	public GameObject fusionMonsterZone;
	public GameObject field;
	public GameObject monsterZone1;
	public GameObject monsterZone2;
	public GameObject monsterZone3;
	public GameObject monsterZone4;
	public GameObject monsterZone5;
	public List<GameObject> monsterZones;
	public bool isPlayerTurn;
	// Use this for initialization
	void Start () {
		monsterZones = new List<GameObject>(){
			monsterZone1,
			monsterZone2,
			monsterZone3,
			monsterZone4,
			monsterZone5
		};
		var cards = GenerateCards ();
		deck.GetComponent<Deck> ().CreateDeckForPlayerOne (cards.Where(x => x.type == "Normal monster").ToList());
		fusionMonsterZone.GetComponent<FusionMonsterZone>().CreateDeck (cards.Where(x => x.type == "Fusion monster").ToList());
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			if(isPlayerTurn){
				Card card = deck.GetComponent<Deck>().Draw();
				card.onClick += OnClickHandCard;
				card.moveCardToHand(hand.GetComponent<Hand>());
				hand.GetComponent<Hand>().Cards.Add (card);
			} 
		}
	}

	public List<CardMetadata> GenerateCards(){
		return new List<CardMetadata> (){
			new LOB002(),
			new LOB001(),
			new LOB000(),
			new LOB003(),
			new LOB005(),
			new LOB004()
		};
	}
	void OnClickHandCard(Card card){
		if (isPlayerTurn) {
			var availableMonsterZone = monsterZones.Where (x => x.GetComponent<MonsterZone> ().isAvailable).First ();
			availableMonsterZone.GetComponent<MonsterZone> ().isAvailable = false;
			card.moveCardToMonsterZone (availableMonsterZone.transform.position);
			Debug.Log ("click en: " + card.cardMetadata.name);
		}
	}
}
