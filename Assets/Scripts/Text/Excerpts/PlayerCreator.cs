using Cards;
using System.Collections.Generic;
using static Text.Position;

namespace Text.Excerpts
{
    public static class PlayerCreator
    {
        private static Excerpt CharacterIdentified_ { get; } = new Excerpt("You are the {0}", belowDealtCards);
        public static Excerpt CharacterSearch { get; } = new Excerpt("Finding a Character card...", belowDealtCards);
        public static Excerpt DealHand { get; } = new Excerpt("Dealing your starting hand...", belowDealtCards);
        private static Excerpt HPCalculated_ { get; } = new Excerpt("You have {0} HP ({1} + {2})", belowDealtCards);
        public static Excerpt HPSearch { get; } = new Excerpt("Dealing cards for initial HP...", belowDealtCards);

        public static Excerpt<Card> CharacterIdentified(Card characterCard)
        {
            return new Excerpt<Card>(CharacterIdentified_, characterCard);
        }

        public static Excerpt<int, int, string> HPCalculated(int hpTotal, int hpBaseValue, List<int> hpCardValues)
        {
            return new Excerpt<int, int, string>(HPCalculated_, hpTotal, hpBaseValue, string.Join(" + ", hpCardValues));
        }
    }
}
