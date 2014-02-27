using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

	public GameObject hand;
	public GameObject deck;
	public GameObject field;
	public GameObject monsterZone1;
	public GameObject monsterZone2;
	public GameObject monsterZone3;
	public GameObject monsterZone4;
	public GameObject monsterZone5;
	public List<GameObject> monsterZones;

	// Use this for initialization
	void Start () {
		monsterZones = new List<GameObject>(){
			monsterZone1,
			monsterZone2,
			monsterZone3,
			monsterZone4,
			monsterZone5
		};
		deck.GetComponent<Deck> ().InitializeWithRandomCards ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("d")){
			Card card = deck.GetComponent<Deck>().Draw();
			card.onClick += OnClickHandCard;
			card.moveCardToHand(hand.GetComponent<Hand>().Cards.Count);
			hand.GetComponent<Hand>().Cards.Add (card);
		}
	}

	void OnClickHandCard(Card card){
		var availableMonsterZone = monsterZones.Where (x => x.GetComponent<MonsterZone> ().isAvailable).First ();
		availableMonsterZone.GetComponent<MonsterZone>().isAvailable = false;
		card.moveCardToMonsterZone (availableMonsterZone.transform.position);
		Debug.Log ("click en: " + card.cardMetadata.name);
	}
}
