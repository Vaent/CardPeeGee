using System.Collections.Generic;
using static System.Math;
using static Text.Trap.TextReference;
using UnityEngine;

public class Trap : Encounter
{
    private static readonly Color themeColor = new Color(0.75f, 0.75f, 0.75f);

    private int baseDamage;
    private int damageAvoided;
    private int damageDealt;
    private int evasionScore;
    private List<Card> scoreCards;
    private int trapDifficulty;

    protected override Color ThemeColor => themeColor;
    protected override JukeBox.Track ThemeMusic => JukeBox.Track.Trap;

    public Trap(List<Card> cards) : base(cards)
    {
        baseDamage = agitator.Value
            + CardUtil.SumValues(props, Suit.Club)
            + (int)Ceiling((float)CardUtil.SumValues(props, Suit.Heart) / 2);
        trapDifficulty = agitator.Value
            + CardUtil.SumValues(props, Suit.Spade)
            + (int)Ceiling((float)CardUtil.SumValues(props, Suit.Diamond) / 2);
    }

    // Traps are resolved automatically - Advance() should never be called
    public override void Advance(){ }

    protected override void BeginImpl()
    {
        Debug.Log($"Trap [difficulty: {trapDifficulty}, damage: {baseDamage}] has been triggered");
        Text.Trap.DisplayFormatted(StrongTextOptions(), (int)Announce);
        Text.Trap.DisplayFormatted(StrongTextOptions(), (int)Stats, trapDifficulty, baseDamage);
        Timer.DelayThenInvoke(1f, CalculateScores);
    }

    private void CalculateScores()
    {
        Text.Options smallText = new Text.Options().Size(Text.TextSize.Small);
        Text.Trap.DisplayFormatted(smallText, (int)ScoreCards);
        Text.Trap.DisplayAsExtension((int)AttemptEvade, (int)Announce);
        deck.DealCards(1);
    }

    // Traps are resolved automatically - there is no opportunity to play cards in Vanilla CardPeeGee
    public override void CardSelected(Card card) { }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            // currently the encounter cards remain in the staging area so scores are not being calculated correctly
            scoreCards = cardZone.Cards;
            if (scoreCards.Count < (player.CardsActivated.Cards.Count + 1))
            {
                Text.Trap.DisplayAsExtension((int)Assisted, (int)AttemptEvade);
                deck.DealCards(1);
                // TODO: play a random "helper" audio clip
                return;
            }
            else
            {
                DetermineOutcome();
            }
        }
    }

    private void DetermineOutcome()
    {
        evasionScore = CardUtil.SumValues(scoreCards);
        damageAvoided = Mathf.Clamp((2 * (evasionScore - trapDifficulty)), 0, baseDamage);
        damageDealt = baseDamage - damageAvoided;
        Text.Trap.Display((int)EvasionScore, evasionScore);
        Timer.DelayThenInvoke(0.4f, DetermineOutcomeCallback);
    }

    private void DetermineOutcomeCallback()
    {
        Text.Trap.DisplayAsExtension((int)DamageAvoided, (int)EvasionScore, damageAvoided);
        Timer.DelayThenInvoke(0.4f, DetermineOutcomeCallback1);
    }

    private void DetermineOutcomeCallback1()
    {
        Text.Trap.DisplayAsExtension((int)DamageDealt, (int)DamageAvoided, damageDealt);
        Timer.DelayThenInvoke(1, DetermineOutcomeCallback2);
    }

    private void DetermineOutcomeCallback2()
    {
        player.Damage(damageDealt);
        Timer.DelayThenInvoke(2.5f, GameState.EndEncounter, this);
    }
}
