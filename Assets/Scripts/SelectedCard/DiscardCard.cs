using UnityEngine;

public class DiscardCard : MonoBehaviour
{
    public Deck deck;
    public SelectedCardOptionsPanel panel;

    private void OnMouseDown()
    {
        panel.Hide();
        GameController.RegisterDiscardAction(panel.SelectedCard);
    }
}
