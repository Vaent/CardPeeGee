using ExtensionMethods;
using System;
using System.Collections.Generic;
using static Text.Excerpts.Generic;
using static Text.TextManager;
using UnityEngine;

/* Singleton for managing overall flow of a game. */
public class GameState
{
    private static readonly GameState instance = new GameState();

    private Deck deck;
    private bool locked;
    private Player player;
    private StagingArea stagingArea;

    public static IGamePhase CurrentPhase { get; private set; } = PlayerCreator.GetClean();
    public static Deck GetDeck => instance.deck;
    public static Player GetPlayer => instance.player;

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

    public static void Next()
    {
        if (instance.locked) return;
        instance.locked = true;
        // IMPORTANT: remember to unlock GameState when ready for new input

        if (!instance.IsPlayerAlive())
        {
            instance.StartGame();
        }
        else if (CurrentPhase is EncounterPhase)
        {
            if (((EncounterPhase)CurrentPhase).encounter == null)
            {
                Debug.Log("New Encounter");
                TearDownDisplayedText();
                instance.deck.DealCards(3);
            }
            else
            {
                ((EncounterPhase)CurrentPhase).encounter.Advance();
            }
        }
        else if (CurrentPhase is Town)
        {
            Town.Advance();
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
            PlayerCreator.Close();
            instance.PlayerCreated(player);
        }
        else
        {
            throw new Exception("Illegal call to Register(Player)");
        }
    }

    public static void Unlock()
    {
        instance.locked = false;
    }

// instance methods are only visible to the class/instance

    private bool IsInEncounter(Encounter encounter)
    {
        return (CurrentPhase is EncounterPhase EP) && (encounter.Equals(EP.encounter));
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
        ((EncounterPhase)CurrentPhase).encounter.TearDown();
        deck.Accept(stagingArea.Cards);
        deck.Accept(player.CardsPlayed.Cards);
        // TODO: ensure tear down is complete & cards have been returned to the deck before continuing
        Debug.Log($"Ending {((EncounterPhase)CurrentPhase).encounter} and entering Town");
        ((EncounterPhase)CurrentPhase).encounter = null;
        CurrentPhase = Town.GetClean();
        Town.Enter(player, deck);
    }

    private void LeaveTown()
    {
        CurrentPhase = EncounterPhase.GetClean();
        DisplayText(NewEncounterPrompt);
        locked = false;
    }

    private void PlayerCreated(Player player)
    {
        this.player = player;
        CurrentPhase = EncounterPhase.GetClean();
        DisplayText(NewEncounterPrompt);
        this.locked = false;
    }

    private void StartGame()
    {
        if (player != null)
        {
            deck.Accept(stagingArea.Cards);
            ((EncounterPhase)CurrentPhase).encounter?.TearDown();
            ((EncounterPhase)CurrentPhase).encounter = null;
            deck.Accept(player.CharacterCard.Cards);
            Debug.Log("Should be destroying... " + player.Hand.Cards.Print());
            deck.Accept(player.Hand.Cards);
            deck.Accept(player.CardsActivated.Cards);
            deck.Accept(player.CardsPlayed.Cards);
            player = null;
            Timer.DelayThenInvoke(3, PlayerCreator.Initialise, deck, stagingArea);
            CurrentPhase = PlayerCreator.GetClean();
            return;
            //TODO: complete/refactor handling of dead player
            //NB Timer.Callback<T,U> was created for the rough implementation; if not subsequently used elsewhere it may be disposable
        }
        UnityEngine.Object.Destroy(GameObject.Find("eventtext3"));
        PlayerCreator.Initialise(deck, stagingArea);
        CurrentPhase = PlayerCreator.GetClean();
    }
}
