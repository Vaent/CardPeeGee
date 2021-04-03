using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : Encounter
{
    private List<Monster> enemies;

    public Battle(List<Card> cards) : base(cards)
    {
        JukeBox.PlayCombat();
        enemies = new List<Monster>();
        enemies.Add(new Monster(agitator, props));
    }

    // alternative constructor for battling "guards" on a Healer
    public Battle(List<Monster> enemies)
    {
        JukeBox.PlayCombat();
        // copy input list to ensure enemies can't be mutated from outside
        this.enemies = new List<Monster>(enemies);
    }

    public override void Advance()
    {
        Debug.Log("Invoked Battle.Advance()");
        // TODO: deal cards for the player and each enemy
        // TODO: calculate the winner(s) and deal damage
        // TODO: return cards to the deck
        // TODO: end the encounter if player or all enemies are dead
    }

    public override void BeginImpl()
    {
        Debug.Log("Beginning a battle against " + enemies.Print());
        // TODO: prompt to activate/play cards then click the deck
        GameState.Unlock();
    }
}
