using ExtensionMethods;
﻿using System.Collections;
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
                throw new System.Exception("Card suit could not be determined");
        }
    }

    public abstract void Advance();

    public void Begin()
    {
        if (player == null) throw new System.Exception("Attempted to Begin encounter with no Player");
        BeginImpl();
    }

    public abstract void BeginImpl();

    public void HappensTo(Player player)
    {
        this.player = player;
    }
}
