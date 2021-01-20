using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Encounter
{
    public Healer(List<Card> cards) : base(cards)
    {
        JukeBox.PlayHealer();
    }

    public override void Advance()
    {

    }
}
