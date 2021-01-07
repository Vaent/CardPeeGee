using System.Collections.Generic;
using UnityEngine;

public class CardUtil
{
    private static readonly Sprite[] cardFaces = Resources.LoadAll<Sprite>("Graphics/Sprites/Card Faces/Standard");

    public static int Compare(Card a, Card b)
    {
        if (a.Suit == b.Suit)
        {
            return a.Value - b.Value;
        }
        else
        {
            return a.Suit - b.Suit;
        }
    }

    public static int CountValues(List<Card> cards, Suit suit)
    {
        if (cards == null || cards.Count == 0) return 0;
        var filteredList = cards.FindAll(card => (card.Suit == suit));
        return filteredList.Count;
    }

    public static void NewPack(CardZone startingLocation)
    {
        List<Card> pack = new List<Card>();
        foreach (Sprite sprite in cardFaces)
        {
            pack.Add(new Card(sprite, startingLocation));
        }
        startingLocation.Accept(pack);
    }

    public static void Sort(List<Card> cards)
    {
        cards.Sort(Compare);
    }

    public static int SumValues(List<Card> cards)
    {
        var i = 0;
        foreach (Card card in cards)
        {
            i += card.Value;
        }
        return i;
    }

    public static int SumValues(List<Card> cards, Suit suit)
    {
        if (cards == null || cards.Count == 0) return 0;
        var filteredList = cards.FindAll(card => (card.Suit == suit));
        return SumValues(filteredList);
    }
}
