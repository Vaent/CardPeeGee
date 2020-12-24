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

        if (!instance.IsPlayerAlive())
        {
            CreatePlayer();
        }
        else if (instance.IsEncounterActive())
        {
            instance.currentEncounter.Advance();
        }
        else
        {
            Debug.Log("New Encounter");
            instance.deck.DealCards(instance.stagingArea, 3);
            instance.currentEncounter = Encounter.From(instance.stagingArea.Cards);
            Debug.Log("Started a " + instance.currentEncounter + " Encounter");
        }

        instance.locked = false;
    }

    public static void HarmPlayer(int amount)
    {
        if (instance.IsPlayerAlive()) instance.player.Damage(amount);
    }

    public static void HealPlayer(int amount)
    {
        if (instance.IsPlayerAlive()) instance.player.Heal(amount);
    }

    public bool IsEncounterActive()
    {
        // placeholder logic
        return instance.currentEncounter != null;
    }

    public bool IsPlayerAlive()
    {
        return (instance.player != null) && instance.player.IsAlive();
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

    private static void CreatePlayer()
    {
        instance.deck.DealCards(instance.stagingArea, 1);
        // TODO: keep dealing single cards until a suitable Character card is found
        instance.player = new Player(instance.stagingArea.Cards[0]);
        instance.deck.DealCards(instance.stagingArea, 3);
        instance.player.Heal(15 + Cards.SumValues(instance.stagingArea.Cards));
        instance.deck.Accept(instance.stagingArea.Cards);
        // TODO: deal the Player's starting hand
    }
}
