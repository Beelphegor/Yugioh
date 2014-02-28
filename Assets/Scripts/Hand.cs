using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public GameObject leftLimit;
    public GameObject rightLimit;
    public float leftValue;
    public float rightValue; 

	public List<Card> Cards = new List<Card>();
	// Use this for initialization
	void Start ()
	{
	    leftValue = 0;
	    rightValue = rightLimit.transform.position.x - leftLimit.transform.position.x;

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
        float distance = (float)(rightValue/Cards.Count);
        int index = 0;
        foreach (Card card in Cards)
        {
            float xPosition = leftLimit.transform.position.x + distance*index++;
            card.OrderCard(xPosition, transform.position.y);
        }
    }

	void OnMouseEnter(){
		foreach (Card card in Cards) {
			//Debug.Log ("your hand has " + card.cardMetadata.name + " cards" );
		}
	}
}
