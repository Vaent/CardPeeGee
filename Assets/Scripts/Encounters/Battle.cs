using static CardUtil;
using static Constant;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using Text;
using static Text.Excerpts.Battle;
using static Text.TextManager;
using UnityEngine;

public class Battle : Encounter
{
    private const int defaultPlayerAttack = 10;
    private const int defaultPlayerDeal = 3;

    private bool battleHasStarted;
    private List<Enemy> enemies;
    private readonly bool isHealerBattle;
    private int enemyScore;
    private BaseExcerpt enemyScoreExcerpt;
    private UpdateableExcerpt<int, int, int> enemyStatsExcerpt;
    private int playerAttack = defaultPlayerAttack;
    private BaseExcerpt playerAttackExcerpt;
    private int playerDeal = defaultPlayerDeal;
    private BaseExcerpt playerDealExcerpt;
    private int playerScore;
    private BaseExcerpt playerScoreExcerpt;
    private BaseExcerpt resultExcerpt;

    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Battle;

    public Battle(List<Card> cards) : base(cards)
    {
        enemies = new List<Enemy>
        {
            new Enemy(agitator, props)
        };
    }

    // alternative constructor for battling "jailors" on a Healer
    public Battle(List<Enemy> enemies)
    {
        isHealerBattle = true;
        // copy input list to ensure enemies can't be mutated from outside
        this.enemies = new List<Enemy>(enemies);
    }

    public override void Advance()
    {
        Debug.Log("Invoked Battle.Advance()");
        if (!battleHasStarted) battleHasStarted = true;
        HideText(InitialPrompt, PromptNextRound);
        DisplayTextAsExtension(PlayerScoreCardsLabel, 1, playerDealExcerpt, playerAttackExcerpt);
        deck.DealCards(playerDeal);
    }

    protected override void BeginImpl()
    {
        Debug.Log("Beginning a battle against " + enemies.Print());
        DisplayText(Announce);
        enemyStatsExcerpt = AnnounceStats(enemies[0].Attack, enemies[0].Deal, enemies[0].Hp);
        DisplayTextAsExtension(enemyStatsExcerpt, Announce);
        RefreshPlayerStatsDisplay();
        DisplayText(InitialPrompt);
    }

    private int CalculatePlayerStat(int defaultValue, Suit suit, Func<CardZone, Suit, int> reducer)
    {
        bool boostStat = player.CardsActivated.Exists(Is(suit, ACE));
        int bonusFromPlayedCards = reducer(player.CardsPlayed, suit);
        return defaultValue + (boostStat ? Mathf.CeilToInt(bonusFromPlayedCards * 1.5f) : bonusFromPlayedCards);
    }

    public override void CardSelected(Card card)
    {
        if (battleHasStarted && !isHealerBattle) return;

        player.ConfigureSelectedCardOptions(card, new TargetSuit(Suit.Club, true), new TargetSuit(Suit.Spade, false));
    }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (player.CardsActivated.Equals(cardZone) || player.CardsPlayed.Equals(cardZone))
        {
            playerAttack = CalculatePlayerStat(defaultPlayerAttack, Suit.Club, SumValues);
            playerDeal = CalculatePlayerStat(defaultPlayerDeal, Suit.Spade, CountMatches);
            RefreshPlayerStatsDisplay();
        }
        else if (cardZone is StagingArea)
        {
            if (playerScore == 0)
            {
                ProcessPlayerScoreCards(cards);
            }
            else
            {
                ProcessEnemyScoreCards(cards);
            }
        }
        else if (cardZone is Deck)
        {
            HideText(resultExcerpt);
            // TODO: handle multiple enemies
            if (enemies[0].IsAlive())
            {
                DisplayText(PromptNextRound);
            }
            else
            {
                deck.Accept(enemies[0].Agitator);
                enemies.RemoveAt(0);
                // TODO: alternate handling for end of healer battles
                ClaimLoot();
            }
        }
    }

    private void ClaimLoot()
    {
        DisplayText(LootIsClaimed);
        player.Hand.Accept(props);
        Timer.DelayThenInvoke(2, GameState.EndEncounter, this);
    }

    private void EndCombatRound()
    {
        playerScore = enemyScore = 0;
        deck.Accept(GameState.GetStagingArea.Cards);
    }

    private void EndCombatRound(float delay)
    {
        Timer.DelayThenInvoke(delay, EndCombatRound);
    }

    private void HandleScoresDrawn()
    {
        resultExcerpt = ResultDrawn;
        DisplayText(ResultDrawn);
        EndCombatRound(1);
    }

    private void HandleScoresEnemyWon()
    {
        resultExcerpt = ResultEnemyWon(enemies[0].Attack);
        DisplayText(resultExcerpt);
        player.Damage(enemies[0].Attack);
        Timer.DelayThenInvoke(1 + (0.05f * enemies[0].Attack), () =>
        {
            if (player.IsAlive()) EndCombatRound(0);
        });
    }

    private void HandleScoresPlayerWon()
    {
        resultExcerpt = ResultPlayerWon(playerAttack);
        DisplayText(resultExcerpt);
        damageEnemy(enemies[0], playerAttack);
    }

    private void damageEnemy(Enemy enemy, int damageToApply)
    {
        if (damageToApply > 0 && enemy.IsAlive())
        {
            enemy.Damage(1);
            updateEnemyHpDisplay(enemyStatsExcerpt, enemy.Hp);
            Timer.DelayThenInvoke(0.05f, () => damageEnemy(enemy, --damageToApply));
        }
        else
        {
            EndCombatRound(1);
        }
    }

    private void ProcessEnemyScoreCards(List<Card> cards)
    {
        // TODO: handle multiple enemies
        enemyScore = enemies[0].Attack + SumValues(cards);
        // TODO: use variant text for jailors
        enemyScoreExcerpt = ScoreForEnemy(enemyScore);
        DisplayTextAsExtension(enemyScoreExcerpt, playerScoreExcerpt);

        Action outcomeHandler = (playerScore, enemyScore) switch
        {
            _ when playerScore < enemyScore => HandleScoresEnemyWon,
            _ when playerScore == enemyScore => HandleScoresDrawn,
            _ when playerScore > enemyScore => HandleScoresPlayerWon,
            _ => () => throw new NotImplementedException() // to satisfy the compiler
        };
        Timer.DelayThenInvoke(2, () =>
        {
            HideText(playerScoreExcerpt, enemyScoreExcerpt);
            outcomeHandler();
        });
    }

    private void ProcessPlayerScoreCards(List<Card> cards)
    {
        playerScore = playerAttack + SumValues(cards);
        playerScoreExcerpt = ScoreForPlayer(playerScore);
        DisplayText(playerScoreExcerpt);
        Timer.DelayThenInvoke(0.5f, deck.DealCardsAlternate, enemies[0].Deal);
    }

    private void RefreshPlayerStatsDisplay()
    {
        HideText(playerAttackExcerpt, playerDealExcerpt);
        playerAttackExcerpt = PlayerAttackStat(playerAttack);
        playerDealExcerpt = PlayerDealStat(playerDeal);
        DisplayText(playerAttackExcerpt);
        DisplayTextAsExtension(playerDealExcerpt, playerAttackExcerpt);
    }
}
