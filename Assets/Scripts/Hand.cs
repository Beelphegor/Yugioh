using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public GameObject leftLimit;
    public GameObject rightLimit;
    public float spaceForCards; 

	public List<Card> Cards = new List<Card>();
	// Use this for initialization
    void Awake()
    {
        spaceForCards = rightLimit.transform.position.x - leftLimit.transform.position.x;
    }

	void Start ()
	{
	    

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
        OrderHand();
    }
    public void AddCard(Card card)
    {
        Cards.Add(card);
        OrderHand();
    }

    public void OrderHand()
    {
        float distance = spaceForCards / Cards.Count;
        int index = 0;
        foreach (Card card in Cards)
        {
            float xPosition = leftLimit.transform.position.x + distance*index;

            card.OrderCard(xPosition, transform.position.y);
            index++;
        }
    }
}
