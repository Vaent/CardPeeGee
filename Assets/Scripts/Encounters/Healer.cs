using ExtensionMethods;
using System.Collections.Generic;
using static System.Math;
using Text;
using static Text.Excerpts.Healer;
using static Text.TextManager;
using UnityEngine;

public class Healer : Encounter
{
    private Battle battleToResolve;
    private int feeToPay;
    private int healingAmount;
    private List<Card> potions;

    protected override JukeBox.Track ThemeMusic => (battleToResolve == null) ? JukeBox.Track.Healer : JukeBox.Track.Battle;

    public Healer(List<Card> cards) : base(cards)
    {
        healingAmount = agitator.Value
            + (int)Ceiling((float)CardUtil.SumValues(props) / 2);
        potions = props.FindAll(card => (card.Suit == Suit.Heart));
        feeToPay = CardUtil.SumValues(props, Suit.Diamond);
        List<Enemy> jailors = Enemy.FindAllIn(props);
        if (jailors.Count > 0) battleToResolve = new Battle(jailors);
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
            // TODO: if player cannot pay the fee, display a message advising of this
        }

        if (IsHealingBlocked())
        {
            // TODO: prompt input for battle/fee as required
            Timer.DelayThenInvoke(2, RemoveBlockers);
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
            // TODO: calculate how much of the fee has been paid, update display
            // TODO: deliver healing (and potions) to player if fee has been fully paid
        }
    }

    private void DeliverHealing()
    {
        HideText(TempRemoveJailors, TempRemoveFee);
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

    // temporary method to resolve Healer encounters while full encounter logic has not been delivered
    public void RemoveBlockers()
    {
        if (battleToResolve != null)
        {
            battleToResolve = null;
            DisplayText(TempRemoveJailors);
            deck.Accept(props.FindAll(card => card.Suit == Suit.Club));
            Timer.DelayThenInvoke(2, RemoveBlockers);
            return;
        }
        if (feeToPay > 0)
        {
            feeToPay = 0;
            DisplayTextAsExtension(TempRemoveFee, TempRemoveJailors);
            Timer.DelayThenInvoke(2, RemoveBlockers);
            return;
        }
        DeliverHealing();
    }
}
