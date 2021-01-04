using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardZone
{
    public StagingArea stagingArea;

    void Start()
    {
        Sprite[] cardFaces = Resources.LoadAll<Sprite>("Graphics/Sprites/Card Faces/Standard");
        foreach (Sprite sprite in cardFaces)
        {
            new Card(sprite, this);
        }
        GameState.Register(this);
    }

    void OnMouseDown()
    {
        GameState.Next();
    }

    public void DealCards(int count)
    {
        DealCards(stagingArea, count);
    }

    public void DealCards(CardZone target, int count)
    {
        Debug.Log("Deal " + count + " cards to " + target);
        target.Accept(DrawCards(count));
    }

    // this method will select `count` cards from the top of the deck and return them to the caller
    // the cards remain in the deck until registered to another CardZone
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = Cards.GetRange(0, count);
        return drawnCards;
    }

    protected override void ProcessNewCards(List<Card> cards)
    {
        Debug.Log("Cards were returned to the Deck: " + string.Join(" | ", cards));
        Debug.Log("Deck now contains the following: " + string.Join(" | ", Cards));
        cards.ForEach(card =>
        {
            card.MoveToFaceDown(GetComponent<Transform>().position);
            card.Hide();
        });
    }

    public void Shuffle()
    {
        // TODO: randomise the order of cardsInDeck
    }
}
