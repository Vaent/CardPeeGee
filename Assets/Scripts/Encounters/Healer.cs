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
        potions = props.FindAll(card => (card.Suit == Suit.Heart));
        feeToPay = CardUtil.SumValues(props, Suit.Diamond);
        List<Monster> jailors = Monster.FindAllIn(props);
        if (jailors.Count > 0) battleToResolve = new Battle(jailors);
    }

    public override void Advance()
    {
        // TODO: resolve battle if applicable
        // TODO: resolve fee if applicable and no battle
        // TODO: deliver healing (and potions) to player if no battle/fee
    }

    public override void BeginImpl()
    {
        if (battleToResolve != null)
        {
            Debug.Log("Need to fight jailor(s) to free the healer");
        }
        if (feeToPay > 0)
        {
            Debug.Log("Healer charges a fee of " + feeToPay);
        }
        var healerEffects = "Healer will heal " + healingAmount + "HP";
        if (potions.Count > 0)
        {
            healerEffects += (" and provide potions: " + potions.Print());
        }
        Debug.Log(healerEffects);

        if (IsHealingBlocked())
        {
            // TODO: prompt input for battle/fee as required
            GameState.Unlock();
        }
        else
        {
            // TODO: deliver healing (and potions) to player
        }
    }

    private bool IsHealingBlocked()
    {
        return (battleToResolve != null) || (feeToPay > 0);
    }
}
