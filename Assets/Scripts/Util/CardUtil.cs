using Cards;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardUtil
{
    private static readonly Sprite[] cardFaces = Resources.LoadAll<Sprite>("Graphics/Sprites/Card Faces/Standard");
    private static readonly Sprite[] cardFacesNonstandard = Resources.LoadAll<Sprite>("Graphics/Sprites/Card Faces");

    public static int Compare(Card a, Card b)
    {
        if (a.Suit == b.Suit)
        {
            if (a.Value == 10 && b.Value == 10)
            {
                return GetPriorityOfTenValuedCard(a) - GetPriorityOfTenValuedCard(b);
            }
            else
            {
                return a.Value - b.Value;
            }
        }
        else
        {
            return a.Suit - b.Suit;
        }
    }

    public static int CountMatches(CardZone cardZone, Suit suit)
    {
        return CountMatches(cardZone.Cards, suit);
    }

    public static int CountMatches(List<Card> cards, Suit suit)
    {
        if (cards == null || cards.Count == 0) return 0;
        var filteredList = cards.FindAll(card => (card.Suit == suit));
        return filteredList.Count;
    }

    public static Sprite GetCardFace(Suit suit, int value)
    {
        Sprite face = (value < 2) ?
            Array.Find(cardFacesNonstandard, sprite => sprite.name.StartsWith($"{suit} {value:00}")) :
            Array.Find(cardFaces, sprite => sprite.name.StartsWith($"{suit} {value:00}"));
        if (face == null) throw new ArgumentException($"No card face found matching suit: {suit} and value: {value}");
        return face;
    }

    public static int GetPriorityOfTenValuedCard(Card card)
    {
        return card.Name switch
        {
            "10" => 1,
            "Jack" => 2,
            "Queen" => 3,
            "King" => 4,
            _ => 0
        };
    }

    public static Predicate<Card> Is(Suit suit, string name)
    {
        return (Card card) => suit.Equals(card.Suit) && card.Name == name;
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

    public static int SumValues(CardZone cardZone)
    {
        return SumValues(cardZone.Cards);
    }

    public static int SumValues(CardZone cardZone, Suit suit)
    {
        return SumValues(cardZone.Cards, suit);
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
