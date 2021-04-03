using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton for managing overall flow of a game. */
public class GameState
{
    private static readonly GameState instance = new GameState();

    private Encounter currentEncounter;
    private Phase currentPhase;
    private Deck deck;
    private TextMesh eventText3;
    private bool locked;
    private Player player;
    private StagingArea stagingArea;

    private GameState() { }

// class methods have public visibility

    public static void Next()
    {
        if (instance.locked) return;
        instance.locked = true;
        // IMPORTANT: remember to unlock GameState when ready for new input

        if (!instance.IsPlayerAlive())
        {
            instance.StartGame();
        }
        else if (instance.currentPhase == Phase.InEncounter)
        {
            instance.currentEncounter.Advance();
        }
        else if (instance.currentPhase == Phase.NewDay)
        {
            Debug.Log("New Encounter");
            instance.deck.DealCards(instance.stagingArea, 3);
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

    public static void Register(Player player)
    {
        if (instance.IsWaitingForNewPlayer())
        {
            PlayerCreator.Close();
            instance.PlayerCreated(player);
        }
        else
        {
            throw new System.Exception("Illegal call to Register(Player)");
        }
    }

    public static void Unlock()
    {
        instance.locked = false;
    }

// instance methods are only visible to the class/instance

    private bool IsPlayerAlive()
    {
        return (player != null) && player.IsAlive();
    }

    private bool IsWaitingForNewPlayer()
    {
        return player == null && currentPhase == Phase.PlayerCreation;
    }

    private void NewCards(CardZone cardZone, List<Card> cards)
    {
        switch (currentPhase)
        {
            case Phase.PlayerCreation:
                PlayerCreator.NotifyCardsReceived(cardZone, cards);
                break;
            case Phase.NewDay:
                NewCardsNewDay(cardZone, cards);
                break;
            case Phase.InEncounter:
                NewCardsInEncounter(cardZone, cards);
                break;
            case Phase.InTown:
                NewCardsInTown(cardZone, cards);
                break;
        }
    }

    private void NewCardsInEncounter(CardZone cardZone, List<Card> cards)
    {
        // placeholder
    }

    private void NewCardsInTown(CardZone cardZone, List<Card> cards)
    {
        // placeholder
    }

    private void NewCardsNewDay(CardZone cardZone, List<Card> cards)
    {
        if (stagingArea.Equals(cardZone))
        {
            currentEncounter = Encounter.From(stagingArea.Cards);
            currentEncounter.HappensTo(player);
            Debug.Log("Starting a " + currentEncounter + " Encounter");
            currentPhase = Phase.InEncounter;
            currentEncounter.Begin();
        }
    }

    private void PlayerCreated(Player player)
    {
        this.player = player;
        this.eventText3.text = "";
        this.currentPhase = Phase.NewDay;
        this.locked = false;
    }

    private void StartGame()
    {
        currentPhase = Phase.PlayerCreation;
        player = null;
        if (eventText3 == null)
        {
            GameObject et3 = GameObject.Find("eventtext3");
            eventText3 = et3.GetComponent<TextMesh>();
        }
        var hpDisplay = GameObject.Find("hptext").GetComponent<TextMesh>();
        PlayerCreator.Initialise(deck, stagingArea, eventText3, hpDisplay);
    }

// GameState.Phase is managed internally through the currentPhase field

    private enum Phase
    {
        PlayerCreation,
        NewDay,
        InEncounter,
        InTown
    }
}
