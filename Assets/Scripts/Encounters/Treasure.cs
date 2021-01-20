using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Encounter
{
    public Treasure(List<Card> cards) : base(cards)
    {
        JukeBox.PlayTreasure();
    }

    public override void Advance()
    {

    }
}
