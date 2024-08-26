using static CardUtil;
using static Constant;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using Text;
using static Text.Excerpts.Treasure;
using static Text.TextManager;
using static Timer;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : Encounter
{
    private static Canvas leaveButtonCanvas;

    private int disarmBonus;
    private BaseExcerpt disarmBonusExcerpt;
    private int playerScore;
    private BaseExcerpt playerScoreExcerpt;
    private int trapScore;
    private BaseExcerpt trapScoreExcerpt;
    private List<Card> trapsOnChest;
    private Card trapSelectedForDisarm;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Treasure;

    public Treasure(List<Card> cards) : base(cards)
    {
        // TODO: improve acquisition of LeaveButton reference
        leaveButtonCanvas ??= GameObject.Find("LeaveButtonCanvas").GetComponent<Canvas>();
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(AbandonTreasure);
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    void AbandonTreasure()
    {
        leaveButtonCanvas.enabled = false;
        GameState.EndEncounter(this);
    }

    // Treasure progression (if not automatically resolved) is through clicking on trap cards
    // or the "leave" button; Advance() has no purpose
    public override void Advance() { }

    protected override void BeginImpl()
    {
        Debug.Log("Found a treasure");
        DisplayText(Announce);
        if (trapsOnChest.Count == 0)
        {
            Timer.DelayThenInvoke(2, DeliverTreasure);
        }
        else
        {
            Debug.Log("The chest is trapped: " + trapsOnChest.Print());
            DisplayTextAsExtension(AnnounceTrap, Announce);
            DisplayPrompts();
            UpdateDisarmBonus();
        }
    }

    public override void CardSelected(Card card)
    {
        if (trapsOnChest.Contains(card))
        {
            Debug.Log("Player clicked trap card: " + card);
            trapSelectedForDisarm = card;
            HideText(PromptPlayCards);
            HideText(PromptClickToDisarm);
            HideText(PromptAbandonEncounter);
            leaveButtonCanvas.enabled = false;
            deck.DealCards(1);
        }
        else
        {
            player.ConfigureSelectedCardOptions(card, Suit.Spade);
        }
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            if (playerScore == 0)
            { 
                playerScore = disarmBonus + SumValues(cards);
                playerScoreExcerpt = ScoreForPlayer(playerScore);
                DisplayText(playerScoreExcerpt);
                // TODO: deal the trap's score card to (1.5, -1.3, 0) - requires work on StagingArea and/or CardZone.Accept
                deck.DealCards(1);
            }
            else
            {
                trapScore = trapSelectedForDisarm.Value + SumValues(cards);
                trapScoreExcerpt = ScoreForTrap(trapScore);
                DisplayTextAsExtension(trapScoreExcerpt, playerScoreExcerpt);

                Action outcomeHandler = (playerScore, trapScore) switch
                {
                    _ when playerScore < trapScore => HandleDisarmFailure,
                    _ when playerScore == trapScore => HandleDisarmNeutral,
                    _ when playerScore > trapScore => HandleDisarmSuccess,
                    _ => () => throw new NotImplementedException() // to satisfy the compiler
                };
                DelayThenInvoke(2, () => {
                    HideText(playerScoreExcerpt, trapScoreExcerpt);
                    outcomeHandler();
                });
            }
        }
        else if (cardZone is Deck)
        {
            if (cards.Contains(trapSelectedForDisarm))
            {
                HandleTrapRemoved();
            }
            else
            {
                HideText(trapScoreExcerpt);
                HideText(playerScoreExcerpt);
                trapSelectedForDisarm = null;
                playerScore = 0;
                trapScore = 0;
                DisplayPrompts();
            }
        }
        else if (player.CardsActivated.Equals(cardZone) || player.CardsPlayed.Equals(cardZone))
        {
            UpdateDisarmBonus();
        }
    }

    public void DeliverTreasure()
    {
        Debug.Log("The treasure is claimed");
        DisplayTextAsExtension(KaChing, AnnounceTrap, Announce);
        agitator.ResetDisplayProperties();
        player.Hand.Accept(EncounterCards.Cards);
        DelayThenInvoke(1.5f, GameState.EndEncounter, this);
    }

    private static void DisplayPrompts()
    {
        DisplayText(PromptPlayCards);
        DisplayTextAsExtension(PromptClickToDisarm, 1, PromptPlayCards);
        DisplayTextAsExtension(PromptAbandonEncounter, 2, PromptClickToDisarm);
        leaveButtonCanvas.enabled = true;
    }

    private void EndDisarmAttempt(BaseExcerpt outcomeExcerpt)
    {
        DelayThenInvoke(2, () =>
        {
            Debug.Log("ending disarm attempt");
            HideText(outcomeExcerpt);
            deck.Accept(GameState.GetStagingArea.Cards);
        });
    }

    private void HandleDisarmFailure()
    {
        var failureExcerpt = DisarmFailure(trapSelectedForDisarm.Value);
        DisplayText(failureExcerpt);
        player.Damage(trapSelectedForDisarm.Value);
        EndDisarmAttempt(failureExcerpt);
    }

    private void HandleDisarmNeutral()
    {
        DisplayText(DisarmNeutralOutcome);
        EndDisarmAttempt(DisarmNeutralOutcome);
    }

    private void HandleDisarmSuccess()
    {
        DisplayText(DisarmSuccess);
        deck.Accept(trapSelectedForDisarm);
    }

    private void HandleTrapRemoved()
    {
        trapsOnChest.Remove(trapSelectedForDisarm);
        HideText(AnnounceTrap);
        if (trapsOnChest.Count == 0)
        {
            DeliverTreasure();
        }
        else
        {
            DisplayTextAsExtension(AnnounceTrapContinuation, Announce);
            EndDisarmAttempt(DisarmSuccess);
        }
    }

    private void UpdateDisarmBonus()
    {
        disarmBonus = SumValues(player.CardsPlayed.Cards);
        if (player.CardsActivated.Exists(Is(Suit.Spade, ACE)))
        {
            disarmBonus = (int)Math.Ceiling(disarmBonus * 1.5f);
        }
        if (disarmBonusExcerpt != null) HideText(disarmBonusExcerpt);
        disarmBonusExcerpt = DisarmBonus(disarmBonus);
        DisplayText(disarmBonusExcerpt);
    }
}
