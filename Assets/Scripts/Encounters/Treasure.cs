using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Encounter
{
    private List<Card> trapsOnChest;

    public Treasure(List<Card> cards) : base(cards)
    {
        JukeBox.PlayTreasure();
        trapsOnChest = props.FindAll(card => (card.Suit == Suit.Spade));
    }

    public override void Advance()
    {
        // TODO: determine scores for the player and selected trap,
        // deal damage or remove the trap if appropriate,
        // deliver treasure if no traps remain.
        // N.B. need a way to identify which trap was selected
        // when there is more than one trap on the chest.
    }

    public override void BeginImpl()
    {
        Debug.Log("Found a treasure");
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

    private void DeliverTreasure()
    {
        Debug.Log("The treasure is claimed");
        // TODO: transfer agitator & props to the player's hand
        // TODO: end encounter
    }
}
