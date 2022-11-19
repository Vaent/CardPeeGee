using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Treasure
    {
        public static Excerpt Announce { get; } = new Excerpt("you find a\nTREASURE\nCHEST", announceEncounter, Options.StrongText.Color(Color.yellow));
        public static Excerpt AnnounceTrap { get; } = new Excerpt("(trapped)", announceEncounter);
        public static Excerpt KaChing { get; } = new Excerpt("ka-ching!", announceEncounter);

        // Temporary Excerpts below should be replaced when relevant functionality is introduced
        public static Excerpt PromptAbandonEncounter { get; } = new Excerpt("<demo> Your only option in the\ncurrent demo build is to", stagingAreaOffset1);
        //public static Excerpt PromptAbandonEncounter { get; } = new Excerpt("Or, if you don't think it's\nworth the effort, you can just", stagingAreaOffset1);
        public static Excerpt PromptClickToDisarm { get; } = new Excerpt("<demo> It is not yet possible\nto disarm a trapped chest.", stagingAreaOffset1);
        //public static Excerpt PromptClickToDisarm { get; } = new Excerpt("Click on the spade 'prop' to\ntry disarming it.", stagingAreaOffset1);
        public static Excerpt PromptPlayCards { get; } = new Excerpt("<demo> Playing cards from\nyour hand has no effect yet.", stagingAreaOffset1);
        //public static Excerpt PromptPlayCards { get; } = new Excerpt("Play spades to increase your\nchance of disarming the trap.", stagingAreaOffset1);
    }
}
