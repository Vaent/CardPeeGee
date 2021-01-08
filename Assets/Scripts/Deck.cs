using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardZone
{
    public StagingArea stagingArea;

    void Start()
    {
        GameState.Register(this);
        CardUtil.NewPack(this);
    }

    void OnMouseDown()
    {
        GameState.Next();
    }

    public void DealCards(int count)
    {
        DealCards(stagingArea, count);
    }

    public void DealCards(CardZone target, int count)
    {
        Debug.Log("Deal " + count + " cards to " + target);
        target.Accept(DrawCards(count));
    }

    // this method will select `count` cards randomly from the deck and return them to the caller
    // the cards remain in the deck until registered to another CardZone
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = new List<Card>();
        List<Card> cardsInDeck = Cards;
        var rand = new System.Random();
        for (var i = 0; i < count; i++)
        {
            var randomIndex = rand.Next(cardsInDeck.Count);
            drawnCards.Add(cardsInDeck[randomIndex]);
            cardsInDeck.RemoveAt(randomIndex);
        }
        return drawnCards;
    }

    protected override void ProcessNewCards(List<Card> cards)
    {
        Debug.Log("Cards were returned to the Deck: " + cards.Print());
        Debug.Log("Deck now contains the following: " + Cards.Print());
        cards.ForEach(card =>
        {
            CardMover.MovementTracker tracker = cardsInMotion[card];
            card.MoveToFaceDown(this.transform.position, tracker);
        });
    }
}
