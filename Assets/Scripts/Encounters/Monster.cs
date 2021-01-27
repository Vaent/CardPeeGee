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

    public static List<Monster> FindAllIn(List<Card> possibleMonsters)
    {
        List<Monster> monsters = new List<Monster>();
        possibleMonsters.ForEach(card =>
        {
            if (card.Suit == Suit.Club)
            {
                monsters.Add(new Monster(card, new List<Card>()));
            }
        });
        return monsters;
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    public override string ToString()
    {
        return ("Monster[ATK" + attack + ":DL" + deal + ":HP" + hp + "]");
    }
}
