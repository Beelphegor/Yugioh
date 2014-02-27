using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public CardMetadata cardMetadata;
	public delegate void clickCard(GameObject g);
	public event clickCard onClick;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		onClick (gameObject);
	}

	public void moveCardToHand (int cardsInHand)
	{
		StartCoroutine (AnimateCardToHandMovement(cardsInHand));
	}

	IEnumerator AnimateCardToHandMovement(int cardsInHand){
		var initialPosition = transform.position;

		for (float f = 0; f <= 1; f += 0.01f) {
			transform.position = Vector3.Lerp (initialPosition, new Vector3 (-8 + transform.localScale.x * cardsInHand , -8, 0), f);
			yield return null;
		}

		Debug.Log ("destination reached");
		var specificCardSprite = Resources.Load<Sprite>(cardMetadata.code);
		transform.GetComponent<SpriteRenderer>().sprite = specificCardSprite;
	}

	IEnumerator AnimateCardToMonsterZone() {

	}
}
