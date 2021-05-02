using UnityEngine;

public abstract class SelectedCardOption : MonoBehaviour
{
    public SelectedCardOptionsPanel panel;

    protected abstract Color NewColor { get; }
    protected abstract CardZone TargetZone { get; }

    protected virtual void OnMouseDown()
    {
        Card selectedCard = panel.SelectedCard;
        selectedCard.Resize(1);
        selectedCard.ApplyColor(NewColor);
        TargetZone.Accept(selectedCard);
        panel.Hide();
    }
}
