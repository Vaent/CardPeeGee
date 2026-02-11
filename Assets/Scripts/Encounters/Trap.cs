using Audio;
using Cards;
using System.Collections.Generic;
using static System.Math;
using static Text.Excerpts.Trap;
using static Text.TextManager;
using UnityEngine;

public class Trap : Encounter
{
    private int baseDamage;
    private int damageAvoided;
    private Text.BaseExcerpt damageAvoidedExcerpt;
    private int damageDealt;
    private int evasionScore;
    private Text.BaseExcerpt evasionScoreExcerpt;
    private List<Card> scoreCards;
    private int trapDifficulty;

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
        DisplayText(Announce);
        DisplayText(Stats(trapDifficulty, baseDamage));
        Timer.DelayThenInvoke(1f, CalculateScores);
    }

    private void CalculateScores()
    {
        DisplayText(ScoreCards);
        DisplayTextAsExtension(AttemptEvade, Announce);
        deck.DealCards(1);
    }

    // Traps are resolved automatically - there is no opportunity to play cards in Vanilla CardPeeGee
    public override void CardSelected(Card card) { }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        if (cardZone is StagingArea)
        {
            scoreCards = cardZone.Cards;
            if (scoreCards.Count < (player.CardsActivated.Cards.Count + 1))
            {
                DisplayTextAsExtension(Assisted, AttemptEvade);
                deck.DealCards(1);
                SoundEffects.PlayRandomTrapAssistClip();
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
        evasionScoreExcerpt = EvasionScore(evasionScore);
        DisplayText(evasionScoreExcerpt);
        Timer.DelayThenInvoke(0.4f, DetermineOutcomeCallback);
    }

    private void DetermineOutcomeCallback()
    {
        damageAvoidedExcerpt = DamageAvoided(damageAvoided);
        DisplayTextAsExtension(damageAvoidedExcerpt, evasionScoreExcerpt);
        Timer.DelayThenInvoke(0.4f, DetermineOutcomeCallback1);
    }

    private void DetermineOutcomeCallback1()
    {
        DisplayTextAsExtension(DamageDealt(damageDealt), damageAvoidedExcerpt);
        Timer.DelayThenInvoke(1, DetermineOutcomeCallback2);
    }

    private void DetermineOutcomeCallback2()
    {
        player.Damage(damageDealt);
        //TODO: refactor handling of player death
        //the implementation below would fail if >50hp are removed resulting in death
        //(it should not be possible for a trap to do that much damage in Vanilla but a more robust solution is needed)
        Timer.DelayThenInvoke(2.5f, DetermineOutcomeCallback3);
    }

    private void DetermineOutcomeCallback3()
    {
        if (player.IsAlive())
        {
            GameState.EndEncounter(this);
        }
    }
}
