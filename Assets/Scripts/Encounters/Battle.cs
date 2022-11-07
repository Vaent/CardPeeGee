using ExtensionMethods;
using System.Collections.Generic;
using static Text.Excerpts.Battle;
using static Text.TextManager;
using UnityEngine;

public class Battle : Encounter
{
    private bool battleHasStarted;
    private List<Monster> enemies;
    private bool isHealerBattle;
    private Text.BaseExcerpt monsterStatsText;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Battle;

    public Battle(List<Card> cards) : base(cards)
    {
        enemies = new List<Monster>();
        enemies.Add(new Monster(agitator, props));
    }

    // alternative constructor for battling "guards" on a Healer
    public Battle(List<Monster> enemies)
    {
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
        DisplayText(Announce);
        monsterStatsText = AnnounceStats(enemies[0].Attack, enemies[0].Deal, enemies[0].Hp);
        DisplayTextAsExtension(monsterStatsText, Announce);
        Timer.DelayThenInvoke(2, MonsterFlees);
        // TODO: prompt to activate/play cards then click the deck
    }

    public override void CardSelected(Card card)
    {
        if (battleHasStarted && !isHealerBattle) return;

        player.ConfigureSelectedCardOptions(card, Suit.Club, Suit.Spade);
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            // TODO: calculate scores, deal damage if applicable
            // TODO: end the encounter if all enemies are dead
            // TODO: return cards to the deck if encounter is still active
        }
    }

    public void ClaimLoot()
    {
        HideText(TempMonsterFlees);
        DisplayText(LootIsClaimed);
        player.Hand.Accept(props);
        Timer.DelayThenInvoke(2, GameState.EndEncounter, this);
    }

    // temporary method to resolve Battle encounters while full encounter logic has not been delivered
    public void MonsterFlees()
    {
        DisplayText(TempMonsterFlees);
        deck.Accept(agitator);
        Timer.DelayThenInvoke(2, ClaimLoot);
    }
}
