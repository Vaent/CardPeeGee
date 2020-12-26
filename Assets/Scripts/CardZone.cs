﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Every part of the game area which can have cards placed in it should implement this interface
   in a script which determines how cards are handled in that area. */
public abstract class CardZone : MonoBehaviour
{
    private List<Card> cards = new List<Card>();

    public List<Card> Cards => new List<Card>(cards);

    public void Accept(List<Card> cards)
    {
        this.cards.AddRange(cards);
        cards.ForEach(card => card.RegisterTo(this));
        ProcessNewCards(cards);
    }

    // this method allows for custom behaviour after new cards have been registered
    // typically it will trigger an animation to 'move' the cards, and may perform further logic
    protected abstract void ProcessNewCards(List<Card> cards);

    public void Unregister(Card card)
    {
        if (cards.Contains(card) && card.CurrentLocation != this)
        {
            cards.Remove(card);
        }
    }
}