using UnityEngine;
using System.Collections;

public class MonsterZone : MonoBehaviour {
	public bool IsAvailable;

	void Awake(){
        IsAvailable = true;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddMonster(Card card)
    {
        Monster = card;
        IsAvailable = false;
        card.moveCardToMonsterZone(transform.position);
        card.isOnMonsterZone = true;
    }

    public Card Monster { get; set; }

    public void RemoveMonster()
    {
        IsAvailable = true;
        Monster = null;
    }
}
