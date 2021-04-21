using System.Collections;
using UnityEngine;

public class PlayCardAsClub : PlayCard
{
    private readonly Color convertedCardColor = new Color(1f, 1f, 0.6f);

    protected override Color NewColour => convertedCardColor;

    protected override void OnMouseDown()
    {
        // TODO: convert card to Club suit
        base.OnMouseDown();
    }
}
