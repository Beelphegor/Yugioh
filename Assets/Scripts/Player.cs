using Assets.Scripts.Cards;
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

    public delegate void EventHandler();

    public event EventHandler InitialCardsDrawed;

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
	}

    public void DrawInitialCards()
    {
        StartCoroutine(AnimateDrawInitialCards());
    }

    IEnumerator AnimateDrawInitialCards()
    {
        for (var i = 0; i < 5; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.4f);
        }
        InitialCardsDrawed();
        hand.GetComponent<Hand>().OrderHand();
    }

    public void DrawCard(){
		Card card = deck.GetComponent<Deck>().Draw();
		card.SummonMonster += OnMonsterSummon;
	    card.SacrificeMonster += OnSacrificeMonster;
	    card.SacrificeMonsterSummon += OnSacrificeMonsterSummon;
		hand.GetComponent<Hand>().Cards.Add (card);
        var specificCardSprite = Resources.Load<Sprite>(card.cardMetadata.code);
        card.transform.GetComponent<SpriteRenderer>().sprite = specificCardSprite;
	}

    // Update is called once per frame
	void Update () {
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
        card.isOnMonsterZone = true; //mejor si preguntamos al field si tiene esta carta
        var availableMonsterZone = monsterZones.First(x => x.GetComponent<MonsterZone>().isAvailable);
        availableMonsterZone.GetComponent<MonsterZone>().isAvailable = false;
        hand.GetComponent<Hand>().RemoveCard(card);
        card.moveCardToMonsterZone(availableMonsterZone.transform.position);
        Debug.Log("click en: " + card.cardMetadata.name);
        cardToBeSummon = null;
        Debug.Log("cartas en la mano: " + hand.GetComponent<Hand>().Cards.Count);
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

    public void StartTurn()
    {
        isPlayerTurn = true;
        monstersSummoned = 0;
        DrawCard();
        hand.GetComponent<Hand>().OrderHand();
    }

    public void EndTurn()
    {
		isPlayerTurn = false;
    }
}
