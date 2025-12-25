using static CardUtil;
using static Constant;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using Text;
using static Text.Excerpts.Treasure;
using static Text.TextManager;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : Encounter
{
    private static Canvas leaveButtonCanvas;

    private int disarmBonus;
    private BaseExcerpt disarmBonusExcerpt;
    private BaseExcerpt disarmOutcomeExcerpt;
    private int playerScore;
    private BaseExcerpt playerScoreExcerpt;
    private int trapScore;
    private BaseExcerpt trapScoreExcerpt;
    private List<Card> trapsOnChest;
    private Card trapSelectedForDisarm;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Treasure;

    public Treasure(List<Card> cards) : base(cards)
    {
        // TODO: improve acquisition of LeaveButton reference e.g. using a tag (see also Healer encounter)
        leaveButtonCanvas ??= GameObject.Find("LeaveButtonCanvas").GetComponent<Canvas>();
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(AbandonTreasure);
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    void AbandonTreasure()
    {
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.RemoveListener(AbandonTreasure);
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
            DeliverTreasure(2);
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
            // TODO: block player interactions to prevent triggering further disarm attempts while one is in progress
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
                // a card has been dealt for the player's score in a disarm attempt
                playerScore = disarmBonus + SumValues(cards);
                playerScoreExcerpt = ScoreForPlayer(playerScore);
                DisplayText(playerScoreExcerpt);
                deck.DealCardsAlternate(1);
            }
            else
            {
                // a card has been dealt for the trap's score in a disarm attempt
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
                Timer.DelayThenInvoke(2, () => {
                    HideText(playerScoreExcerpt, trapScoreExcerpt);
                    outcomeHandler();
                });
            }
        }
        else if (cardZone is Deck)
        {
            if (cards.Contains(trapSelectedForDisarm))
            {
                // a disarmed trap has been returned to the deck
                trapsOnChest.Remove(trapSelectedForDisarm);
                EndDisarmAttempt();
            }
            else
            {
                // score cards from a disarm attempt have been returned to the deck
                if (trapsOnChest.Count == 0)
                {
                    DeliverTreasure(1);
                }
                else
                {
                    // the Treasure is still trapped
                    HideText(disarmOutcomeExcerpt, AnnounceTrap);
                    DisplayTextAsExtension(AnnounceTrapContinuation, Announce);
                    trapSelectedForDisarm = null;
                    playerScore = 0;
                    trapScore = 0;
                    DisplayPrompts();
                }
            }
        }
        else if (player.CardsActivated.Equals(cardZone) || player.CardsPlayed.Equals(cardZone))
        {
            UpdateDisarmBonus();
        }
    }

    public void DeliverTreasure(int delay)
    {
        Debug.Log("The treasure is claimed");
        Timer.DelayThenInvoke(delay, () =>
        {
            HideText(disarmOutcomeExcerpt, AnnounceTrap, AnnounceTrapContinuation, Announce);
            DisplayText(KaChing);
            agitator.ResetDisplayProperties();
            player.Hand.Accept(EncounterCards.Cards);
            // TODO: check for Full Court/Elemental Union victory
            Timer.DelayThenInvoke(1.5f, GameState.EndEncounter, this);
        });
    }

    private static void DisplayPrompts()
    {
        DisplayText(PromptPlayCards);
        DisplayTextAsExtension(PromptClickToDisarm, 1, PromptPlayCards);
        DisplayTextAsExtension(PromptAbandonEncounter, 2, PromptClickToDisarm);
        leaveButtonCanvas.enabled = true;
    }

    private void EndDisarmAttempt()
    {
        Debug.Log("ending disarm attempt");
        deck.Accept(GameState.GetStagingArea.Cards);
    }

    private void HandleDisarmFailure()
    {
        SetAndDisplayOutcome(DisarmFailure(trapSelectedForDisarm.Value));
        // TODO: add dart animation(s) with appropriate delay
        Timer.DelayThenInvoke(0.5f, () =>
        {
            player.Damage(trapSelectedForDisarm.Value);
            Timer.DelayThenInvoke(0.5f, EndDisarmAttempt);
        });
    }

    private void HandleDisarmNeutral()
    {
        SetAndDisplayOutcome(DisarmNeutralOutcome);
        Timer.DelayThenInvoke(3, EndDisarmAttempt);
    }

    private void HandleDisarmSuccess()
    {
        SetAndDisplayOutcome(DisarmSuccess);
        deck.Accept(trapSelectedForDisarm);
    }

    private void SetAndDisplayOutcome(BaseExcerpt outcomeExcerpt)
    {
        disarmOutcomeExcerpt = outcomeExcerpt;
        DisplayText(disarmOutcomeExcerpt);
    }

    private void UpdateDisarmBonus()
    {
        disarmBonus = SumValues(player.CardsPlayed);
        if (player.CardsActivated.Exists(Is(Suit.Spade, ACE)))
        {
            disarmBonus = (int)Math.Ceiling(disarmBonus * 1.5f);
        }
        if (disarmBonusExcerpt != null) HideText(disarmBonusExcerpt);
        disarmBonusExcerpt = DisarmBonus(disarmBonus);
        DisplayText(disarmBonusExcerpt);
    }
}
