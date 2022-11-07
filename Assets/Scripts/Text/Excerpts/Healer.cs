using static Text.Position;
using UnityEngine;

namespace Text.Excerpts
{
    public static class Healer
    {
        public static Excerpt Announce { get; } = new Excerpt("you meet a\nHEALER", announceEncounter, Options.StrongText.Color(Color.cyan));
        public static Excerpt AnnounceFeeCharged { get; } = new Excerpt("(charging a fee)", announceEncounter);
        private static Excerpt AnnounceHealAmount_ { get; } = new Excerpt("heals {0} points", announceEncounter);
        public static Excerpt AnnouncePrisonerStatus { get; } = new Excerpt("(prisoner)", announceEncounter);
        private static Excerpt HealingIsDelivered_ { get; } = new Excerpt("You are healed for {0} HP", stagingArea);
        public static Excerpt PotionIsDelivered { get; } = new Excerpt("The healer also gives you\na healing potion", stagingArea);
        public static Excerpt PotionsAreDelivered { get; } = new Excerpt("The healer also gives you\nhealing potions", stagingArea);

        // temporary Excerpts used while encounter resolution logic is incomplete
        public static Excerpt TempRemoveFee { get; } = new Excerpt("The healer waives the fee!", stagingArea);
        public static Excerpt TempRemoveJailors { get; } = new Excerpt("The jailor(s) run away!", stagingArea);
        // temporary Excerpts end

        public static Excerpt<int> HealingIsDelivered(int healingAmount)
        {
            return new Excerpt<int>(HealingIsDelivered_, healingAmount);
        }

        public static Excerpt<int> AnnounceHealAmount(int healingAmount)
        {
            return new Excerpt<int>(AnnounceHealAmount_, healingAmount);
        }
    }
}
