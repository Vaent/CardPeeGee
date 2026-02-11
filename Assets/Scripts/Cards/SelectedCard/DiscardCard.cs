using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards.SelectedCard
{
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
}
