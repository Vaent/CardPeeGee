using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private readonly CharacterCardZone characterCard = new GameObject().AddComponent<CharacterCardZone>();
    private readonly HandZone hand = new GameObject().AddComponent<HandZone>();
    private int hp;

    public CardZone CharacterCard => characterCard;
    public CardZone Hand => hand;

    public Player(Card characterCard)
    {
        Debug.Log("Creating new Player from " + characterCard);
        this.characterCard.Accept(new List<Card>{characterCard});
    }

    public void AddToHand(List<Card> cards)
    {
        hand.Accept(cards);
    }

    public void Damage(int amount)
    {
        hp -= amount;
        // TODO: check for death
    }

    public void Heal(int amount)
    {
        Debug.Log("Healing Player... initial HP " + hp + ", heal amount " + amount);
        hp += amount;
        Debug.Log("Player has " + hp + " HP after healing");
        // TODO: check for victory
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    class CharacterCardZone : CardZone
    {
        private static Vector2 characterCardPosition = new Vector2(-1, 1);

        protected override void ProcessNewCards(List<Card> cards)
        {
            if (Cards.Count != 1) throw new System.Exception("CharacterCardZone can only contain a single element, it now contains " + Cards.Print());
            CardMover.MovementTracker tracker = cardsInMotion[cards[0]];
            cards[0].MoveToFaceUp(characterCardPosition, tracker);
        }
    }

    class HandZone : CardZone
    {
        private static Vector2 handPosition = new Vector2(0, -3.8f);
        private static HandObject leftHand;
        private static HandObject rightHand;

        void Start()
        {
            leftHand = GameObject.Find("LeftHand").GetComponent<HandObject>();
            rightHand = GameObject.Find("RightHand").GetComponent<HandObject>();
        }

        protected override void ProcessNewCards(List<Card> cards)
        {
            Debug.Log("Hand received " + cards.Print());
            var allCards = Cards;
            CardUtil.Sort(allCards);
            Debug.Log("Hand now contains " + allCards.Print());

            Vector2 leftPosition = handPosition + Vector2.left * 0.55f * allCards.Count;
            Vector2 rightPosition = handPosition + Vector2.right * 0.55f * allCards.Count;
            leftHand.Reposition(leftPosition);
            rightHand.Reposition(rightPosition);
            // TODO: refine positioning when there are too many cards for the default width
            for (var i = 0; i < allCards.Count; i++)
            {
                Card card = allCards[i];
                Vector2 positionAdjustment = Vector2.right * (i + 0.5f) * 1.1f;
                CardMover.MovementTracker tracker = cardsInMotion[card];
                card.MoveToFaceUp(leftPosition + positionAdjustment, tracker);
            }
        }
    }
}
