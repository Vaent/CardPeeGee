// PLACEHOLDER - see documentation/Design decisions - Gameplay flow.md
using Cards;
using System.Collections.Generic;

public interface IGamePhase
{
    public void RegisterCardsReceived(CardZone destination, List<Card> cards);

    public void RegisterDiscardAction(Card card);

    public void RegisterInteractionWith(Card card);

    public void RegisterInteractionWithDeck();
}
