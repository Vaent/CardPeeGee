using static Constant;
using static Suit;
using System;
using System.Collections.Generic;
using static Text.Excerpts.Town;
using static Text.TextManager;
using UnityEngine;
using Text;

public class Town : IGamePhase
{
    private static readonly int charityUpperLimitExclusive = 4;
    private static readonly int costOfNewCard = 8;
    private static readonly int taxLowerLimitExclusive = 6;

    private static readonly Town instance = new Town();

    private Phase phase;

    private Town() { }

    public static IGamePhase GetClean()
    {
        JukeBox.Play(JukeBox.Track.Ambient);
        DisplayText(Announce);
        Timer.DelayThenInvoke(0.8f, instance.SetUpTaxPhase);
        return instance;
    }

    private void LeaveTown()
    {
        TearDownDisplayedText();
        GameState.PlayerLeftTown();
    }

    private void GiveCharity()
    {
        GameState.GetDeck.DealCards(GameState.GetPlayer.Hand, 1);
        Timer.DelayThenInvoke(1.5f, SetUpShopPhase);
    }

    private bool PlayerCanHeal()
    {
        if (GameState.GetPlayer.CardsActivated.Cards.Exists(card => Heart.Equals(card.Suit)))
        {
            return true;
        }
        if (GameState.GetPlayer.Hand.Cards.Exists(card => Heart.Equals(card.Suit)))
        {
            return true;
        }
        // TODO: further logic
        return false;
    }

    private bool PlayerCanShop()
    {
        int availableToSpend = 0;
        foreach (Card card in GameState.GetPlayer.CardsActivated.Cards)
        {
            availableToSpend += ValueIfPlayedOrConverted(card, Diamond);
            if (availableToSpend >= costOfNewCard) return true;
        }
        foreach (Card card in GameState.GetPlayer.Hand.Cards)
        {
            availableToSpend += ValueIfPlayedOrConverted(card, Diamond);
            if (availableToSpend >= costOfNewCard) return true;
        }
        foreach (Card card in GameState.GetPlayer.Hand.Cards)
        {
            if (JACK.Equals(card.Name) && (card.Suit != Diamond))
            {
                List<Card> matchingSuits = GameState.GetPlayer.Hand.Cards.FindAll(match => (card.Suit.Equals(match.Suit) && !match.Equals(card)));
                availableToSpend += CardUtil.SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                matchingSuits = GameState.GetPlayer.CardsActivated.Cards.FindAll(match => card.Suit.Equals(match.Suit));
                availableToSpend += CardUtil.SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                if (availableToSpend >= costOfNewCard) return true;
            }
        }
        return false;
    }

    public void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        // placeholder
    }

    public void RegisterDiscardAction(Card card)
    {
        if (phase == Phase.Tax)
        {
            GameState.GetDeck.Accept(card);
            HideText(Tax);
            Timer.DelayThenInvoke(0.5f, SetUpShopPhase);
        }
        else
        {
            Debug.LogError($"Attempted to discard {card} when tax is not expected to be paid");
        }
    }

    public void RegisterInteractionWith(Card card)
    {
        switch (phase)
        {
            case Phase.Tax:
                GameState.GetPlayer.ConfigureSelectedCardDiscard(card);
                break;
            case Phase.Heal:
            case Phase.Shop:
                // TODO: check whether the card is usable in the current context, display options if so
                break;
        }
    }

    public void RegisterInteractionWithDeck()
    {
        ResolveCurrentPhase();
    }

    private void ResolveCurrentPhase()
    {
        switch (phase)
        {
            case Phase.Shop:
                if (GameState.GetPlayer.CardsPlayed.Cards.Count > 0)
                {
                    int amountSpent = CardUtil.SumValues(GameState.GetPlayer.CardsPlayed.Cards);
                    if (GameState.GetPlayer.CardsActivated.Exists(card => ACE.Equals(card.Name) && Diamond.Equals(card.Suit)))
                    {
                        amountSpent = Mathf.CeilToInt(1.5f * amountSpent);
                    }
                    GameState.GetDeck.Accept(GameState.GetPlayer.CardsPlayed.Cards);
                    int numberOfNewCards = Mathf.FloorToInt(amountSpent / costOfNewCard);
                    Timer.DelayThenInvoke(2f, GameState.GetDeck.DealCards, numberOfNewCards);
                }
                HideText(Shopping, ShoppingIsPossible, ShoppingNotPossible, CardsCanBeActivated);
                // TODO: hide "points text"
                SetUpHealPhase();
                break;
            case Phase.Heal:
                if (GameState.GetPlayer.CardsPlayed.Cards.Count > 0)
                {
                    int healAmount = CardUtil.SumValues(GameState.GetPlayer.CardsPlayed.Cards);
                    if (GameState.GetPlayer.CardsActivated.Exists(card => ACE.Equals(card.Name) && Heart.Equals(card.Suit)))
                    {
                        healAmount = Mathf.CeilToInt(1.5f * healAmount);
                    }
                    GameState.GetPlayer.Heal(healAmount);
                    GameState.GetDeck.Accept(GameState.GetPlayer.CardsPlayed.Cards);
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
    }

    private void SetUpHealOrShopCommon(Phase targetPhase, Excerpt defaultText, Excerpt textIfPossible, Excerpt textIfNotPossible, Func<bool> playerCanDo)
    {
        phase = targetPhase;
        DisplayTextAsExtension(defaultText, Announce);
        if (GameState.GetPlayer.CanActivateAny()) DisplayText(CardsCanBeActivated);
        if (playerCanDo())
        {
            DisplayTextAsExtension(textIfPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, textIfPossible);
        }
        else
        {
            DisplayTextAsExtension(textIfNotPossible, CardsCanBeActivated);
            DisplayTextAsExtension(Continue, textIfNotPossible);
        }
    }

    private void SetUpHealPhase()
    {
        SetUpHealOrShopCommon(Phase.Heal, Healing, HealingIsPossible, HealingNotPossible, PlayerCanHeal);
    }

    private void SetUpShopPhase()
    {
        SetUpHealOrShopCommon(Phase.Shop, Shopping, ShoppingIsPossible, ShoppingNotPossible, PlayerCanShop);
    }

    private void SetUpTaxPhase()
    {
        phase = Phase.Tax;
        int cardsHeldCount = GameState.GetPlayer.CardsActivated.Cards.Count + GameState.GetPlayer.Hand.Cards.Count;
        if (cardsHeldCount < charityUpperLimitExclusive)
        {
            SetUpCharity();
        }
        else if (cardsHeldCount > taxLowerLimitExclusive)
        {
            DisplayText(Tax);
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
        else if (GameState.GetPlayer.CanConvert(card, targetSuit))
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
