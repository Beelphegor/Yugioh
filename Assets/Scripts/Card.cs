using System;
using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{

    public string code;
	public CardMetadata cardMetadata;
	public delegate void Summon(Card g);
	public event Summon SummonMonster;
    public delegate void Sacrifice(Card g);
    public event Sacrifice SacrificeMonster;
    public event Summon SacrificeMonsterSummon;
	public bool isSelected;
	public bool isFirstPlayerCard;
	public bool isOnMonsterZone;
	public bool isSacrificeable;
    public bool cardWasMeantToBeSummon;
    public bool doneSacrification;
    public bool markedToSacrifice;


	// Use this for initialization
	void Start () {
		isSelected = false;
		isOnMonsterZone = false;
		isSacrificeable = false;
	    code = cardMetadata.code;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI()
	{
		float buttonHeight = 25;
		float buttonWidth = 25;

		var recCoordinates = Camera.main.WorldToScreenPoint(transform.position);
		Rect buttonRect = new Rect(recCoordinates.x, Screen.height - recCoordinates.y, buttonWidth, buttonHeight);
		if (isSelected && !isOnMonsterZone) {
			if (GUI.Button (buttonRect, "S"))
			{
			    Debug.Log("click en la s");
				SummonMonster (this);
				isSelected = false;
			}
		}

		if (isSacrificeable) {
			if(GUI.Button (buttonRect, "X"))
			{
			    SacrificeMonster(this);
			}
		}

        if (doneSacrification && cardWasMeantToBeSummon)
        {
            if(GUI.Button(buttonRect, ":)"))
            {
                SacrificeMonsterSummon(this);
                isSelected = false;
            }
        }
	}
	void OnMouseDown()
	{
		isSelected = true;
	}

    

	public void moveCardToMonsterZone (Vector3 zonePosition)
	{
		StartCoroutine (AnimateMovementToMonsterZone(zonePosition));
	}

	IEnumerator AnimateMovementToMonsterZone (Vector3 zonePosition)
	{
		var initialPosition = transform.position;
		
		for (float f = 0; f <= 1; f += 0.01f) {
			transform.position = Vector3.Lerp (initialPosition, zonePosition, f);
			yield return null;
		}
	}

    public void moveCardToGraveyard(Vector3 zonePosition)
    {
        StartCoroutine(AnimateMovementToGraveyard(zonePosition));
    }

    IEnumerator AnimateMovementToGraveyard(Vector3 zonePosition)
    {
        var initialPosition = transform.position;

        for (float f = 0; f <= 1; f += 0.01f)
        {
            transform.position = Vector3.Lerp(initialPosition, zonePosition, f);
            yield return null;
        }
    }

    public decimal GetMonstersNeededToSummon()
    {
        return cardMetadata.level < 5 ? 0 : cardMetadata.level < 7 ? 1 : 2;
    }

    public void OrderCard(float xPosition, float yPosition)
    {
        StartCoroutine(AnimateMovementOnHand(xPosition, yPosition));
    }

    IEnumerator AnimateMovementOnHand(float xPosition, float yPosition)
    {
        var initialPosition = transform.position;
        //preguntar por que cojones pasa esto en stack overflow
        for (float f = 0; f <= 1.2; f += 0.1f)
        {
            transform.position = Vector3.Lerp(initialPosition, new Vector3(xPosition, yPosition, 0), f);
            yield return null;
        }
    }
}
