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
        Debug.Log("Beginning a battle against " + enemies.Print());
    }

    // alternative constructor for battling "guards" on a Healer
    public Battle(List<Monster> enemies)
    {
        JukeBox.PlayCombat();
        // copy input list to ensure enemies can't be mutated from outside
        this.enemies = new List<Monster>(enemies);
        Debug.Log("Battle created versus " + enemies.Print());
    }

    public override void Advance()
    {
        Debug.Log("Invoked Battle.Advance()");
        // TODO: deal cards for the player and each enemy
        // TODO: calculate the winner(s) and deal damage
        // TODO: return cards to the deck
        // TODO: end the encounter if player or all enemies are dead
    }
}
