using static Constant;
using ExtensionMethods;
using System.Linq;
using System;
using System.Collections.Generic;
using static System.Math;
using Text;
using static Text.Excerpts.Healer;
using static Text.TextManager;
using UnityEngine;
using UnityEngine.UI;

public class Healer : Encounter
{
    private static Canvas leaveButtonCanvas;

    private Battle battleToResolve;
    private int feeToPay;
    private int healingAmount;
    private UpdateableExcerpt<int, int> paymentStatus;
    private List<Card> potions;

    protected override JukeBox.Track ThemeMusic => (battleToResolve == null) ? JukeBox.Track.Healer : JukeBox.Track.Battle;

    public Healer(List<Card> cards) : base(cards)
    {
        // TODO: improve acquisition of LeaveButton reference e.g. using a tag (see also Treasure encounter)
        leaveButtonCanvas ??= GameObject.Find("LeaveButtonCanvas").GetComponent<Canvas>();
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(AbandonHealer);

        healingAmount = agitator.Value
            + (int)Ceiling((float)CardUtil.SumValues(props) / 2);
        potions = props.FindAll(card => (card.Suit == Suit.Heart));
        feeToPay = CardUtil.SumValues(props, Suit.Diamond);
        List<Enemy> jailors = Enemy.FindAllIn(props);
        if (jailors.Count > 0) battleToResolve = new Battle(jailors);
    }

    void AbandonHealer()
    {
        leaveButtonCanvas.GetComponentInChildren<Button>().onClick.RemoveListener(AbandonHealer);
        leaveButtonCanvas.enabled = false;
        GameState.EndEncounter(this);
    }

    public override void Advance()
    {
        if (battleToResolve != null)
        {
            battleToResolve.Advance();
        }
        else
        {
            // while paying a fee, GameState is unlocked but the encounter cannot be advanced directly
            Debug.Log("Healer.Advance() called with no effect");
        }
    }

    protected override void BeginImpl()
    {
        DisplayText(Announce);
        Excerpt<int> healAmountText = AnnounceHealAmount(healingAmount);
        DisplayTextAsExtension(healAmountText, Announce);
        var potionsText = (potions.Count > 0) ? $" and provide potions: {potions.Print()}" : "";
        Debug.Log($"Healer will heal {healingAmount} HP{potionsText}");

        if (battleToResolve != null)
        {
            Debug.Log("Need to fight jailor(s) to free the healer");
            DisplayTextAsExtension(AnnouncePrisonerStatus, healAmountText);
        }
        if (feeToPay > 0)
        {
            DisplayTextAsExtension(AnnounceFeeCharged, AnnouncePrisonerStatus, healAmountText);
            Debug.Log($"Healer charges a fee of {feeToPay}");
            // TODO: if player cannot pay the fee, display a message advising of this.
            // Need to check whether that message is only shown when there is also a jailor.
        }

        if (IsHealingBlocked())
        {
            Timer.DelayThenInvoke(2, ResolveBlockers);
        }
        else
        {
            Timer.DelayThenInvoke(2, DeliverHealing);
        }
    }

    public override void CardSelected(Card card)
    {
        if (battleToResolve != null)
        {
            battleToResolve.CardSelected(card);
        }
        else if ((feeToPay > 0) && player.IsHolding(card))
        {
            // TODO: configure and display the SelectedCardOptionsPanel
            // all suits can be played against a healer's fee (no need to check validity)
            // convert options should be included ONLY if active Aces would increase the card's effective value
            // this will require an extension/alternative to the default implementation which allows all conversions if possible
            player.ConfigureSelectedCardOptions(card, Suit.Club, Suit.Diamond, Suit.Heart, Suit.Spade);
        }
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (EncounterCards.Equals(cardZone)) return;

        if (battleToResolve != null)
        {
            battleToResolve.CardsArrivedAt(cardZone, cards);
        }
        else if (player.CardsActivated.Equals(cardZone) || player.CardsPlayed.Equals(cardZone))
        {
            List<Suit> activeAces = player.CardsActivated.Cards.FindAll(card => ACE.Equals(card.Name)).ConvertAll(card => card.Suit);
            int amountPaid = ((Suit[]) Enum.GetValues(typeof(Suit))).Aggregate(
                0,
                (runningTotal, suit) => runningTotal + (int)Math.Ceiling(
                    CardUtil.SumValues(player.CardsPlayed.Cards.FindAll(card => suit.Equals(card.Suit)))
                    * (activeAces.Contains(suit) ? 1.5 : 1)));
            updatePaymentStatus(paymentStatus, amountPaid, feeToPay - amountPaid);
            if (amountPaid >= feeToPay)
            {
                // TODO: disable leaveButton
                Timer.DelayThenInvoke(1, DeliverHealing);
            }
        }
    }

    private void DeliverHealing()
    {
        HideText(TempRemoveJailors, paymentStatus, PromptPayFee);
        BaseExcerpt healingDeliveredText = HealingIsDelivered(healingAmount);
        DisplayText(healingDeliveredText);
        player.Heal(healingAmount);
        Timer.DelayThenInvoke(healingAmount * 0.05f + 1, DeliverPotions, healingDeliveredText);
    }

    public void DeliverPotions(BaseExcerpt healingDeliveredText)
    {
        if (potions.Count > 0)
        {
            DisplayTextAsExtension(potions.Count > 1 ? PotionsAreDelivered : PotionIsDelivered, healingDeliveredText);
            player.Hand.Accept(potions);
        }
        Timer.DelayThenInvoke(2, GameState.EndEncounter, this);
    }

    private bool IsHealingBlocked()
    {
        return (battleToResolve != null) || (feeToPay > 0);
    }

    public void ResolveBlockers()
    {
        if (battleToResolve != null)
        {
            // temporary code to remove Jailors while full encounter logic has not been delivered
            battleToResolve = null;
            DisplayText(TempRemoveJailors);
            Timer.DelayThenInvoke(2, () =>
            {
                // TODO: only display PaymentRequiredPostBattle text if there is actually a fee to pay
                DisplayTextAsExtension(PaymentRequiredPostBattle, TempRemoveJailors);
                deck.Accept(props.FindAll(card => card.Suit == Suit.Club));
                Timer.DelayThenInvoke(3, ResolveBlockers);
            });
            return;
        }
        if (feeToPay > 0)
        {
            HideText(TempRemoveJailors, PaymentRequiredPostBattle);
            DisplayText(PromptPayFee);
            leaveButtonCanvas.enabled = true;
            paymentStatus = PaymentStatus(0, feeToPay);
            DisplayText(paymentStatus);
            // enable leaveButton
            return;
        }
        DeliverHealing();
    }
}
