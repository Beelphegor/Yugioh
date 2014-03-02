using Assets.Scripts.Cards;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

public class Player : MonoBehaviour {
    //pasar los monsterzones al field, y el graveyard tambien O.0 creooo
	public GameObject hand;
    public GameObject opponent;
	public GameObject deck;
    public GameObject field;
	public GameObject fusionMonsterZone;
    public GameObject graveyard;
    public GameObject actionsMenu;
	public bool isPlayerTurn;
	public int monstersSummonedOnTurn;
	public List<Card> cardsOnMonsterZone;// no seguro si esto va aqui
	public List<Card> cardsOnGraveyard;
    public List<Card> cardsToSacrifice;
    public Card cardToBeSummon;
    public Card cardToBeAttacked;
    public Card attackingCard;
    public delegate void EventHandler();
    public event EventHandler InitialCardsDrawed;
    public event EventHandler TurnFinished;
    public bool isFirstTurn;
    public int lifePoints;

	// Use this for initialization
	void Start ()
	{
	    isFirstTurn = true;
	    lifePoints = 8000;
        cardsToSacrifice = new List<Card>();
	}

    public void DrawInitialCards()
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }
        hand.GetComponent<Hand>().OrderHand();
    }

    public void DrawCard(){
		Card card = deck.GetComponent<Deck>().Draw();
		card.SummonMonster += OnMonsterSummon;
	    card.SacrificeMonster += OnSacrificeMonster;
        card.SelectCard += OnSelectCard;
	    card.SacrificeMonsterSummon += OnSacrificeMonsterSummon;
        card.AttackMonster += OnAttackMonster;
		hand.GetComponent<Hand>().Cards.Add(card);
        var specificCardSprite = Resources.Load<Sprite>(card.cardMetadata.code);
        card.transform.GetComponent<SpriteRenderer>().sprite = specificCardSprite;
	}

    void OnGUI()
    {
        float buttonHeightA = 25;
        float buttonWidthA = 25;
        var recPlayerCoordinates = Camera.main.WorldToScreenPoint(actionsMenu.transform.position);
        var actionButton = new Rect(recPlayerCoordinates.x, Screen.height - recPlayerCoordinates.y, buttonWidthA, buttonHeightA);
        if (isPlayerTurn && !isFirstTurn)
        {
            if (GUI.Button(actionButton, "A"))
            {
                var cards = field.GetComponent<Field>().Monsters();
                foreach (Card card in cards)
                {
                    card.isAbleToAttack = true;
                }
            }
        }

        var lifePointButton = new Rect(recPlayerCoordinates.x, Screen.height - recPlayerCoordinates.y - 40, 40, 25);
        GUI.Button(lifePointButton, "" + lifePoints);
    }

    private void OnBeingAttacked(Card card)
    {
        attackingCard.isAbleToAttack = false;
        attackingCard.alreadyAttacked = true;
        card.canBeAttacked = false;
        if (attackingCard.cardMetadata.attack > card.cardMetadata.attack)
        {
            opponent.GetComponent<Player>().field.GetComponent<Field>().RemoveMonsters(card);
            opponent.GetComponent<Player>().cardsOnGraveyard.Add(card);
            card.moveCardToGraveyard(opponent.GetComponent<Player>().graveyard.transform.position);
            opponent.GetComponent<Player>().lifePoints -= (attackingCard.cardMetadata.attack - card.cardMetadata.attack);
        }
        else if (attackingCard.cardMetadata.attack < card.cardMetadata.attack)
        {
            field.GetComponent<Field>().RemoveMonsters(attackingCard);
            cardsOnGraveyard.Add(attackingCard);
            attackingCard.moveCardToGraveyard(graveyard.transform.position);
            lifePoints -= (card.cardMetadata.attack - attackingCard.cardMetadata.attack);
        }
        else
        {
            field.GetComponent<Field>().RemoveMonsters(attackingCard);
            cardsOnGraveyard.Add(attackingCard);
            attackingCard.moveCardToGraveyard(graveyard.transform.position);

            opponent.GetComponent<Player>().field.GetComponent<Field>().RemoveMonsters(card);
            opponent.GetComponent<Player>().cardsOnGraveyard.Add(card);
            card.moveCardToGraveyard(opponent.GetComponent<Player>().graveyard.transform.position);
        }
        var opponentCards = opponent.GetComponent<Player>().field.GetComponent<Field>().Monsters();
        foreach (Card opponetCard in opponentCards)
        {
            opponetCard.canBeAttacked = false;
            opponetCard.BeingAttacked += null;
        }
    }

    private void OnAttackMonster(Card card)
    {
        attackingCard = card;
        var opponentCards = opponent.GetComponent<Player>().field.GetComponent<Field>().Monsters();
        foreach (Card opponetCard in opponentCards)
        {
            opponetCard.canBeAttacked = true;
            opponetCard.BeingAttacked += OnBeingAttacked;
        }
        var cards = field.GetComponent<Field>().Monsters();
        foreach (Card fieldCard in cards)
        {
            if(fieldCard != attackingCard)
                fieldCard.isAbleToAttack = false;
        }
    }

    private void OnSelectCard(Card card)
    {
        foreach (Card cardOnHand in hand.GetComponent<Hand>().Cards)
        {
            cardOnHand.isSelected = false;
        }
        card.isSelected = true;
    }

    // Update is called once per frame
	void Update () {
	}

	public virtual void OnMonsterSummon(Card card){

        Debug.Log("on monster summon");
		if (isPlayerTurn)
		{
		    cardToBeSummon = card;
			if( isAllowedToSummonMonster (card)) {			
				if( isLowLevelCard(card)){
					SummonMonster(card);
                }
                else if (isMediumLevelCard(card) && field.GetComponent<Field>().Monsters().Count > 0)
                {
					AskForSacrifice(card);
                }
                else if (isHighLevelCard(card) && field.GetComponent<Field>().Monsters().Count > 1)
                {
					AskForSacrifice(card);
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

    bool isAllowedToSummonMonster(Card card)
    {
        return monstersSummonedOnTurn == 0;
    }

    public virtual void OnSacrificeMonsterSummon(Card card)
    {
        cardsToSacrifice = new List<Card>();
        var monstersOnField = field.GetComponent<Field>().Monsters();
        foreach (Card cards in monstersOnField)
        {
            if (cards.markedToSacrifice)
            {
                Debug.Log("deberia de quitar esta carta: " + cards.cardMetadata.name);
                field.GetComponent<Field>().RemoveMonsters(cards);
                cardsOnGraveyard.Add(cards); //probable esto tenga que ser graveyard.sendMonster
                cards.moveCardToGraveyard(graveyard.transform.position);
                    // probable esto tenga que ir en graveyard.sendMonster
            }
            cards.isSacrificeable = false;
        }
        SummonMonster(card);
    }

    protected void SummonMonster(Card card)
    {
        monstersSummonedOnTurn++;
        card.cardWasMeantToBeSummon = false;
        field.GetComponent<Field>().AddMonster(card);
        hand.GetComponent<Hand>().RemoveCard(card);
        cardToBeSummon = null;
    }

    private void OnSacrificeMonster(Card card)
    {
        if (isPlayerTurn)
        {
            card.markedToSacrifice = true;
            cardsToSacrifice.Add(card);
            if (cardsToSacrifice.Count == cardToBeSummon.GetMonstersNeededToSummon())
            {
                cardToBeSummon.doneSacrification = true;
            }
        }
    }

	void AskForSacrifice(Card card)
	{
	    card.cardWasMeantToBeSummon = true;
        foreach (Card cardOnMonsterZone in field.GetComponent<Field>().Monsters())
        {
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

    public virtual void StartTurn()
    {
        isPlayerTurn = true;
        monstersSummonedOnTurn = 0;
        DrawCard();
        var cards = field.GetComponent<Field>().Monsters();
        foreach (Card card in cards)
        {
            card.alreadyAttacked = false;
        }
        hand.GetComponent<Hand>().OrderHand();
    }

    public virtual void EndTurn()
    {
		isPlayerTurn = false;
        TurnFinished();
    }
}
