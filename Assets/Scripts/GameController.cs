﻿using System.Collections.Generic;

public static class GameController
{
    public static void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        GameState.NotifyCardsReceived(destination, cards);
    }

    public static void RegisterDiscardAction(Card card)
    {
        Town.PlayerHasDiscarded(card);
    }

    public static void RegisterInteractionWithCard(Card card)
    {
        GameState.NotifyCardSelected(card);
    }

    public static void RegisterInteractionWithDeck()
    {
        GameState.Next();
    }
}
