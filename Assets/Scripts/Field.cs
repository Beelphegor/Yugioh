using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {
    //monster zones serian mas pinta como prefabs
    public GameObject MonsterZone1;
    public GameObject MonsterZone2;
    public GameObject MonsterZone3;
    public GameObject MonsterZone4;
    public GameObject MonsterZone5;
    public List<GameObject> MonsterZones;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        MonsterZones = new List<GameObject>(){
			MonsterZone1,
			MonsterZone2,
			MonsterZone3,
			MonsterZone4,
			MonsterZone5
		};
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void AddMonster(Card card)
    {
        var availableMonsterZone = MonsterZones.First(x => x.GetComponent<MonsterZone>().IsAvailable);
        availableMonsterZone.GetComponent<MonsterZone>().AddMonster(card);
    }

    public List<Card> Monsters()
    {
        var monsters = new List<Card>();
        foreach (var monsterZone in MonsterZones)
        {
            if (monsterZone.GetComponent<MonsterZone>().Monster)
            {
                monsters.Add(monsterZone.GetComponent<MonsterZone>().Monster);
            }
        }
        return monsters;
    }

    public void RemoveMonsters(Card monsterToRemove)
    {
        MonsterZones.First(x => x.GetComponent<MonsterZone>().Monster.Equals(monsterToRemove)).GetComponent<MonsterZone>().RemoveMonster();
    }
}
