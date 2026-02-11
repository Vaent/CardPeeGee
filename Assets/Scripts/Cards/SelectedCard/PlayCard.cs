using UnityEngine;

namespace Cards.SelectedCard
{
    public class PlayCard : SelectedCardOption
    {
        private readonly Color playedCardColor = new Color(0.69f, 0.84f, 0.96f);

        protected override Color NewColor => playedCardColor;
        protected override CardZone TargetZone => panel.CardsPlayedZone;
    }
}
