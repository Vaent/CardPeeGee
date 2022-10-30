using ExtensionMethods;
using System.Collections.Generic;
using static Text.Excerpts.Treasure;
using static Text.TextManager;
using UnityEngine;

public class Treasure : Encounter
{
    private List<Card> trapsOnChest;
    private Card trapSelectedForDisarm;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Treasure;

    public Treasure(List<Card> cards) : base(cards)
    {
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    public override void Advance()
    {
        // TODO: deal score cards
    }

    protected override void BeginImpl()
    {
        Debug.Log("Found a treasure");
        DisplayText(Announce);
        if (trapsOnChest.Count == 0)
        {
            DeliverTreasure();
        }
        else
        {
            Debug.Log("The chest is trapped: " + trapsOnChest.Print());
            // TODO: prompt to attempt disarm or abandon the treasure
        }
    }

    public override void CardSelected(Card card)
    {
        if (trapsOnChest.Contains(card))
        {
            trapSelectedForDisarm = card;
            // TODO: deal score cards
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
            // TODO: calculate scores
            // TODO: deal damage/remove trap if applicable
            // if (trapsOnChest.Count == 0) DeliverTreasure();
            // TODO: return cards to the deck if encounter is still active
            trapSelectedForDisarm = null;
        }
    }

    private void DeliverTreasure()
    {
        Debug.Log("The treasure is claimed");
        // TODO: transfer agitator & props to the player's hand
        // TODO: end encounter
    }
}
