using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public CardMetadata cardMetadata;
	public delegate void clickCard(Card g);
	public event clickCard onClick;
	public bool isSelected;
	public bool isFirstPlayerCard;

	// Use this for initialization
	void Start () {
		isSelected = false;
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
		if (isSelected) {
			if (GUI.Button (buttonRect, "M")) {
				onClick (this);
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
		StartCoroutine (AnimateMovementToMonsterZone1(zonePosition));
	}

	IEnumerator AnimateMovementToMonsterZone1 (Vector3 zonePosition)
	{
		var initialPosition = transform.position;
		
		for (float f = 0; f <= 1; f += 0.01f) {
			transform.position = Vector3.Lerp (initialPosition, zonePosition, f);
			yield return null;
		}
	}
}
