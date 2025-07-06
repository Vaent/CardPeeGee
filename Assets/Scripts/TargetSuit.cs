public readonly struct TargetSuit
{
    public readonly Suit Suit { get; }
    public readonly bool HasValue { get; }

    public TargetSuit(Suit suit, bool hasValue)
    {
        Suit = suit;
        HasValue = hasValue;
    }
};
