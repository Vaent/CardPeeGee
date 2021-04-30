using UnityEngine;

public class DiscardCard : SelectedCardOption
{
    protected override Color NewColor => Color.white;
    // TODO: get a reference to the Deck and use this for DiscardCard.TargetZone
    protected override CardZone TargetZone => throw new System.NotImplementedException();
}
