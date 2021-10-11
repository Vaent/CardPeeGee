using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Treasure
    {
        public static Excerpt Announce { get; } = new Excerpt("you find a\nTREASURE\nCHEST", announceEncounter, Options.StrongText.Color(Color.yellow));
    }
}
