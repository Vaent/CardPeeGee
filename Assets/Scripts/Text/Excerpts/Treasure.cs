using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Treasure
    {
        public static Excerpt Announce { get; } = new Excerpt("you find a\nTREASURE\nCHEST", announceEncounter, Options.StrongText.Color(Color.yellow));
        public static Excerpt AnnounceTrap { get; } = new Excerpt("(trapped)", announceEncounter);
        public static Excerpt KaChing { get; } = new Excerpt("ka-ching!", announceEncounter);

        // temporary Excerpt used while encounter resolution logic is incomplete
        public static Excerpt TempRemoveTraps { get; } = new Excerpt("The trap(s) fall off!", stagingArea);
    }
}
