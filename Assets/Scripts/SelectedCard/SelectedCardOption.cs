using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SelectedCardOption : MonoBehaviour, IPointerClickHandler
{
    public SelectedCardOptionsPanel panel;

    protected abstract Color NewColor { get; }
    protected abstract CardZone TargetZone { get; }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Card selectedCard = panel.SelectedCard;
        selectedCard.Resize(1);
        selectedCard.ApplyColor(NewColor);
        TargetZone.Accept(selectedCard);
        panel.Hide();
    }
}
