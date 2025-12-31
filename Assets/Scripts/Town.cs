using static CardUtil;
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

    private bool isAcceptingInput = false;
    private Phase phase;
    private UpdateableExcerpt<string, string> pointsTextShopping;
    private UpdateableExcerpt<int> pointsTextHealing;

    private Town() { }

    public static IGamePhase GetClean()
    {
        instance.phase = Phase.Tax;
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

    private static int CalculatePlayedCardPoints(bool pointsAreBoosted)
    {
        int basePoints = SumValues(GameState.GetPlayer.CardsPlayed);
        return pointsAreBoosted ? Mathf.CeilToInt(1.5f * basePoints) : basePoints;
    }

    private void GiveCharity()
    {
        GameState.GetDeck.DealCards(GameState.GetPlayer.Hand, 1);
    }

    private bool PlayerCanHeal()
    {
        List<Card> usableCards = new List<Card>(GameState.GetPlayer.CardsActivated.Cards);
        usableCards.AddRange(GameState.GetPlayer.Hand.Cards);
        // Any Heart card of any value can be used to heal
        if (usableCards.Exists(card => Heart.Equals(card.Suit)))
        {
            return true;
        }
        // Check for non-Heart cards which can be converted by a Jack of their own suit and still provide at least 1 point of value
        Suit[] convertibleSuits = { Club, Diamond, Spade };
        foreach (Suit suit in convertibleSuits)
        {
            if (usableCards.Exists(card => suit.Equals(card.Suit) && JACK.Equals(card.Name))
                && usableCards.Exists(card => suit.Equals(card.Suit) && !JACK.Equals(card.Name) && card.NaturalValue > Card.conversionPenalty))
            {
                return true;
            }
        }
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
                availableToSpend += SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                matchingSuits = GameState.GetPlayer.CardsActivated.Cards.FindAll(match => card.Suit.Equals(match.Suit));
                availableToSpend += SumValues(matchingSuits);
                availableToSpend -= (2 * matchingSuits.Count); // conversion penalty

                if (availableToSpend >= costOfNewCard) return true;
            }
        }
        return false;
    }

    public void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        if (GameState.GetPlayer.Hand.Equals(destination))
        {
            isAcceptingInput = true;
            switch (phase)
            {
                case Phase.Tax:
                    HideText(Charity);
                    SetUpShopPhase();
                    break;
                case Phase.Shop:
                    SetUpHealPhase();
                    break;
            }
        }
        else if (GameState.GetPlayer.CardsPlayed.Equals(destination) || GameState.GetPlayer.CardsActivated.Equals(destination))
        {
            if (phase == Phase.Shop)
            {
                bool pointsAreBoosted = GameState.GetPlayer.CardsActivated.Exists(Is(Diamond, ACE));
                int pointsPlayed = CalculatePlayedCardPoints(GameState.GetPlayer.CardsActivated.Exists(Is(Diamond, ACE)));
                int cardsPurchased = Mathf.FloorToInt(pointsPlayed / costOfNewCard);
                updateShoppingPoints(pointsTextShopping, pointsPlayed, cardsPurchased);
                if (pointsAreBoosted)
                {
                    DisplayTextAsExtension(ShoppingPointsBoosted, pointsTextShopping);
                }
                else
                {
                    HideText(ShoppingPointsBoosted);
                }
            }
            else if (phase == Phase.Heal)
            {
                bool pointsAreBoosted = GameState.GetPlayer.CardsActivated.Cards.Exists(Is(Heart, ACE));
                int pointsPlayed = CalculatePlayedCardPoints(GameState.GetPlayer.CardsActivated.Exists(Is(Heart, ACE)));
                updateHealingPoints(pointsTextHealing, pointsPlayed);
                if (pointsAreBoosted)
                {
                    DisplayTextAsExtension(HealingPointsBoosted, pointsTextHealing);
                }
                else
                {
                    HideText(HealingPointsBoosted);
                }
            }
        }
    }

    public void RegisterDiscardAction(Card card)
    {
        if (phase == Phase.Tax)
        {
            isAcceptingInput = false;
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
        if (!isAcceptingInput) return;

        switch (phase)
        {
            case Phase.Tax:
                GameState.GetPlayer.ConfigureSelectedCardDiscard(card);
                break;
            case Phase.Shop:
                GameState.GetPlayer.ConfigureSelectedCardOptions(card, Diamond);
                break;
            case Phase.Heal:
                GameState.GetPlayer.ConfigureSelectedCardOptions(card, Heart);
                break;
        }
    }

    public void RegisterInteractionWithDeck()
    {
        if (isAcceptingInput) ResolveCurrentPhase();
    }

    private void ResolveCurrentPhase()
    {
        switch (phase)
        {
            case Phase.Shop:
                isAcceptingInput = false;
                HideText(Shopping, ShoppingIsPossible, ShoppingNotPossible, pointsTextShopping, ShoppingPointsBoosted, CardsCanBeActivated, Continue);
                if (GameState.GetPlayer.CardsPlayed.Cards.Count > 0)
                {
                    int amountSpent = CalculatePlayedCardPoints(GameState.GetPlayer.CardsActivated.Exists(Is(Diamond, ACE)));
                    GameState.GetDeck.Accept(GameState.GetPlayer.CardsPlayed.Cards);
                    int numberOfNewCards = Mathf.FloorToInt(amountSpent / costOfNewCard);
                    if (numberOfNewCards > 0)
                    {
                        Timer.DelayThenInvoke(1f, GameState.GetDeck.DealCards, GameState.GetPlayer.Hand, numberOfNewCards);
                    }
                    else
                    {
                        Timer.DelayThenInvoke(1f, SetUpHealPhase);
                    }
                }
                else
                {
                    SetUpHealPhase();
                }
                break;
            case Phase.Heal:
                isAcceptingInput = false;
                if (GameState.GetPlayer.CardsPlayed.Cards.Count > 0)
                {
                    int healAmount = CalculatePlayedCardPoints(GameState.GetPlayer.CardsActivated.Exists(Is(Heart, ACE)));
                    GameState.GetPlayer.Heal(healAmount);
                    HideText(pointsTextHealing, HealingPointsBoosted);
                    GameState.GetDeck.Accept(GameState.GetPlayer.CardsPlayed.Cards);
                }
                LeaveTown();
                break;
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
        isAcceptingInput = true;
    }

    private void SetUpHealPhase()
    {
        SetUpHealOrShopCommon(Phase.Heal, Healing, HealingIsPossible, HealingNotPossible, PlayerCanHeal);
        pointsTextHealing = HealingPoints(0);
        DisplayText(pointsTextHealing);
        if (GameState.GetPlayer.CardsActivated.Exists(Is(Heart, ACE))) DisplayTextAsExtension(HealingPointsBoosted, pointsTextHealing);
    }

    private void SetUpShopPhase()
    {
        SetUpHealOrShopCommon(Phase.Shop, Shopping, ShoppingIsPossible, ShoppingNotPossible, PlayerCanShop);
        pointsTextShopping = ShoppingPoints(0, 0);
        DisplayText(pointsTextShopping);
        if (GameState.GetPlayer.CardsActivated.Exists(Is(Diamond, ACE))) DisplayTextAsExtension(ShoppingPointsBoosted, pointsTextShopping);
    }

    private void SetUpTaxPhase()
    {
        int cardsHeldCount = GameState.GetPlayer.CardsActivated.Cards.Count + GameState.GetPlayer.Hand.Cards.Count;
        if (cardsHeldCount < charityUpperLimitExclusive)
        {
            SetUpCharity();
        }
        else if (cardsHeldCount > taxLowerLimitExclusive)
        {
            DisplayText(Tax);
            isAcceptingInput = true;
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
