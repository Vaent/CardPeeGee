using static Text.Position;

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
        public static Excerpt Shopping { get; } = new Excerpt("(shopping)", announceEncounter);
        public static Excerpt ShoppingIsPossible { get; } = new Excerpt("You may play diamonds\nfrom your hand, which will\nbe traded for new cards,\nat an exchange rate of\none new card for every\n8 diamonds (total value of\ncards played = amount of\ndiamonds spent).", stagingArea);
        public static Excerpt ShoppingNotPossible { get; } = new Excerpt("You can't afford to buy\nany new cards.", stagingArea);
        public static Excerpt Tax { get; } = new Excerpt("You must pay tax:\nSelect a card\nto throw away.", stagingArea);
        // TODO: add "points text"
        // TODO: add "proceed text", replace affected usage of Continue
    }
}
