using UnityEngine;
using System.Collections;

public class MonsterField1 : MonoBehaviour {
	Card currentCard; 
	public GameObject cardPrefab;
	// Use this for initialization
	void Start () {
		//currentCard = ((GameObject)Instantiate(cardPrefab, new Vector3(), Quaternion.Euler(new Vector3()))).GetComponent<Card>();
		//currentCard = new LOB002 ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter(){
		transform.renderer.material.color = Color.blue;
		Debug.Log ("mouse enter on " + currentCard.name );
	}
	
	void OnMouseExit(){
		transform.renderer.material.color = Color.white;
		Debug.Log ("mouse exit");
	}
}
