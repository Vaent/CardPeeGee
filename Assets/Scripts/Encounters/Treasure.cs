using ExtensionMethods;
using System.Collections.Generic;
using static Text.Treasure.TextReference;
using UnityEngine;

public class Treasure : Encounter
{
    private static readonly Color themeColor = Color.yellow;

    private List<Card> trapsOnChest;
    private Card trapSelectedForDisarm;

    protected override Color ThemeColor => themeColor;

    public Treasure(List<Card> cards) : base(cards)
    {
        JukeBox.PlayTreasure();
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    public override void Advance()
    {
        // TODO: deal score cards
    }

    protected override void BeginImpl()
    {
        Debug.Log("Found a treasure");
        Text.Treasure.DisplayFormatted(AnnounceTextOptions(), (int)Announce);
        if (trapsOnChest.Count == 0)
        {
            DeliverTreasure();
        }
        else
        {
            Debug.Log("The chest is trapped: " + trapsOnChest.Print());
            // TODO: prompt to attempt disarm or abandon the treasure
            GameState.Unlock();
        }
    }

    public override void CardSelected(Card card)
    {
        if (trapsOnChest.Contains(card))
        {
            trapSelectedForDisarm = card;
            // TODO: deal score cards
        }
        else if (PlayerCanUse(card, Suit.Spade))
        {
            player.ConfigureSelectedCardOptions(card, Suit.Spade);
        }
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            // TODO: calculate scores
            // TODO: deal damage/remove trap if applicable
            // if (trapsOnChest.Count == 0) DeliverTreasure();
            // TODO: return cards to the deck and unlock GameState if encounter is still active
            trapSelectedForDisarm = null;
        }
        else if (cardZone is Deck)
        {
            GameState.Unlock();
        }
    }

    private void DeliverTreasure()
    {
        Debug.Log("The treasure is claimed");
        // TODO: transfer agitator & props to the player's hand
        // TODO: end encounter
    }
}
