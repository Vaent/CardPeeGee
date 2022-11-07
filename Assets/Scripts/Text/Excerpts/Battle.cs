using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Battle
    {
        public static Excerpt Announce { get; } = new Excerpt("a MONSTER\nattacks you!", announceEncounter, Options.StrongText.Color(Color.green));
        public static Excerpt AnnounceStats_ { get; } = new Excerpt("Attack: {0}\nDeal: {1}\nHP: {2}", announceEncounter);
        public static Excerpt LootIsClaimed { get; } = new Excerpt("The monster is defeated.\nYou take its equipment.", stagingArea);

        // temporary Excerpt used while encounter resolution logic is incomplete
        public static Excerpt TempMonsterFlees { get; } = new Excerpt("The monster runs away,\nleaving its possessions\nbehind!", stagingArea);

        public static Excerpt<int, int, int> AnnounceStats(int attack, int deal, int hp)
        {
            return new Excerpt<int, int, int>(AnnounceStats_, attack, deal, hp);
        }
    }
}
