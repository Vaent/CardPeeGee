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
        // TODO: replace leading newlines with explicit placement
        public static Excerpt PaymentRequiredPostBattle { get; } = new Excerpt("\n\nYou must still pay the healer's\nfee to receive healing.", stagingArea);
        private static Excerpt PaymentStatus_ { get; } = new Excerpt("fee paid: {0}\nfee remaining: {1}", rightOfPlayedCards, Options.TinyText);
        public static Excerpt PotionIsDelivered { get; } = new Excerpt("The healer also gives you\na healing potion", stagingArea);
        public static Excerpt PotionsAreDelivered { get; } = new Excerpt("The healer also gives you\nhealing potions", stagingArea);
        // TODO: replace leading newlines with explicit placement
        public static Excerpt PromptPayFee { get; } = new Excerpt("\n\n\nPay the fee using\ncards of any suit.\n\n\n\nOr, if you're already feeling\nhealthy enough, you can", stagingArea);

        // temporary Excerpts used while encounter resolution logic is incomplete
        public static Excerpt TempRemoveJailors { get; } = new Excerpt("The jailor(s) run away!", stagingArea);
        // temporary Excerpts end

        public static Excerpt<int> AnnounceHealAmount(int healingAmount)
        {
            return new Excerpt<int>(AnnounceHealAmount_, healingAmount);
        }

        public static Excerpt<int> HealingIsDelivered(int healingAmount)
        {
            return new Excerpt<int>(HealingIsDelivered_, healingAmount);
        }

        public static UpdateableExcerpt<int, int> PaymentStatus(int feePaid, int feeRemaining)
        {
            return new UpdateableExcerpt<int, int>(PaymentStatus_, feePaid, feeRemaining);
        }

        public static void updatePaymentStatus(UpdateableExcerpt<int, int> excerpt, int feePaid, int feeRemaining)
        {
            excerpt.updateArg0(feePaid);
            excerpt.updateArg1(feeRemaining);
        }
    }
}
