using UnityEngine;

public class ActivateCard : SelectedCardOption
{
    private readonly Color activatedCardColor = new Color(0.88f, 0.75f, 0.96f);

    protected override Color NewColor => activatedCardColor;
    protected override CardZone TargetZone => panel.CardsActivatedZone;
}
