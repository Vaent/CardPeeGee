using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Healer
    {
        public static Excerpt Announce { get; } = new Excerpt("you meet a\nHEALER", announceEncounter, Options.StrongText.Color(Color.cyan));
    }
}
