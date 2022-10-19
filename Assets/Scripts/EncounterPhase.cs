// PLACEHOLDER - see documentation/Design decisions - Gameplay flow.md
public class EncounterPhase : IGamePhase
{
    private static readonly EncounterPhase instance = new EncounterPhase();

    // PLACEHOLDER pending extraction of encounter management logic from GameState into this class
    public Encounter encounter;

    private EncounterPhase() { }

    public static IGamePhase GetClean()
    {
        // TODO: initialisation
        return instance;
    }

    // cards are not discarded during encounters
    public void RegisterDiscardAction(Card card) { }

    public void RegisterInteractionWith(Card card)
    {
        encounter.CardSelected(card);
    }
}
