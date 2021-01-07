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
    private bool locked;
    private Player player;
    private StagingArea stagingArea;

    private GameState() { }

// class methods have public visibility

    public static void Next()
    {
        if (instance.locked) return;
        instance.locked = true;
        // IMPORTANT: remember to unlock at the end of NewCards() branches!

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

// instance methods are only visible to the class/instance

    private bool IsPlayerAlive()
    {
        return (player != null) && player.IsAlive();
    }

    private void NewCards(CardZone cardZone, List<Card> cards)
    {
        switch (currentPhase)
        {
            case Phase.PlayerCreation:
                NewCardsPlayerCreation(cardZone, cards);
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
        instance.currentEncounter = Encounter.From(instance.stagingArea.Cards);
        Debug.Log("Started a " + instance.currentEncounter + " Encounter");
        currentPhase = Phase.InEncounter;
        locked = false;
    }

    private void NewCardsPlayerCreation(CardZone cardZone, List<Card> cards)
    {
        Debug.Log("NCPC::" + cardZone + "::" + cards.Print());
        if (player == null)
        {
            if (!cardZone.Equals(stagingArea))
            {
                throw new System.Exception("Cards moved incorrectly before new player creation");
            }
            if (stagingArea.Cards.Count != 1)
            {
                throw new System.Exception("Incorrect number of cards dealt for new player creation: " + stagingArea.Cards);
            }
            player = new Player(cards[0]);
            // TODO: keep dealing single cards until a suitable Character card is found
        }
        else if (cardZone.Equals(player.CharacterCard))
        {
            // character card received, deal cards for HP
            deck.DealCards(3);
        }
        else if (cardZone.Equals(stagingArea) && !player.IsAlive())
        {
            // player exists but not alive => HP time
            player.Heal(15 + CardUtil.SumValues(cards));
            deck.Accept(stagingArea.Cards);
            // the below should happen after hand creation when that is implemented
            currentPhase = Phase.NewDay;
            locked = false;
        }
        // TODO: deal the Player's starting hand
    }

    private void StartGame()
    {
        currentPhase = Phase.PlayerCreation;
        player = null;
        deck.DealCards(1);
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
