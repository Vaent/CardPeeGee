// PLACEHOLDER - see documentation/Design decisions - Gameplay flow.md
public class EncounterPhase : GamePhase
{
    private static readonly EncounterPhase instance = new EncounterPhase();

    // PLACEHOLDER pending extraction of encounter management logic from GameState into this class
    public Encounter encounter;

    private EncounterPhase() { }

    public static GamePhase GetClean()
    {
        // TODO: initialisation
        return instance;
    }

    public void RegisterInteractionWith(Card card)
    {
        encounter.CardSelected(card);
    }
}
