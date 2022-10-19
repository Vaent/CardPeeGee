using System.Collections.Generic;

public static class GameController
{
    public static void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        GameState.NotifyCardsReceived(destination, cards);
    }

    public static void RegisterDiscardAction(Card card)
    {
        GameState.CurrentPhase.RegisterDiscardAction(card);
    }

    public static void RegisterInteractionWith(Card card)
    {
        GameState.CurrentPhase.RegisterInteractionWith(card);
    }

    public static void RegisterInteractionWithDeck()
    {
        GameState.Next();
    }
}
