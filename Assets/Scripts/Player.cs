using static Constant;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Player
{
    private static GameObject selectedCardOptions = GameObject.Find("Selected Card Options");
    private static SelectedCardOptionsPanel selectedCardOptionsPanel = selectedCardOptions.GetComponent<SelectedCardOptionsPanel>();

    // readonly references representing static game elements
    private readonly CardsActivatedZone cardsActivated = new GameObject().AddComponent<CardsActivatedZone>();
    private readonly CardsPlayedZone cardsPlayed = new GameObject().AddComponent<CardsPlayedZone>();
    private readonly CharacterCardZone characterCard = new GameObject().AddComponent<CharacterCardZone>();
    private readonly HandZone hand = new GameObject().AddComponent<HandZone>();
    private readonly TextMesh hpDisplay;

    private int hp;
    private Card selectedCard;

    public CardZone CardsActivated => cardsActivated;
    public CardZone CardsPlayed => cardsPlayed;
    public CardZone CharacterCard => characterCard;
    public CardZone Hand => hand;

    public Player(Card characterCard, TextMesh hpDisplay)
    {
        Debug.Log("Creating new Player from " + characterCard);
        this.characterCard.Accept(characterCard);
        this.hpDisplay = hpDisplay;
        hand.Register(this);
        cardsActivated.Register(this);
        selectedCardOptionsPanel.SetUp(this);
    }

    public void AddToHand(List<Card> cards)
    {
        hand.Accept(cards);
    }

    public bool CanActivate(Card card)
    {
        return (ACE.Equals(card.Name) || JACK.Equals(card.Name))
            && !card.CurrentLocation.Equals(CardsActivated);
    }

    public void ConfigureSelectedCardDiscard(Card selectedCard)
    {
        // TODO: share logic from ConfigureOptions method to record new selection, resize/raise/lower cards etc
        selectedCardOptionsPanel.ConfigureAndDisplayDiscardOption(selectedCard);
    }

    public void ConfigureSelectedCardOptions(Card newSelectedCard, params Suit[] playableSuits)
    {
        Debug.Log(newSelectedCard + " has been selected in " + newSelectedCard.CurrentLocation);

        if (selectedCard != null)
        {
            SelectableCards selectedCardLocation = (SelectableCards)selectedCard.CurrentLocation;
            selectedCardLocation.Deselect(selectedCard);
        }

        if (newSelectedCard.Equals(selectedCard))
        {
            // reselection => deselection
            selectedCard = null;
        }
        else if (ConvertedCardRequires(newSelectedCard))
        {
            // TODO: display a message advising the card cannot be played because it was used to convert another card
        }
        else
        {
            selectedCard = newSelectedCard;
            newSelectedCard.SetHeight(1);
            newSelectedCard.Resize(1.5f);
            SelectedCardOptionsPanel.ReInitialiser ri = selectedCardOptionsPanel.PrepareFor(newSelectedCard);
            if (CanActivate(newSelectedCard)) ri.IncludeActivate();
            if (Array.Exists(playableSuits, suit => newSelectedCard.Suit == suit)) ri.IncludePlay();
            if (Array.Exists(playableSuits, suit => newSelectedCard.Suit != suit))
            {
                if (cardsActivated.Exists(card => JACK.Equals(card.Name) && card.Suit == newSelectedCard.Suit))
                {
                    ri.IncludePlayAs(Array.FindAll(playableSuits, suit => newSelectedCard.Suit != suit));
                }
                else
                {
                    Suit[] convertableSuits = Array.FindAll(playableSuits, suit =>
                        suit != newSelectedCard.Suit
                        && cardsActivated.Exists(card => JACK.Equals(card.Name) && card.Suit == suit));
                    if (convertableSuits.Length > 0) ri.IncludePlayAs(convertableSuits);
                }
            }
            ri.Display();
        }
    }

    private bool ConvertedCardRequires(Card newSelectedCard)
    {
        return JACK.Equals(newSelectedCard.Name)
            && CardsActivated.Equals(newSelectedCard.CurrentLocation)
            && CardsPlayed.Exists(card =>
                card.IsConvertedSuit()
                && (card.Suit == newSelectedCard.Suit || card.NaturalSuit == newSelectedCard.Suit)
                && !CardsActivated.Exists(activeCard =>
                    JACK.Equals(activeCard.Name)
                    && (card.Suit == activeCard.Suit || card.NaturalSuit == activeCard.Suit)
                    && !activeCard.Equals(newSelectedCard)));
    }

    public void Damage(int amount)
    {
        // TODO: determine and play appropriate audio clip
        // note that Damage(0) calls result in the "phew" audio clip
        Debug.Log($"Player is taking damage... initial HP {hp}, damage amount {amount}");
        UpdateHP(hp - amount);
    }

    private void Deselect(Card card)
    {
        if (!card.Equals(selectedCard)) throw new Exception("Attempted to deselect a card which is not selected");
        selectedCard = null;
    }

    public void Heal(int amount)
    {
        // TODO: play audio clip
        Debug.Log($"Healing Player... initial HP {hp}, heal amount {amount}");
        UpdateHP(hp + amount);
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    public bool IsHolding(Card card)
    {
        return hand.Contains(card) || cardsActivated.Contains(card);
    }

    public void UpdateHP(int newValue)
    {
        hp += newValue.CompareTo(hp);
        hpDisplay.text = $"HP: {hp}";
        // TODO: check for death

        if (newValue == hp)
        {
            Debug.Log($"Player has {hp} HP after healing/damage applied");
            // TODO: check for victory
        }
        else
        {
            Timer.DelayThenInvoke(0.05f, this.UpdateHP, newValue);
        }
    }

    abstract class SelectableCards : CardZone
    {
        protected Player player;

        public void Register(Player player)
        {
            this.player = player;
        }

        public void Deselect(Card card)
        {
            var allCards = Cards;
            CardUtil.Sort(allCards);
            var i = allCards.IndexOf(card);
            card.SetHeight(i * 0.01f);
            card.Resize(1);
            selectedCardOptionsPanel.Hide();
        }
    }

    class CardsActivatedZone : SelectableCards
    {
        private static readonly Vector3 leftPosition = new Vector3(-3.87f, -1.75f, 0);

        protected override void ProcessNewCards(List<Card> newCards)
        {
            AdjustPositions();
            // TODO: if an Ace is activated during an encounter, check whether it affects any played cards
        }

        private void AdjustPositions()
        {
            var allCards = Cards;
            CardUtil.Sort(allCards);
            float spacingFactor = (allCards.Count < 5) ? 1.1f : (3.7f / (allCards.Count - 1));
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector3 positionAdjustment = new Vector3(i * spacingFactor, 0, i * -0.01f);
                CardController.MovementTracker tracker = cardsInMotion[card];
                card.MoveTo(leftPosition + positionAdjustment, tracker, true);
            }
        }

        public override void Unregister(Card card)
        {
            player.Deselect(card);
            base.Unregister(card);
            AdjustPositions();
        }
    }

    class CardsPlayedZone : CardZone
    {
        private static readonly Vector3 leftPosition = new Vector3(-3.87f, 3.85f, 0);

        protected override void ProcessNewCards(List<Card> newCards)
        {
            var allCards = Cards;
            CardUtil.Sort(allCards);
            float spacingFactor = (allCards.Count < 5) ? 1.1f : (3.7f / (allCards.Count - 1));
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector3 positionAdjustment = new Vector3(i * spacingFactor, 0, i * -0.01f);
                CardController.MovementTracker tracker = cardsInMotion[card];
                card.MoveTo(leftPosition + positionAdjustment, tracker, true);
            }
        }
    }

    class CharacterCardZone : CardZone
    {
        private static Vector3 characterCardPosition = new Vector3(-1, 1, 0);

        protected override void ProcessNewCards(List<Card> newCards)
        {
            if (Cards.Count != 1) throw new Exception("CharacterCardZone can only contain a single element, it now contains " + Cards.Print());
            CardController.MovementTracker tracker = cardsInMotion[newCards[0]];
            newCards[0].MoveTo(characterCardPosition, tracker, true);
        }
    }

    class HandZone : SelectableCards
    {
        private static readonly Vector3 handPosition = new Vector3(0, -3.8f, 0);
        private static HandObject leftHand;
        private static HandObject rightHand;

        void Start()
        {
            leftHand = GameObject.Find("LeftHand").GetComponent<HandObject>();
            rightHand = GameObject.Find("RightHand").GetComponent<HandObject>();
        }

        protected override void ProcessNewCards(List<Card> newCards)
        {
            Debug.Log("Hand received " + newCards.Print());
            AdjustPositions();
        }

        private void AdjustPositions()
        {
            var allCards = Cards;
            CardUtil.Sort(allCards);
            Debug.Log("Hand now contains " + allCards.Print());
            float spacingFactor = (allCards.Count < 7) ? 1.1f : (6.6f / (allCards.Count));
            Vector3 leftPosition = handPosition + Vector3.left * 0.55f * Min(6, allCards.Count);
            Vector3 rightPosition = handPosition + Vector3.right * 0.55f * Min(6, allCards.Count);
            leftHand.Reposition(leftPosition);
            rightHand.Reposition(rightPosition);
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector3 positionAdjustment = new Vector3((i + 0.5f) * spacingFactor, 0, i * -0.01f);
                CardController.MovementTracker tracker = cardsInMotion[card];
                card.MoveTo(leftPosition + positionAdjustment, tracker, true);
            }
        }

        public override void Unregister(Card card)
        {
            player.Deselect(card);
            base.Unregister(card);
            AdjustPositions();
        }
    }
}
