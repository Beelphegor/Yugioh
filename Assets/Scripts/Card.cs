using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

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
			if (GUI.Button (buttonRect, "S")) {
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

	public void moveCardToHand (Hand hand)
	{
		StartCoroutine (AnimateMovementToHand(hand));
	}

	IEnumerator AnimateMovementToHand(Hand hand){
		var initialPosition = transform.position;

		for (float f = 0; f <= 1; f += 0.1f) {
			transform.position = Vector3.Lerp (initialPosition, new Vector3 (hand.transform.position.x - transform.localScale.x*2 + transform.localScale.x * (hand.Cards.Count - 1) , hand.transform.position.y, 0), f);
			yield return null;
		}

		var specificCardSprite = Resources.Load<Sprite>(cardMetadata.code);
		transform.GetComponent<SpriteRenderer>().sprite = specificCardSprite;
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
}
