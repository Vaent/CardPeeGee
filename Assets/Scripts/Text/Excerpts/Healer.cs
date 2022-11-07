using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Healer
    {
        public static Excerpt Announce { get; } = new Excerpt("you meet a\nHEALER", announceEncounter, Options.StrongText.Color(Color.cyan));
        public static Excerpt HealAmount_ { get; } = new Excerpt("heals {0} points", announceEncounter);
        public static Excerpt Prisoner { get; } = new Excerpt("(prisoner)", announceEncounter);
        public static Excerpt ChargesFee { get; } = new Excerpt("(charging a fee)", announceEncounter);

        public static Excerpt<int> HealAmount(int amountOfHealing)
        {
            return new Excerpt<int>(HealAmount_, amountOfHealing);
        }
    }
}
