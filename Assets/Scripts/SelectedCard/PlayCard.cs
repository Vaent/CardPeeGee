using System.Collections;
using UnityEngine;

public class PlayCard : SelectedCardOption
{
    private readonly Color playedCardColour = new Color(0.69f, 0.84f, 0.96f);

    protected virtual Color NewColour => playedCardColour;

    protected override void OnMouseDown()
    {
        // TODO: change card colour
        // TODO: move card to "Played cards" area
    }
}
