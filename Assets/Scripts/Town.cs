public class Town
{
    private const int CharityUpperLimitExclusive = 4;
    private const int TaxLowerLimitExclusive = 6;

    private static readonly Town instance = new Town();

    private Town.Phase phase = Phase.Tax;

    private Town() { }

    public static void Advance()
    {
        instance.ResolveCurrentPhase();
        instance.IncrementPhase();
    }

    private void IncrementPhase()
    {
        var nextPhase = ((int)phase + 1) % 3;
        phase = (Town.Phase)nextPhase;
    }

    private void ResolveCurrentPhase()
    {
        switch (phase)
        {
            case Phase.Tax:
                // TODO: collect tax or provide charity
            case Phase.Shop:
                // TODO: collect any played cards, deal new cards
            case Phase.Heal:
                // TODO: collect any played cards, heal the player
            default:
                throw new System.Exception("Town.Phase value not recognised");
        }
    }

    private enum Phase
    {
        Tax,
        Shop,
        Heal
    }
}
