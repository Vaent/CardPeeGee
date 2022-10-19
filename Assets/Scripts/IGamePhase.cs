// PLACEHOLDER - see documentation/Design decisions - Gameplay flow.md
public interface IGamePhase
{
    public void RegisterDiscardAction(Card card);

    public void RegisterInteractionWith(Card card);
}
