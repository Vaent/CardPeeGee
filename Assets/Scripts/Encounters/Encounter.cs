using static Constant;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter
{
    protected readonly Card agitator;
    protected Player player;
    protected readonly List<Card> props;

    // no-args constructor is only for cases where agitator/props are not used by the inheriting class
    // notable example: a Battle created for a Healer which has club props
    protected Encounter() { }

    protected Encounter(List<Card> cards)
    {
        Debug.Log("Creating encounter from " + cards.Print());
        agitator = cards[0];
        cards.RemoveAt(0);
        props = cards;
    }

    // GameState obtains Encounter instances from this factory method
    public static Encounter From(List<Card> cards)
    {
        switch (cards[0].Suit)
        {
            case Suit.Club:
                return new Battle(cards);
            case Suit.Diamond:
                return new Treasure(cards);
            case Suit.Heart:
                return new Healer(cards);
            case Suit.Spade:
                return new Trap(cards);
            default:
                // provided to satisfy the compiler - this branch should be unreachable
                throw new Exception("Card suit could not be determined");
        }
    }

    public abstract void Advance();

    public void Begin()
    {
        if (player == null) throw new Exception("Attempted to Begin encounter with no Player");
        BeginImpl();
    }

    public abstract void BeginImpl();

    public abstract void CardSelected(Card card);

    public abstract void CardsArrivedAt(CardZone cardZone, List<Card> cards);

    public void HappensTo(Player player)
    {
        this.player = player;
    }

    protected bool PlayerCanConvert(Card card, params Suit[] targetSuits)
    {
        return player.CardsActivated.Exists(activeCard =>
            activeCard != card
            && JACK.Equals(activeCard.Name)
            && ((activeCard.Suit == card.Suit) || Array.Exists(targetSuits, suit => activeCard.Suit == suit)));
    }

    protected bool PlayerCanUse(Card card, params Suit[] playableSuits)
    {
        return PlayerCanUse(card, true, playableSuits);
    }

    protected bool PlayerCanUse(Card card, bool allowActivate, params Suit[] playableSuits)
    {
        if (!player.IsHolding(card)) return false;

        return Array.Exists(playableSuits, suit => card.Suit == suit)
            || (allowActivate && player.CanActivate(card))
            || PlayerCanConvert(card, playableSuits);
    }
}
