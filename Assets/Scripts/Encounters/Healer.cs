using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using static System.Convert;
using static System.Math;
using UnityEngine;

public class Healer : Encounter
{
    private int healingAmount;
    private List<Card> potions;
    private Battle battleToResolve;
    private int feeToPay;

    public Healer(List<Card> cards) : base(cards)
    {
        JukeBox.PlayHealer();
        healingAmount = agitator.Value
            + (int)Ceiling((float)CardUtil.SumValues(props) / 2);
        Debug.Log("Healer will heal " + healingAmount + "HP");
        potions = props.FindAll(card => (card.Suit == Suit.Heart));
        if (potions.Count > 0)
        {
            Debug.Log("Healer will provide potions: " + potions.Print());
        }
        feeToPay = CardUtil.SumValues(props, Suit.Diamond);
        if (feeToPay > 0)
        {
            Debug.Log("Healer charges a fee of " + feeToPay);
        }
        List<Monster> jailors = Monster.FindAllIn(props);
        if (jailors.Count > 0)
        {
            Debug.Log("Need to fight " + jailors.Print() + " to free the healer");
            battleToResolve = new Battle(jailors);
        }
    }

    public override void Advance()
    {
        // TODO: resolve battle if applicable
        // TODO: resolve fee if applicable and no battle
        // TODO: deliver healing (and potions) to player if no battle/fee
    }
}
