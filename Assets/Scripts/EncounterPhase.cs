using System.Collections.Generic;
using UnityEngine;

public class EncounterPhase : IGamePhase
{
    private static readonly EncounterPhase instance = new EncounterPhase();

    private Encounter encounter;
    private bool isCreatingEncounter = false;

    private EncounterPhase() { }

    public static IGamePhase GetClean()
    {
        instance.encounter = null;
        instance.isCreatingEncounter = false;
        return instance;
    }

    public bool EncounterIs(Encounter encounter)
    {
        return (encounter != null) && encounter.Equals(this.encounter);
    }

    public void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        if (encounter != null)
        {
            encounter.CardsArrivedAt(destination, cards);
        }
        else if ((encounter == null) && (destination is StagingArea))
        {
            encounter = Encounter.From(destination.Cards);
            encounter.HappensTo(GameState.GetPlayer);
            encounter.Uses(GameState.GetDeck);
            Debug.Log($"Starting a {encounter} Encounter");
            encounter.Begin();
        }
    }

    // cards are not discarded during encounters
    public void RegisterDiscardAction(Card card) { }

    public void RegisterInteractionWith(Card card)
    {
        encounter.CardSelected(card);
    }

    public void RegisterInteractionWithDeck()
    {
        if (encounter == null)
        {
            if (!isCreatingEncounter)
            {
                isCreatingEncounter = true;
                Debug.Log("New Encounter");
                Text.TextManager.TearDownDisplayedText();
                GameState.GetDeck.DealCards(3);
            }
        }
        else
        {
            encounter.Advance();
        }
    }

    public void TearDown()
    {
        encounter?.TearDown();
    }
}
