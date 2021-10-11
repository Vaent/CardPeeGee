using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Trap
    {
        private static readonly Vector2 statsPosition = new Vector2(7.5f, 0);

        public static Excerpt Announce { get; } = new Excerpt("it's a\nTRAP!", announceEncounter, Options.StrongText.Color(new Color(0.75f, 0.75f, 0.75f)));
        public static Excerpt Assisted { get; } = new Excerpt("helped by your\nActive cards", announceEncounter);
        public static Excerpt AttemptEvade { get; } = new Excerpt("You try to\navoid it...", announceEncounter);
        private static Excerpt DamageAvoided_ { get; } = new Excerpt("Damage avoided: {0}", announceEncounter);
        private static Excerpt DamageDealt_ { get; } = new Excerpt("Damage dealt: {0}", belowDealtCards);
        private static Excerpt EvasionScore_ { get; } = new Excerpt("Evasion score: {0}", belowDealtCards);
        public static Excerpt ScoreCards { get; } = new Excerpt("Score cards:\n(evasion)", leftOfDealtCards, Options.SmallText);
        private static Excerpt Stats_ { get; } = new Excerpt("Difficulty: {0}\nDamage: {1}", statsPosition, new Options().Anchor(TextAnchor.UpperCenter));

        public static Excerpt<int> DamageAvoided(int amount)
        {
            return new Excerpt<int>(DamageAvoided_, amount);
        }

        public static Excerpt<int> DamageDealt(int amount)
        {
            return new Excerpt<int>(DamageDealt_, amount);
        }

        public static Excerpt<int> EvasionScore(int evasionScore)
        {
            return new Excerpt<int>(EvasionScore_, evasionScore);
        }

        public static Excerpt<int, int> Stats(int difficulty, int damage)
        {
            return new Excerpt<int, int>(Stats_, difficulty, damage);
        }
    }
}
