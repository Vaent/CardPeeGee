using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Player
{
    private readonly CharacterCardZone characterCard = new GameObject().AddComponent<CharacterCardZone>();
    private readonly HandZone hand = new GameObject().AddComponent<HandZone>();
    private int hp;
    private TextMesh hpDisplay;

    public CardZone CharacterCard => characterCard;
    public CardZone Hand => hand;

    public Player(Card characterCard, TextMesh hpDisplay)
    {
        Debug.Log("Creating new Player from " + characterCard);
        this.characterCard.Accept(new List<Card>{characterCard});
        this.hpDisplay = hpDisplay;
    }

    public void AddToHand(List<Card> cards)
    {
        hand.Accept(cards);
    }

    public void Damage(int amount)
    {
        UpdateHP(hp - amount);
        // TODO: check for death
    }

    public void Heal(int amount)
    {
        Debug.Log("Healing Player... initial HP " + hp + ", heal amount " + amount);
        UpdateHP(hp + amount);
        Debug.Log("Player has " + hp + " HP after healing");
        // TODO: check for victory
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    public void UpdateHP(int newValue)
    {
        if (newValue == hp)
        {
            return;
        }
        else if (newValue > hp)
        {
            hp++;
        }
        else if (newValue < hp)
        {
            hp--;
        }
        hpDisplay.text = "HP: " + hp;
        if (hp != newValue) Timer.DelayThenInvoke(0.05f, this.UpdateHP, newValue);
    }

    class CharacterCardZone : CardZone
    {
        private static Vector3 characterCardPosition = new Vector3(-1, 1, 0);

        public override void NotifySelectionByUser(Card selectedCard) { }

        protected override void ProcessNewCards(List<Card> newCards)
        {
            if (Cards.Count != 1) throw new System.Exception("CharacterCardZone can only contain a single element, it now contains " + Cards.Print());
            CardController.MovementTracker tracker = cardsInMotion[newCards[0]];
            newCards[0].MoveToFaceUp(characterCardPosition, tracker);
        }
    }

    class HandZone : CardZone
    {
        private static Vector3 handPosition = new Vector3(0, -3.8f, 0);
        private static HandObject leftHand;
        private static HandObject rightHand;
        private Card selectedCard;

        void Start()
        {
            leftHand = GameObject.Find("LeftHand").GetComponent<HandObject>();
            rightHand = GameObject.Find("RightHand").GetComponent<HandObject>();
        }

        public override void NotifySelectionByUser(Card newSelectedCard)
        {
            Debug.Log("Clicked on " + newSelectedCard + " from hand");
            if (this.selectedCard != null)
            {
                var allCards = Cards;
                CardUtil.Sort(allCards);
                var i = allCards.IndexOf(this.selectedCard);
                this.selectedCard.RaiseTo(i * 0.01f);
                this.selectedCard.Resize(1);
            }

            if (newSelectedCard.Equals(this.selectedCard))
            {
                // reselection => deselection
                this.selectedCard = null;
            }
            else
            {
                this.selectedCard = newSelectedCard;
                newSelectedCard.RaiseTo(1);
                newSelectedCard.Resize(1.5f);
            }
        }

        protected override void ProcessNewCards(List<Card> newCards)
        {
            Debug.Log("Hand received " + newCards.Print());
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
                card.MoveToFaceUp(leftPosition + positionAdjustment, tracker);
            }
        }
    }
}
