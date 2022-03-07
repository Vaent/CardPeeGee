using static Constant;
using static Suit;
using System;
using System.Collections.Generic;
using static Text.Excerpts.Town;
using static Text.TextManager;
using UnityEngine;

public class Town
{
    private static readonly int charityUpperLimitExclusive = 4;
    private static readonly int costOfNewCard = 8;
    private static readonly int taxLowerLimitExclusive = 6;

    private static readonly Town instance = new Town();

    private Deck deck;
    private Phase phase;
    private Player player;

    private Town() { }

    public static void Advance()
    {
        instance.ResolveCurrentPhase();
    }

    public static void CardSelected(Card card)
    {
        switch (instance.phase)
        {
            case Phase.Tax:
                instance.player.ConfigureSelectedCardDiscard(card);
                break;
            case Phase.Heal:
            case Phase.Shop:
                // TODO: check whether the card is usable in the current context, display options if so
                break;
        }
    }

    public static void Enter(Player player, Deck deck)
    {
        instance.player = player;
        instance.deck = deck;
        JukeBox.Play(JukeBox.Track.Ambient);
        DisplayText(Announce);
        Timer.DelayThenInvoke(0.8f, instance.SetUpTaxPhase);
    }

    private void LeaveTown()
    {
        player = null;
        TearDownDisplayedText();
        GameState.PlayerLeftTown();
    }

    private void GiveCharity()
    {
        deck.DealCards(player.Hand, 1);
        Timer.DelayThenInvoke(1.5f, SetUpShopPhase);
    }

    private bool PlayerCanHeal()
    {
        if (player.CardsActivated.Cards.Exists(card => Heart.Equals(card.Suit)))
        {
            return true;
        }
        if (player.Hand.Cards.Exists(card => Heart.Equals(card.Suit)))
        {
            return true;
        }
        // TODO: further logic
        return false;
    }

    private bool PlayerCanShop()
    {
        int availableToSpend = 0;
        foreach (Card card in player.CardsActivated.Cards)
        {
            availableToSpend += ValueIfPlayedOrConverted(card, Diamond);
            if (availableToSpend >= costOfNewCard) return true;
        }
        foreach (Card card in player.Hand.Cards)
        {
            availableToSpend += ValueIfPlayedOrConverted(card, Diamond);
            if (availableToSpend >= costOfNewCard) return true;
        }
        foreach (Card card in player.Hand.Cards)
        {
            if (JACK.Equals(card.Name) && (card.Suit != Diamond))
            {
                List<Card> matchingSuits = player.Hand.Cards.FindAll(match => (card.Suit.Equals(match.Suit) && !match.Equals(card)));
                availableToSpend += CardUtil.SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                matchingSuits = player.CardsActivated.Cards.FindAll(match => card.Suit.Equals(match.Suit));
                availableToSpend += CardUtil.SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                if (availableToSpend >= costOfNewCard) return true;
            }
        }
        return false;
    }

    public static void PlayerHasDiscarded(Card card)
    {
        if (!instance.deck.Equals(card.CurrentLocation))
        {
            Debug.LogError($"{card} was reported as a discard but has not been returned to the deck");
        }
        else if (instance.phase != Phase.Tax)
        {
            Debug.LogError($"{card} was discarded when tax is not expected to be paid");
        }
        else
        {
            HideText(Tax);
            Timer.DelayThenInvoke(0.5f, instance.SetUpShopPhase);
        }
    }

    private void ResolveCurrentPhase()
    {
        switch (phase)
        {
            case Phase.Shop:
                if (player.CardsPlayed.Cards.Count > 0)
                {
                    int amountSpent = CardUtil.SumValues(player.CardsPlayed.Cards);
                    if (player.CardsActivated.Exists(card => ACE.Equals(card.Name) && Diamond.Equals(card.Suit)))
                    {
                        amountSpent = Mathf.CeilToInt(1.5f * amountSpent);
                    }
                    deck.Accept(player.CardsPlayed.Cards);
                    int numberOfNewCards = Mathf.FloorToInt(amountSpent / costOfNewCard);
                    Timer.DelayThenInvoke(2f, deck.DealCards, numberOfNewCards);
                }
                HideText(Shopping, ShoppingIsPossible, ShoppingNotPossible, CardsCanBeActivated);
                // TODO: hide "points text"
                SetUpHealPhase();
                break;
            case Phase.Heal:
                if (player.CardsPlayed.Cards.Count > 0)
                {
                    int healAmount = CardUtil.SumValues(player.CardsPlayed.Cards);
                    if (player.CardsActivated.Exists(card => ACE.Equals(card.Name) && Heart.Equals(card.Suit)))
                    {
                        healAmount = Mathf.CeilToInt(1.5f * healAmount);
                    }
                    player.Heal(healAmount);
                    deck.Accept(player.CardsPlayed.Cards);
                }
                LeaveTown();
                break;
            default:
                throw new Exception("Town.Phase value not recognised");
        }
    }

    private void SetUpCharity()
    {
        DisplayText(Charity);
        Timer.DelayThenInvoke(0.5f, GiveCharity);
        GameState.Unlock();
    }

    private void SetUpHealPhase()
    {
        instance.phase = Phase.Heal;
        DisplayTextAsExtension(Healing, Announce);
        if (player.CanActivateAny()) DisplayText(CardsCanBeActivated);
        if (PlayerCanHeal())
        {
            DisplayTextAsExtension(HealingIsPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, HealingIsPossible);
        }
        else
        {
            DisplayTextAsExtension(HealingNotPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, HealingNotPossible);
        }
        GameState.Unlock();
    }

    private void SetUpShopPhase()
    {
        instance.phase = Phase.Shop;
        DisplayTextAsExtension(Shopping, Announce);
        if (player.CanActivateAny()) DisplayText(CardsCanBeActivated);
        if (PlayerCanShop())
        {
            DisplayTextAsExtension(ShoppingIsPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, ShoppingIsPossible);
        }
        else
        {
            DisplayTextAsExtension(ShoppingNotPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, ShoppingNotPossible);
        }
        GameState.Unlock();
    }

    private void SetUpTaxPhase()
    {
        instance.phase = Phase.Tax;
        int cardsHeldCount = player.CardsActivated.Cards.Count + player.Hand.Cards.Count;
        if (cardsHeldCount < charityUpperLimitExclusive)
        {
            SetUpCharity();
        }
        else if (cardsHeldCount > taxLowerLimitExclusive)
        {
            DisplayText(Tax);
            GameState.Unlock();
        }
        else
        {
            SetUpShopPhase();
        }
    }

    private int ValueIfPlayedOrConverted(Card card, Suit targetSuit)
    {
        if (targetSuit.Equals(card.Suit))
        {
            return card.Value;
        }
        else if (player.CanConvert(card, targetSuit))
        {
            return Mathf.Max(0, (card.Value - Card.conversionPenalty));
        }

        return 0;
    }

    private enum Phase
    {
        Tax,
        Shop,
        Heal
    }
}
