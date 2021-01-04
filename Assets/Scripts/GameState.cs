using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton for managing overall flow of a game. */
public class GameState
{
    private static readonly GameState instance = new GameState();

    private Encounter currentEncounter;
    private Deck deck;
    private bool locked;
    private Player player;
    private StagingArea stagingArea;

    private GameState() { }

    public static void Next()
    {
        if (instance.locked) return;
        instance.locked = true;
        // IMPORTANT: remember to unlock at the end of NewCards() branches!

        if (!instance.IsPlayerAlive())
        {
            instance.player = null;
            instance.deck.DealCards(1);
            // CreatePlayer();
        }
        else if (instance.IsEncounterActive())
        {
            instance.currentEncounter.Advance();
        }
        else
        {
            // TODO: move this to a NewCards() branch
            Debug.Log("New Encounter");
            instance.deck.DealCards(instance.stagingArea, 3);
            instance.currentEncounter = Encounter.From(instance.stagingArea.Cards);
            Debug.Log("Started a " + instance.currentEncounter + " Encounter");
        }
    }

    public static void NotifyCardsReceived(CardZone cardZone, List<Card> cards)
    {
        instance.NewCards(cardZone, cards);
    }

    public static void Register(CardZone cardZone)
    {
        if (cardZone is Deck)
        {
            instance.deck = (Deck)cardZone;
        }
        else if (cardZone is StagingArea)
        {
            instance.stagingArea = (StagingArea)cardZone;
        }
    }

// private methods

    private bool IsEncounterActive()
    {
        // placeholder logic
        return currentEncounter != null;
    }

    private bool IsPlayerAlive()
    {
        return (player != null) && player.IsAlive();
    }

    private void NewCards(CardZone cardZone, List<Card> cards)
    {
        if (cardZone.Equals(stagingArea) && player == null)
        {
            if (stagingArea.Cards.Count != 1)
            {
                throw new System.Exception("Incorrect number of cards dealt for new player creation: " + stagingArea.Cards);
            }
            player = new Player(cards[0]);
            // TODO: keep dealing single cards until a suitable Character card is found
        }
        else if (cardZone.Equals(player.CharacterCard))
        {
            deck.DealCards(3);
        }
        else if (cardZone.Equals(stagingArea) && !player.IsAlive())
        {
            player.Heal(15 + CardUtil.SumValues(cards));
            deck.Accept(stagingArea.Cards);
            locked = false;
        }
        // TODO: deal the Player's starting hand
    }
}
