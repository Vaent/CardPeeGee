using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Battle
    {
        public static Excerpt Announce { get; } = new Excerpt("a MONSTER\nattacks you!", announceEncounter, Options.StrongText.Color(Color.green));
        private static Excerpt AnnounceStats_ { get; } = new Excerpt("Attack: {0}\nDeal: {1}\nHP: {2}", announceEncounter);
        // TODO: replace leading newline with explicit placement
        public static Excerpt InitialPrompt { get; } = new Excerpt("\nClubs increase Attack.\n\nSpades increase Deal\n(score cards).\n\nYou can only play cards before\nthe fight - choose carefully!\nClick the deck to start fighting.", stagingArea);
        // TODO: replace leading newlines with explicit placement
        public static Excerpt LootIsClaimed { get; } = new Excerpt("\n\nThe monster is defeated.\nYou take its equipment.", stagingArea);
        private static Excerpt PlayerAttackStat_ { get; } = new Excerpt("Your attack points: {0}", rightOfPlayedCards, Options.TinyText);
        private static Excerpt PlayerDealStat_ { get; } = new Excerpt("Your 'deal' (for score cards): {0}", rightOfPlayedCards, Options.TinyText);
        public static Excerpt PlayerScoreCardsLabel { get; } = new Excerpt("Score cards:", rightOfPlayedCards, Options.TinyText);
        // TODO: replace leading newline with explicit placement
        public static Excerpt PromptNextRound { get; } = new Excerpt("\nClick on the deck to\nfight another round", belowDealtCards);
        public static Excerpt ResultDrawn { get; } = new Excerpt("You both parry.\nNo damage dealt", belowDealtCards);
        private static Excerpt ResultEnemyWon_ { get; } = new Excerpt("Monster wins!\nYou take {0} damage", belowDealtCards);
        private static Excerpt ResultPlayerWon_ { get; } = new Excerpt("You win!\nMonster takes {0} damage", belowDealtCards);
        private static Excerpt ScoreForEnemy_ { get; } = new Excerpt("The monster's strike scores {0}", belowDealtCards);
        private static Excerpt ScoreForPlayer_ { get; } = new Excerpt("Your strike scores {0}", belowDealtCards);
        
        public static Excerpt<int, int, int> AnnounceStats(int attack, int deal, int hp)
        {
            return new Excerpt<int, int, int>(AnnounceStats_, attack, deal, hp);
        }

        public static Excerpt<int> PlayerAttackStat(int attack)
        {
            return new Excerpt<int>(PlayerAttackStat_, attack);
        }

        public static Excerpt<int> PlayerDealStat(int deal)
        {
            return new Excerpt<int>(PlayerDealStat_, deal);
        }

        public static Excerpt<int> ResultEnemyWon(int damageDealt)
        {
            return new Excerpt<int>(ResultEnemyWon_, damageDealt);
        }

        public static Excerpt<int> ResultPlayerWon(int damageDealt)
        {
            return new Excerpt<int>(ResultPlayerWon_, damageDealt);
        }

        public static Excerpt<int> ScoreForEnemy(int points)
        {
            return new Excerpt<int>(ScoreForEnemy_, points);
        }

        public static Excerpt<int> ScoreForPlayer(int points)
        {
            return new Excerpt<int>(ScoreForPlayer_, points);
        }
    }
}
