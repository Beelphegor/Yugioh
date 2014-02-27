using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public CardMetadata cardMetadata;
	public delegate void clickCard(Card g);
	public event clickCard onClick;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		onClick (this);
	}

	public void moveCardToHand (int cardsInHand)
	{
		StartCoroutine (AnimateMovementToHand(cardsInHand));
	}

	IEnumerator AnimateMovementToHand(int cardsInHand){
		var initialPosition = transform.position;

		for (float f = 0; f <= 1; f += 0.01f) {
			transform.position = Vector3.Lerp (initialPosition, new Vector3 (-8 + transform.localScale.x * cardsInHand , -8, 0), f);
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
