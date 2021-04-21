using System.Collections;
using UnityEngine;

public class ActivateCard : SelectedCardOption
{
    private readonly Color activatedCardColour = new Color(0.88f, 0.75f, 0.96f);

    protected virtual Color NewColour => activatedCardColour;

    protected override void OnMouseDown()
    {
        // TODO: change card colour
        // TODO: move card to "Activated cards" area
    }
}
