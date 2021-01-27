using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Trap : Encounter
{
    private int baseDamage;
    private int trapDifficulty;

    public Trap(List<Card> cards) : base(cards)
    {
        JukeBox.PlayTrap();
        baseDamage = agitator.Value
            + CardUtil.SumValues(props, Suit.Club)
            + (int)Ceiling((float)CardUtil.SumValues(props, Suit.Heart) / 2);
        trapDifficulty = agitator.Value
            + CardUtil.SumValues(props, Suit.Spade)
            + (int)Ceiling((float)CardUtil.SumValues(props, Suit.Diamond) / 2);
        Debug.Log("Trap difficulty: " + trapDifficulty + ", damage: " + baseDamage);
        // TODO: calculate player's evasion score
        // TODO: calculate and apply amended damage
    }

    // Traps are resolved automatically - Advance() should never be called
    public override void Advance(){ }
}
