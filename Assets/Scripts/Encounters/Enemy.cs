using Cards;
using System.Collections.Generic;

public class Enemy
{
    private const int BaseDeal = 3;

    public Card Agitator { get; }
    public int Attack { get; }
    public int Deal { get; }
    public int Hp { get; private set; }

    public Enemy(Card agitator, List<Card> props)
    {
        Agitator = agitator;
        Attack = agitator.Value + CardUtil.SumValues(props, Suit.Club);
        Deal = BaseDeal + CardUtil.CountMatches(props, Suit.Spade);
        Hp = agitator.Value + CardUtil.SumValues(props, Suit.Heart);
    }

    public void Damage(int amount)
    {
        Hp -= amount;
    }

    public static List<Enemy> FindAllIn(List<Card> possibleEnemies)
    {
        List<Enemy> enemies = new List<Enemy>();
        possibleEnemies.ForEach(card =>
        {
            if (card.Suit == Suit.Club)
            {
                enemies.Add(new Enemy(card, new List<Card>()));
            }
        });
        return enemies;
    }

    public bool IsAlive()
    {
        return Hp > 0;
    }

    public override string ToString()
    {
        return ("Enemy[ATK" + Attack + ":DL" + Deal + ":HP" + Hp + "]");
    }
}
