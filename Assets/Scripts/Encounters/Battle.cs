using ExtensionMethods;
using System.Collections.Generic;
using static Text.Battle.TextReference;
using UnityEngine;

public class Battle : Encounter
{
    private static readonly Color themeColor = Color.green;

    private bool battleHasStarted;
    private List<Monster> enemies;
    private bool isHealerBattle;

    protected override Color ThemeColor => themeColor;

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
        isHealerBattle = true;
        // copy input list to ensure enemies can't be mutated from outside
        this.enemies = new List<Monster>(enemies);
    }

    public override void Advance()
    {
        Debug.Log("Invoked Battle.Advance()");
        if (!battleHasStarted) battleHasStarted = true;
        // TODO: check for played cards and apply effects
        // TODO: deal cards for the player and each enemy
        // (N.B. deals are staggered in the original - can implement this with tracking and recursion in CardsArrivedAt)
    }

    protected override void BeginImpl()
    {
        Debug.Log("Beginning a battle against " + enemies.Print());
        Text.Battle.DisplayFormatted(StrongTextOptions(), (int)Announce);
        // TODO: prompt to activate/play cards then click the deck
        GameState.Unlock();
    }

    public override void CardSelected(Card card)
    {
        if ((battleHasStarted && !isHealerBattle) || !PlayerCanUse(card, Suit.Club, Suit.Spade)) return;
        player.ConfigureSelectedCardOptions(card, Suit.Club, Suit.Spade);
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            // TODO: calculate scores, deal damage if applicable
            // TODO: end the encounter if all enemies are dead
            // TODO: return cards to the deck and unlock GameState if encounter is still active
        }
        else if (cardZone is Deck)
        {
            GameState.Unlock();
        }
    }
}
