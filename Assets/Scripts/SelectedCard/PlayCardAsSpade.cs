using System.Collections;
using UnityEngine;

public class PlayCardAsSpade : PlayCard
{
    private readonly Color convertedCardColor = new Color(1f, 1f, 0.6f);

    protected override Color NewColour => convertedCardColor;

    protected override void OnMouseDown()
    {
        // TODO: convert card to Spade suit
        base.OnMouseDown();
    }
}
