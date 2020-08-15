using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour, CardZone
{
    private List<Card> cardsInDeck;

    void Start()
    {
        cardsInDeck = new List<Card>();
        Sprite[] cardFaces = Resources.LoadAll<Sprite>("Graphics/Sprites/Card Faces/Standard");
        foreach (Sprite sprite in cardFaces)
        {
            cardsInDeck.Add(new Card(sprite));
        }
    }

    public void Accept(List<Card> cards)
    {
        cardsInDeck.AddRange(cards);
    }

    public void DealCards(CardZone target, int count)
    {
        target.Accept(DrawCards(count));
    }

    // this method will remove `count` cards from the top of the deck and return them to the caller
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = cardsInDeck.GetRange(0, count);
        cardsInDeck.RemoveRange(0, count);
        return drawnCards;
    }

    public void Shuffle()
    {
        // TODO: randomise the order of cardsInDeck
    }
}
