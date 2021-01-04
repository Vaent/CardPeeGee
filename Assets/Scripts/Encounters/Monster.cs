using System.Collections.Generic;

public class Monster
{
    private const int BaseDeal = 3;

    private readonly int attack;
    private readonly int deal;
    private int hp;

    // accessors
    public int Attack => attack;
    public int Deal => deal;

    public Monster(Card agitator, List<Card> props)
    {
        attack = agitator.Value + CardUtil.SumValues(props, Suit.Club);
        deal = BaseDeal + CardUtil.CountValues(props, Suit.Spade);
        hp = agitator.Value + CardUtil.SumValues(props, Suit.Heart);
    }

    public void Damage(int amount)
    {
        hp -= amount;
    }

    public bool IsAlive()
    {
        return hp > 0;
    }
}
