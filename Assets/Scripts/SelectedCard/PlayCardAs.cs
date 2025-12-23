using UnityEngine;
using UnityEngine.EventSystems;

public class PlayCardAs : PlayCard, IPointerClickHandler
{
    public Suit suit;

    private readonly Color convertedCardColor = new Color(1f, 1f, 0.6f);

    protected override Color NewColor => convertedCardColor;

    public override void OnPointerClick(PointerEventData eventData)
    {
        panel.SelectedCard.Convert(suit);
        base.OnPointerClick(eventData);
    }
}
