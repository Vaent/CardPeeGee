using UnityEngine;

public class PlayCardAs : PlayCard
{
    public Suit suit;

    private readonly Color convertedCardColor = new Color(1f, 1f, 0.6f);

    protected override Color NewColor => convertedCardColor;

    protected override void OnMouseDown()
    {
        panel.SelectedCard.Convert(suit);
        base.OnMouseDown();
    }
}
