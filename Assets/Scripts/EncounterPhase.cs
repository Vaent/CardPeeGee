// PLACEHOLDER - see documentation/Design decisions - Gameplay flow.md
public class EncounterPhase : GamePhase
{
    private static readonly EncounterPhase instance = new EncounterPhase();

    public GamePhase Get => instance;

    private EncounterPhase() { }
}
