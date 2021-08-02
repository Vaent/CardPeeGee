using System.Collections.Generic;
using static System.Math;
using static Text.Trap.TextReference;
using UnityEngine;

public class Trap : Encounter
{
    private static readonly Color themeColor = new Color(0.75f, 0.75f, 0.75f);

    private bool active;
    private int baseDamage;
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
        active = true;
        Timer.DelayThenInvoke(1f, CalculateScores);
    }

    private void CalculateScores()
    {
        if (!active) throw new System.Exception("Invoked Trap.CalculateScores() out of turn");

        Text.Trap.DisplayAsExtension((int)AttemptEvade, (int)Announce);
        // TODO: calculate scores
    }

    // Traps are resolved automatically - there is no opportunity to play cards in Vanilla CardPeeGee
    public override void CardSelected(Card card) { }

    public override void CardsArrivedAt(CardZone cardZone, List<Card> cards)
    {
        // TODO: verify cardZone = StagingArea
        // TODO: calculate scores
        // TODO: display result/deal damage if applicable
        // TODO: tear down
    }
}
