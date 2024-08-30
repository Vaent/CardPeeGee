using ExtensionMethods;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter
{
    protected readonly Card agitator;
    protected Deck deck;
    private readonly EncounterCardZone encounterCardZone = new GameObject("EncounterCardZone").AddComponent<EncounterCardZone>();
    protected Player player;
    protected readonly List<Card> props;

    protected CardZone EncounterCards => encounterCardZone;
    protected abstract JukeBox.Track ThemeMusic { get; }

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
        return cards[0].Suit switch
        {
            Suit.Club => new Battle(cards),
            Suit.Diamond => new Treasure(cards),
            Suit.Heart => new Healer(cards),
            Suit.Spade => new Trap(cards),
            _ => throw new Exception("Card suit could not be determined") // may need to handle Joker in future
        };
    }

    public abstract void Advance();

    public void Begin()
    {
        if (player == null) throw new Exception("Attempted to Begin encounter with no Player");
        JukeBox.Play(ThemeMusic);
        // TODO: display encounter art/animation (may need to be delegated to BeginImpl)
        var encounterCards = new List<Card>(props);
        encounterCards.Insert(0, agitator);
        encounterCardZone.Accept(encounterCards);
        BeginImpl();
    }

    protected abstract void BeginImpl();

    public abstract void CardSelected(Card card);

    public abstract void CardsArrivedAt(CardZone cardZone, List<Card> cards);

    public void HappensTo(Player player)
    {
        this.player = player;
    }

    public void TearDown()
    {
        Text.TextManager.TearDownDisplayedText();

        deck.Accept(encounterCardZone.Cards);
        Timer.DelayThenInvoke(5, UnityEngine.Object.Destroy, encounterCardZone.gameObject);
        // TODO: refactor to reuse encounterCardZone instead of destroying it
    }

    public void Uses(Deck deck)
    {
        this.deck = deck;
    }

    private class EncounterCardZone : CardZone
    {
        private static readonly Vector3 anchor = new Vector3(6.5f, 3.7f, 0);

        protected override void ProcessNewCards(List<Card> newCards)
        {
            var cardsList = Cards;
            cardsList[0].Resize(1.2f);
            cardsList[0].MoveTo(anchor + (0.1f * Vector3.left), cardsInMotion[cardsList[0]]);
            for (int i = 1; i < cardsList.Count; i++)
            {
                cardsList[i].MoveTo(anchor + (i * Vector3.right), cardsInMotion[cardsList[i]]);
            }
        }
    }
}
