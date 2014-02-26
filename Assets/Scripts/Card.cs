using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	public CardMetadata cardMetadata;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void moveCardToHand ()
	{
		StartCoroutine ("AnimateCardToHandMovement");
	}

	IEnumerator AnimateCardToHandMovement(){
		var initialPosition = transform.position;
		for (float f = 0f; f <= 1f; f += 0.01f) {
			transform.position = Vector3.Lerp (initialPosition, new Vector3 (-8, -8, 0), f);
			yield return null;
		}

		Debug.Log ("destination reach");
		var specificCardSprite = Resources.Load<Sprite>(cardMetadata.code);
		transform.GetComponent<SpriteRenderer>().sprite = specificCardSprite;
	}
}
