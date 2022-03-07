using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public class Generic
    {
        public static Excerpt NewEncounterPrompt { get; } = new Excerpt("Click on the deck (or press 'D') to\ndeal cards for a new encounter.", stagingArea + Vector2.down);
    }
}
