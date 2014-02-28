using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Cards;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public int TurnNumber = 0;

    void Awake()
    {
        Player1.GetComponent<Player>().InitialCardsDrawed += OnInitialCardsDrawedByPlayer1;
        Player2.GetComponent<Player>().InitialCardsDrawed += OnInitialCardsDrawedByPlayer2;
    }

    void OnInitialCardsDrawedByPlayer1()
    {
        Player2.GetComponent<Player>().DrawInitialCards();
    }

    void OnInitialCardsDrawedByPlayer2()
    {
        SetPlayersTurn();
    }

    // Use this for initialization
    void Start()
    {
        var cards = GenerateCards();
        Player1.GetComponent<Player>().deck.GetComponent<Deck>().CreateDeck(cards.Where(x => x.cardType == CartType.NormalMonster).ToList());
        Player1.GetComponent<Player>().fusionMonsterZone.GetComponent<FusionMonsterZone>().CreateDeck(cards.Where(x => x.cardType == CartType.FusionMonster).ToList());
        cards = GenerateCards();
        Player2.GetComponent<Player>().deck.GetComponent<Deck>().CreateDeck(cards.Where(x => x.cardType == CartType.NormalMonster).ToList());
        Player2.GetComponent<Player>().fusionMonsterZone.GetComponent<FusionMonsterZone>().CreateDeck(cards.Where(x => x.cardType == CartType.FusionMonster).ToList());
        DrawInitialCards();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DrawInitialCards()
    {
        Player1.GetComponent<Player>().DrawInitialCards();
    }

    void OnGUI()
    {
        float buttonHeight = 25;
        float buttonWidth = 40;

        Vector3 recCoordinates = Camera.main.WorldToScreenPoint(transform.position);
        var buttonRect = new Rect(recCoordinates.x, Screen.height - recCoordinates.y, buttonWidth, buttonHeight);
        if (GUI.Button(buttonRect, "Next"))
        {
            SetPlayersTurn();
        }
    }

    void SetPlayersTurn()
    {
        TurnNumber++;
        if (!Player1.GetComponent<Player>().isPlayerTurn)
        {
            Player1.GetComponent<Player>().StartTurn();
            Player2.GetComponent<Player>().EndTurn();
        }
        else
        {
            Player1.GetComponent<Player>().EndTurn();
            Player2.GetComponent<Player>().StartTurn();
        }
    }

    //clase!
    public List<CardMetadata> GenerateCards()
    {

        var randomCardList = new List<CardMetadata>();

        var runningAssembly = Assembly.GetExecutingAssembly();
        var typeOfCardMetadata = typeof(CardMetadata);
        var allCardMetadataTypes = new List<Type>();

        foreach (var type in runningAssembly.GetTypes())
        {
            if (typeOfCardMetadata.IsAssignableFrom(type))
                allCardMetadataTypes.Add(type);
        }

        for (int i = 0; i < 50; i++)
        {
            var theRandomIndex = (int)(UnityEngine.Random.value * allCardMetadataTypes.Count);
            var selectedType = allCardMetadataTypes[theRandomIndex];
            var selected = (CardMetadata)Activator.CreateInstance(selectedType);
            randomCardList.Add(selected);
        }

        return randomCardList;
    }
}