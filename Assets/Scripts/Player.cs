using Audio;
using static CardUtil;
using Cards;
using Cards.SelectedCard;
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
            && card.CurrentLocation.Equals(hand);
    }

    public bool CanActivateAny()
    {
        return hand.Cards.Exists(card => ACE.Equals(card.Name) || JACK.Equals(card.Name));
    }

    private void CancelSelectedCard()
    {
        if (selectedCard == null) return;

        SelectableCards selectedCardLocation = (SelectableCards)selectedCard.CurrentLocation;
        selectedCardLocation.Deselect(selectedCard);
        selectedCard = null;
    }

    public bool CanConvert(Card card, params Suit[] targetSuits)
    {
        return CanConvert(card, Array.ConvertAll(targetSuits, suit => new TargetSuit(suit, true)));
    }

    public bool CanConvert(Card card, params TargetSuit[] targetSuits)
    {
        if (!IsHolding(card)) return false;

        // Consider each target suit individually as some may permit zero values while others may not
        foreach (var target in targetSuits)
        {
            if (target.Suit.Equals(card.Suit)) continue; // can't convert a card to its own suit

            if (CardsActivated.Exists(activeCard =>
                activeCard != card
                && JACK.Equals(activeCard.Name)
                && ((activeCard.Suit == card.Suit) || target.Suit.Equals(activeCard.Suit))
                && (!target.HasValue || card.NaturalValue > Card.conversionPenalty)))
            {
                return true;
            }
        }

        return false;
    }

    public bool CanUse(Card card, bool allowActivate, params TargetSuit[] playableSuits)
    {
        if (!IsHolding(card)) return false;

        return Array.Exists(playableSuits, target => target.Suit.Equals(card.Suit))
            || (allowActivate && CanActivate(card))
            || CanConvert(card, playableSuits);
    }

    public void ConfigureSelectedCardDiscard(Card newSelectedCard)
    {
        if (ConfirmCardSelection(newSelectedCard))
        {
            Select(newSelectedCard);
            selectedCardOptionsPanel.ConfigureAndDisplayDiscardOption(newSelectedCard);
        }
    }

    public void ConfigureSelectedCardOptions(Card newSelectedCard, params Suit[] playableSuits)
    {
        ConfigureSelectedCardOptions(newSelectedCard, Array.ConvertAll(playableSuits, suit => new TargetSuit(suit, true)));
    }

    public void ConfigureSelectedCardOptions(Card newSelectedCard, params TargetSuit[] playableSuits)
    {
        ConfigureSelectedCardOptions(newSelectedCard, true, playableSuits);
    }

    public void ConfigureSelectedCardOptions(Card newSelectedCard, bool allowActivate, params TargetSuit[] playableSuits)
    {
        if (!ConfirmCardSelection(newSelectedCard)) return;
        
        if (ConvertedCardRequires(newSelectedCard))
        {
            // TODO: display a message advising the card cannot be played because it was used to convert another card
            Debug.Log($"Unable to play {newSelectedCard} because it is currently converting another played card");
        }
        else if (IsHolding(newSelectedCard))
        {
            bool canActivate = allowActivate && CanActivate(newSelectedCard);
            bool canPlay = Array.Exists(playableSuits, target => target.Suit.Equals(newSelectedCard.Suit));
            TargetSuit[] convertableSuits = Array.FindAll(playableSuits, target => CanConvert(newSelectedCard, target));
            if (!canActivate && !canPlay && convertableSuits.Length == 0) return;

            Select(newSelectedCard);
            SelectedCardOptionsPanel.ReInitialiser ri = selectedCardOptionsPanel.PrepareFor(newSelectedCard);
            if (canActivate) ri.IncludeActivate();
            if (canPlay) ri.IncludePlay();
            if (convertableSuits.Length > 0) ri.IncludePlayAs(convertableSuits);
            ri.Display();
        }
    }

    private bool ConfirmCardSelection(Card newSelectedCard)
    {
        Debug.Log(newSelectedCard + " has been selected in " + newSelectedCard.CurrentLocation);
        bool isNewSelection = (newSelectedCard != selectedCard);
        CancelSelectedCard();
        return isNewSelection;
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
        Debug.Log($"Player is taking damage... initial HP {hp}, damage amount {amount}");
        SoundEffects.Play(amount switch {
            0 => SoundEffects.DamageLevel0,
            < 5 => SoundEffects.DamageLevel1,
            < 13 => SoundEffects.DamageLevel2,
            < 20 => SoundEffects.DamageLevel3,
            < 25 => SoundEffects.DamageLevel4,
            _ => SoundEffects.DamageLevel5
        });
        UpdateHP(hp - amount);
    }

    private void Deselect(Card card)
    {
        if (IsAlive() && !card.Equals(selectedCard)) throw new Exception("Attempted to deselect a card which is not selected");
        selectedCard = null;
    }

    public void Heal(int amount)
    {
        Debug.Log($"Healing Player... initial HP {hp}, heal amount {amount}");
        SoundEffects.Play(SoundEffects.HealingChant);
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

    private void Select(Card newSelectedCard)
    {
        selectedCard = newSelectedCard;
        newSelectedCard.SetHeight(1);
        newSelectedCard.Resize(1.5f);
    }

    private void UpdateHP(int newValue)
    {
        hp += newValue.CompareTo(hp);
        if (hp <= 0)
        {
            hpDisplay.text = "DEAD";
            Text.TextManager.TearDownDisplayedText();
            GameState.PlayerDied();
            return;
            // TODO: implement death properly
        }
        else
        {
            hpDisplay.text = $"HP: {hp}";
        }

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

        protected abstract void AdjustPositions();

        public void Deselect(Card card)
        {
            var allCards = Cards;
            Sort(allCards);
            var i = allCards.IndexOf(card);
            card.SetHeight(i * 0.01f);
            card.Resize(1);
            selectedCardOptionsPanel.Hide();
        }

        public override void Unregister(Card card)
        {
            player.Deselect(card);
            base.Unregister(card);
            AdjustPositions();
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

        protected override void AdjustPositions()
        {
            var allCards = Cards;
            Sort(allCards);
            float spacingFactor = (allCards.Count < 5) ? 1.1f : (3.7f / (allCards.Count - 1));
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector3 positionAdjustment = new Vector3(i * spacingFactor, 0, i * -0.01f);
                CardController.MovementTracker tracker = cardsInMotion[card];
                card.MoveTo(leftPosition + positionAdjustment, tracker, CardController.Orientation.FaceUp);
            }
        }
    }

    class CardsPlayedZone : CardZone
    {
        private static readonly Vector3 leftPosition = new Vector3(-3.87f, 3.85f, 0);

        protected override void ProcessNewCards(List<Card> newCards)
        {
            var allCards = Cards;
            Sort(allCards);
            float spacingFactor = (allCards.Count < 5) ? 1.1f : (3.7f / (allCards.Count - 1));
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector3 positionAdjustment = new Vector3(i * spacingFactor, 0, i * -0.01f);
                CardController.MovementTracker tracker = cardsInMotion[card];
                card.MoveTo(leftPosition + positionAdjustment, tracker, CardController.Orientation.FaceUp);
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
            newCards[0].MoveTo(characterCardPosition, tracker, CardController.Orientation.FaceUp);
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

        protected override void AdjustPositions()
        {
            var allCards = Cards;
            Sort(allCards);
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
                card.MoveTo(leftPosition + positionAdjustment, tracker, CardController.Orientation.FaceUp);
            }
        }
    }
}
