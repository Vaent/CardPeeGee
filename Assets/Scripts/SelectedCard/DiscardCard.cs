using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardCard : MonoBehaviour, IPointerClickHandler
{
    public Deck deck;
    public SelectedCardOptionsPanel panel;

    public void OnPointerClick(PointerEventData eventData)
    {
        panel.Hide();
        GameController.RegisterDiscardAction(panel.SelectedCard);
    }
}
