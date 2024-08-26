using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Treasure
    {
        public static Excerpt Announce { get; } = new Excerpt("you find a\nTREASURE\nCHEST", announceEncounter, Options.StrongText.Color(Color.yellow));
        public static Excerpt AnnounceTrap { get; } = new Excerpt("(trapped)", announceEncounter);
        public static Excerpt AnnounceTrapContinuation { get; } = new Excerpt("(still trapped)", announceEncounter);
        private static Excerpt DisarmBonus_ { get; } = new Excerpt("disarm bonus: {0}", rightOfPlayedCards, Options.TinyText);
        private static Excerpt DisarmFailure_ { get; } = new Excerpt("You take {0} damage!", belowDealtCards);
        public static Excerpt DisarmNeutralOutcome { get; } = new Excerpt("You fail to disarm the trap,\nbut take no damage", belowDealtCards);
        public static Excerpt DisarmSuccess { get; } = new Excerpt("The trap is disarmed", belowDealtCards);
        public static Excerpt KaChing { get; } = new Excerpt("ka-ching!", announceEncounter);
        public static Excerpt PromptAbandonEncounter { get; } = new Excerpt("Or, if you don't think it's\nworth the effort, you can just", stagingAreaOffset1);
        public static Excerpt PromptClickToDisarm { get; } = new Excerpt("Click on the spade 'prop' to\ntry disarming it.", stagingAreaOffset1);
        public static Excerpt PromptPlayCards { get; } = new Excerpt("Play spades to increase your\nchance of disarming the trap.", stagingAreaOffset1);
        private static Excerpt ScoreForPlayer_ { get; } = new Excerpt("disarm score: {0}", belowDealtCards);
        private static Excerpt ScoreForTrap_ { get; } = new Excerpt("trap score: {0}", belowDealtCards);

        public static Excerpt<int> DisarmBonus(int points)
        {
            return new Excerpt<int>(DisarmBonus_, points);
        }

        public static Excerpt<int> DisarmFailure(int damageAmount)
        {
            return new Excerpt<int>(DisarmFailure_, damageAmount);
        }

        public static Excerpt<int> ScoreForPlayer(int points)
        {
            return new Excerpt<int>(ScoreForPlayer_, points);
        }

        public static Excerpt<int> ScoreForTrap(int points)
        {
            return new Excerpt<int>(ScoreForTrap_, points);
        }
    }
}
