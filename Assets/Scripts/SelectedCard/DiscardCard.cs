using UnityEngine;

public class DiscardCard : MonoBehaviour
{
    public Deck deck;
    public SelectedCardOptionsPanel panel;

    private void OnMouseDown()
    {
        deck.Accept(panel.SelectedCard);
        panel.Hide();
        Town.PlayerHasDiscarded(panel.SelectedCard);
    }
}
