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
            instance.deck.DealCards(3);
        }
    }

    public static void NotifyCardSelected(Card card)
    {
        if (instance.locked == false) instance.CardSelected(card);
    }

    public static void NotifyCardsReceived(CardZone cardZone, List<Card> cards)
    {
        instance.NewCards(cardZone, cards);
    }

#if UNITY_EDITOR
    #region Quick Start extension
    public static void QuickStart()
    {
        if (!instance.IsPlayerAlive())
        {
            GameObject.Find("eventtext3").GetComponent<TextMesh>().text = "";
            Player player = new Player(instance.deck.DrawCards(1)[0], GameObject.Find("hptext").GetComponent<TextMesh>());
            player.Heal(30);
            // delay so that Player.HandZone.Start() can be concluded before trying to add cards to the hand
            Timer.DelayThenInvoke(0.01f, QuickStartCallback, player);
        }
    }

    public static void QuickStartCallback(Player player)
    {
        player.AddToHand(instance.deck.DrawCards(5));
        instance.PlayerCreated(player);
    }
    #endregion
#endif

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

    private void CardSelected(Card card)
    {
        switch (currentPhase)
        {
            case Phase.InEncounter:
                currentEncounter.CardSelected(card);
                break;
            // TODO: add case for Phase.InTown
        }
    }

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
        currentEncounter.CardsArrivedAt(cardZone, cards);
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
        this.currentPhase = Phase.NewDay;
        this.locked = false;
    }

    private void StartGame()
    {
        currentPhase = Phase.PlayerCreation;
        player = null;
        GameObject.Find("eventtext3").GetComponent<TextMesh>().text = "";
        PlayerCreator.Initialise(deck, stagingArea);
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
