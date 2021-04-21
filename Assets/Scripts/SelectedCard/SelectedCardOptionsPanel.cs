using UnityEngine;

public class SelectedCardOptionsPanel : MonoBehaviour
{
	private static readonly Vector3 positionOffset = new Vector3(0, 3.8f, 0);

	// component references
	public ActivateCard activate;
	public DiscardCard discard;
	public PlayCard play;
	public PlayCardAsClub playAsClub;
	public PlayCardAsDiamond playAsDiamond;
	public PlayCardAsHeart playAsHeart;
	public PlayCardAsSpade playAsSpade;

	public CardZone CardsActivatedZone { get; private set; }
	public CardZone CardsPlayedZone { get; private set; }
	public Card SelectedCard { get; private set; }

	public void ConfigureAndDisplay(ReInitialiser ri)
    {
		gameObject.SetActive(false);

		SelectedCard = ri.SelectedCard;
        gameObject.transform.position = SelectedCard.Position() + positionOffset;

		activate.gameObject.SetActive(ri.Activate);
		discard.gameObject.SetActive(ri.Discard);
		play.gameObject.SetActive(ri.Play);
		playAsClub.gameObject.SetActive(ri.PlayAsClub);
		playAsDiamond.gameObject.SetActive(ri.PlayAsDiamond);
		playAsHeart.gameObject.SetActive(ri.PlayAsHeart);
		playAsSpade.gameObject.SetActive(ri.PlayAsSpade);

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public ReInitialiser PrepareFor(Card newSelectedCard)
    {
		return new ReInitialiser(this, newSelectedCard);
    }

	// SetUp method should be used only once, when Player is initialised
	public void SetUp(Player player)
    {
		CardsActivatedZone = player.CardsActivated;
		CardsPlayedZone = player.CardsPlayed;
	}

	// Builder-style class for preparing a new panel configuration
	public class ReInitialiser
	{
		private SelectedCardOptionsPanel panel;

		public bool Activate { get; private set; }
		public bool Discard { get; private set; }
		public bool Play { get; private set; }
		public bool PlayAsClub { get; private set; }
		public bool PlayAsDiamond { get; private set; }
		public bool PlayAsHeart { get; private set; }
		public bool PlayAsSpade { get; private set; }
		public Card SelectedCard { get; private set; }

		public ReInitialiser(SelectedCardOptionsPanel panel, Card newSelectedCard)
        {
			this.panel = panel;
			SelectedCard = newSelectedCard;
		}

		public void Display()
        {
			panel.ConfigureAndDisplay(this);
        }

		public ReInitialiser IncludeActivate()
        {
			Activate = true;
			return this;
		}

		public ReInitialiser IncludeDiscard()
		{
			Discard = true;
			return this;
		}

		public ReInitialiser IncludePlay()
		{
			Play = true;
			return this;
		}

		public ReInitialiser IncludePlayAsClub()
		{
			PlayAsClub = true;
			return this;
		}

		public ReInitialiser IncludePlayAsDiamond()
		{
			PlayAsDiamond = true;
			return this;
		}

		public ReInitialiser IncludePlayAsHeart()
		{
			PlayAsHeart = true;
			return this;
		}

		public ReInitialiser IncludePlayAsSpade()
		{
			PlayAsSpade = true;
			return this;
		}
	}
}
