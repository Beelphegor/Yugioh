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
	public int monstersSummoned;
	public List<Card> cardsOnMonsterZone;
	public List<Card> cardsOnGraveyard;

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
		deck.GetComponent<Deck> ().CreateDeckForPlayerOne (cards.Where(x => x.cardType == "Normal monster").ToList());
		fusionMonsterZone.GetComponent<FusionMonsterZone>().CreateDeck (cards.Where(x => x.cardType == "Fusion monster").ToList());
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			if(isPlayerTurn && hand.GetComponent<Hand>().Cards.Count < 5){
				Card card = deck.GetComponent<Deck>().Draw();
				card.SummonMonster += OnMonsterSummon;
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
	void OnMonsterSummon(Card card){
		if (isPlayerTurn){
			if( isAllowedToSummonMonster (card)) {			
				if( isLowLevelCard(card)){
					monstersSummoned++;
					cardsOnMonsterZone.Add(card);
					card.isOnMonsterZone = true;
					var availableMonsterZone = monsterZones.Where (x => x.GetComponent<MonsterZone> ().isAvailable).First ();
					availableMonsterZone.GetComponent<MonsterZone> ().isAvailable = false;
					card.moveCardToMonsterZone (availableMonsterZone.transform.position);
					Debug.Log ("click en: " + card.cardMetadata.name);	
				} else if(isMediumLevelCard(card) && cardsOnMonsterZone.Count > 0) {
					AskForSacrifice();
				} else if(isHighLevelCard(card) && cardsOnMonsterZone.Count > 1){
					AskForSacrifice();
				} else {
					Debug.Log ("cannot summon that monster");
				}
			} 
		} else {
			Debug.Log ("cannot summon monster");
		}
	}
	void AskForSacrifice(){
		foreach (Card card in cardsOnMonsterZone) {
			card.isSacrificeable = true;
		}
	}

	bool isLowLevelCard(Card card){
		return card.cardMetadata.level < 5;
	}
	bool isMediumLevelCard(Card card){
		return card.cardMetadata.level >= 5 && card.cardMetadata.level <= 6;
	}
	bool isHighLevelCard(Card card){
		return card.cardMetadata.level > 6;
	}

	bool isAllowedToSummonMonster(Card card){
		return monstersSummoned == 0 ;
	}
}
