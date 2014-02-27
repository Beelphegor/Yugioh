using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

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
    public GameObject graveyard;
	public List<GameObject> monsterZones;
	public bool isPlayerTurn;
	public int monstersSummoned;
	public List<Card> cardsOnMonsterZone;
	public List<Card> cardsOnGraveyard;
    public List<Card> cardsToSacrifice;
    public Card cardToBeSummon;

	// Use this for initialization
	void Start () {
        cardsToSacrifice = new List<Card>();
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
		StartCoroutine ("DrawInitialCards");
	}

	IEnumerator DrawInitialCards(){
		for (int i = 0; i < 5; i++) {
			DrawCard();
			yield return new WaitForSeconds(0.4f);
		}
	}

	void DrawCard(){
		Card card = deck.GetComponent<Deck>().Draw();
		card.SummonMonster += OnMonsterSummon;
	    card.SacrificeMonster += OnSacrificeMonster;
	    card.SacrificeMonsterSummon += OnSacrificeMonsterSummon;
		card.moveCardToHand(hand.GetComponent<Hand>());
		hand.GetComponent<Hand>().Cards.Add (card);
	}

    // Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			if(isPlayerTurn){
				DrawCard();
			} 
		}
	}

	public List<CardMetadata> GenerateCards(){

		var randomCardList = new List<CardMetadata> ();

		var runningAssembly = Assembly.GetExecutingAssembly ();
		var typeOfCardMetadata = typeof(CardMetadata);
		var allCardMetadataTypes = new List<Type>();

		foreach (var type in runningAssembly.GetTypes())
		{
			if (typeOfCardMetadata.IsAssignableFrom(type))
				allCardMetadataTypes.Add(type);
		}

		for (int i = 0; i < 50; i++) {
			var theRandomIndex = (int)(UnityEngine.Random.value * allCardMetadataTypes.Count);			
			var selectedType = allCardMetadataTypes[theRandomIndex];			
			var selected = (CardMetadata)Activator.CreateInstance(selectedType);
			randomCardList.Add(selected);
		}

		return randomCardList;
	}


	void OnMonsterSummon(Card card){
		if (isPlayerTurn)
		{
		    cardToBeSummon = card;
			if( isAllowedToSummonMonster (card)) {			
				if( isLowLevelCard(card)){
					SummonMonster(card);
				} else if(isMediumLevelCard(card) && cardsOnMonsterZone.Count > 0) {
					AskForSacrifice(1, card);
				} else if(isHighLevelCard(card) && cardsOnMonsterZone.Count > 1){
					AskForSacrifice(2, card);
				} else {
					Debug.Log ("cannot summon that monster");
				    cardToBeSummon = null;
				}
			} 
		} else {
			Debug.Log ("cannot summon monster");
            cardToBeSummon = null;
		}
	}

    private void OnSacrificeMonsterSummon(Card card)
    {
        SummonMonster(card);
        foreach (Card sacrificedCard in cardsOnMonsterZone.Where(x => x.markedToSacrifice).ToList())
        {
            cardsOnMonsterZone.Remove(sacrificedCard);
            cardsOnGraveyard.Add(sacrificedCard);
            sacrificedCard.moveCardToGraveyard(graveyard.transform.position);
        }
    }

    private void SummonMonster(Card card)
    {
        monstersSummoned++;
        card.cardWasMeantToBeSummon = false;
        cardsOnMonsterZone.Add(card);
        card.isOnMonsterZone = true;
        var availableMonsterZone = monsterZones.First(x => x.GetComponent<MonsterZone>().isAvailable);
        availableMonsterZone.GetComponent<MonsterZone>().isAvailable = false;
        card.moveCardToMonsterZone(availableMonsterZone.transform.position);
        Debug.Log("click en: " + card.cardMetadata.name);
        cardToBeSummon = null;
    }

    private void OnSacrificeMonster(Card card)
    {
        if (isPlayerTurn)
        {
            card.markedToSacrifice = true;
            cardsToSacrifice.Add(card);
            monstersSummoned--;
            if (cardsToSacrifice.Count == cardToBeSummon.GetMonstersNeededToSummon())
            {
                cardToBeSummon.doneSacrification = true;
            }
        }
    }

	void AskForSacrifice(int quantityOfSacrificationsNeeded, Card card)
	{
	    card.cardWasMeantToBeSummon = true;
		foreach (Card cardOnMonsterZone in cardsOnMonsterZone) {
			cardOnMonsterZone.isSacrificeable = true;
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
