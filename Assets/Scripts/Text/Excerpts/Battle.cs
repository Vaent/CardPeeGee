using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Battle
    {
        public static Excerpt Announce { get; } = new Excerpt("a MONSTER\nattacks you", announceEncounter, Options.StrongText.Color(Color.green));
    }
}
