using System.Collections.Generic;

public class Cards
{
    public static int CountValues(List<Card> cards, Suit suit)
    {
        if (cards == null || cards.Count == 0) return 0;
        var filteredList = cards.FindAll(card => (card.Suit == suit));
        return filteredList.Count;
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
