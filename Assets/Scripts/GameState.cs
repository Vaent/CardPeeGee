using ExtensionMethods;
using System;
using static Text.Excerpts.Generic;
using static Text.TextManager;
using UnityEngine;

public class GameState
{
    private static readonly GameState instance = new GameState();

    private Deck deck;
    private Player player;
    private StagingArea stagingArea;

    public static IGamePhase CurrentPhase { get; private set; } = PlayerCreator.GetClean();
    public static Deck GetDeck => instance.deck;
    public static Player GetPlayer => instance.player;
    public static StagingArea GetStagingArea => instance.stagingArea;

    private GameState() { }

// class methods have public visibility

    public static void EndEncounter(Encounter encounter)
    {
        if (instance.IsInEncounter(encounter))
        {
            instance.LeaveEncounter();
        }
        else
        {
            Debug.LogWarning("Attempted to end an Encounter which is not currently active!");
        }
    }

    public static void PlayerDied()
    {
        if (instance.player?.IsAlive() == false)
        {
            ((EncounterPhase)CurrentPhase).TearDown();
            CurrentPhase = PlayerCreator.GetClean();
        }
    }

    public static void PlayerLeftTown()
    {
        if (CurrentPhase is Town)
        {
            instance.LeaveTown();
        }
        else
        {
            Debug.LogWarning("Attempted to exit town when not currently in town!");
        }
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
            instance.PlayerCreated(player);
        }
        else
        {
            throw new Exception("Illegal call to Register(Player)");
        }
    }

    public static void Restart()
    {
        instance.StartNewGame();
    }

    // instance methods are only visible to the class/instance

    private bool IsInEncounter(Encounter encounter)
    {
        return (CurrentPhase is EncounterPhase EP) && EP.EncounterIs(encounter);
    }

    private bool IsPlayerAlive()
    {
        return (player != null) && player.IsAlive();
    }

    private bool IsWaitingForNewPlayer()
    {
        return player == null && (CurrentPhase is PlayerCreator);
    }

    private void LeaveEncounter()
    {
        ((EncounterPhase)CurrentPhase).TearDown();
        deck.Accept(stagingArea.Cards);
        deck.Accept(player.CardsPlayed.Cards);
        // TODO: ensure tear down is complete & cards have been returned to the deck before continuing
        Debug.Log($"Ending Encounter and entering Town");
        CurrentPhase = Town.GetClean();
        Town.Enter(player, deck);
    }

    private void LeaveTown()
    {
        CurrentPhase = EncounterPhase.GetClean();
        DisplayText(NewEncounterPrompt);
    }

    private void PlayerCreated(Player player)
    {
        this.player = player;
        CurrentPhase = EncounterPhase.GetClean();
        DisplayText(NewEncounterPrompt);
    }

    private void StartNewGame()
    {
        deck.Accept(stagingArea.Cards);
        deck.Accept(player.CharacterCard.Cards);
        Debug.Log("Should be destroying... " + player.Hand.Cards.Print());
        deck.Accept(player.Hand.Cards);
        deck.Accept(player.CardsActivated.Cards);
        deck.Accept(player.CardsPlayed.Cards);
        player = null;
        Timer.DelayThenInvoke(3, ((PlayerCreator)CurrentPhase).BeginCreating);
        return;
        //TODO: complete/refactor handling of dead player
    }
}
