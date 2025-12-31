using static Text.Position;
using static Util.TextUtil;

namespace Text.Excerpts
{
    public static class Town
    {
        public static Excerpt Announce { get; } = new Excerpt("You are\nin TOWN", announceEncounter, Options.StrongText);
        public static Excerpt CardsCanBeActivated { get; } = new Excerpt("You may activate cards.", stagingArea);
        public static Excerpt Continue { get; } = new Excerpt("Click the deck (or press D)\nto continue.", stagingArea);
        public static Excerpt Charity { get; } = new Excerpt("Your possessions are so\nmeagre that you receive\ncharity, in the form\nof an extra card.", stagingArea);
        public static Excerpt Healing { get; } = new Excerpt("(healing)", announceEncounter);
        public static Excerpt HealingIsPossible { get; } = new Excerpt("You may play hearts\nfrom your hand, which will\nincrease your HP.", stagingArea);
        public static Excerpt HealingNotPossible { get; } = new Excerpt("You have no hearts to play\nand no cards that can be\nconverted to hearts.", stagingArea);
        private static Excerpt HealingPoints_ { get; } = new Excerpt("HP gained: {0}", rightOfPlayedCards, Options.TinyText);
        public static Excerpt HealingPointsBoosted { get; } = new Excerpt("(Ace of Hearts gives\na 50% bonus)", rightOfPlayedCards, Options.TinyText);
        public static Excerpt Shopping { get; } = new Excerpt("(shopping)", announceEncounter);
        public static Excerpt ShoppingIsPossible { get; } = new Excerpt("You may play diamonds\nfrom your hand, which will\nbe traded for new cards,\nat an exchange rate of\none new card for every\n8 diamonds (total value of\ncards played = amount of\ndiamonds spent).", stagingArea);
        public static Excerpt ShoppingNotPossible { get; } = new Excerpt("You can't afford to buy\nany new cards.", stagingArea);
        private static Excerpt ShoppingPoints_ { get; } = new Excerpt("{0} = {1}", rightOfPlayedCards, Options.TinyText);
        public static Excerpt ShoppingPointsBoosted { get; } = new Excerpt("(Ace of Diamonds gives\na 50% bonus to total spent)", rightOfPlayedCards, Options.TinyText);
        public static Excerpt Tax { get; } = new Excerpt("You must pay tax:\nSelect a card\nto throw away.", stagingArea);
        // TODO: add "proceed text", replace affected usage of Continue

        public static UpdateableExcerpt<int> HealingPoints(int amountOfHealing)
        {
            return new UpdateableExcerpt<int>(HealingPoints_, amountOfHealing);
        }

        public static void updateHealingPoints(UpdateableExcerpt<int> excerpt, int amountOfHealing)
        {
            excerpt.updateArg0(amountOfHealing);
        }

        public static UpdateableExcerpt<string, string> ShoppingPoints(int amountPaid, int cardsPurchased)
        {
            return new UpdateableExcerpt<string, string>(
                ShoppingPoints_,
                QuantifiedItem("diamond", amountPaid),
                QuantifiedItem("new card", cardsPurchased)
            );
        }

        public static void updateShoppingPoints(UpdateableExcerpt<string, string> excerpt, int amountPaid, int cardsPurchased)
        {
            excerpt.updateAllArgs(QuantifiedItem("diamond", amountPaid), QuantifiedItem("new card", cardsPurchased));
        }
    }
}
