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
            //UnityEngine.Debug.LogFormat("CharacterIdentified_ precedingText: {0}", CharacterIdentified_.precedingText);
            return new Excerpt<Card>(CharacterIdentified_, characterCard);
        }

        public static Excerpt<int, int, string> HPCalculated(int hpTotal, int hpBaseValue, string hpFromCards)
        {
            //UnityEngine.Debug.LogFormat("HPCalculated_ precedingText: {0}", HPCalculated_.precedingText);
            return new Excerpt<int, int, string>(HPCalculated_, hpTotal, hpBaseValue, hpFromCards);
        }
    }
}
