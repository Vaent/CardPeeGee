using ExtensionMethods;
using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Every part of the game area which can have cards placed in it
   requires a script implementing this abstract class. */
public abstract class CardZone : MonoBehaviour
{
    private List<Card> cards = new List<Card>();
    protected Dictionary<Card, CardController.MovementTracker> cardsInMotion = new Dictionary<Card, CardController.MovementTracker>();

    public List<Card> Cards => new List<Card>(cards);

    public void Accept(Card newCard)
    {
        Accept(new List<Card>(){ newCard });
    }

    public void Accept(List<Card> newCards)
    {
        this.cards.AddRange(newCards);
        newCards.ForEach(card =>
        {
            card.RegisterTo(this);
            cardsInMotion.Add(card, new CardController.MovementTracker());
        });
        ProcessNewCards(newCards);
        StartCoroutine(ListenForMovement(newCards));
    }

    public bool Contains(Card card)
    {
        return cards.Contains(card);
    }

    public bool Exists(Predicate<Card> predicate)
    {
        return cards.Exists(predicate);
    }

    protected bool IsInMotion(Card card)
    {
        return cardsInMotion.ContainsKey(card)
            && !(cardsInMotion[card].completed);
    }

    protected IEnumerator ListenForMovement(List<Card> movingCards)
    {
        while (movingCards.Exists(card => IsInMotion(card)))
        {
            yield return null;
        }
        // validate ownership in case a card was diverted while moving
        movingCards.RemoveAll(card => !this.cards.Contains(card));
        if (movingCards.Count > 0)
        {
            Debug.Log(this + " finished moving " + movingCards.Print());
            GameState.NotifyCardsReceived(this, movingCards);
            MovedCards(movingCards);
        }
    }

    // empty method which can optionally be overridden by CardZone implementations
    protected virtual void MovedCards(List<Card> movingCards) { }

    /* This method allows for custom behaviour after new cards have been registered.
    Typically it will trigger an animation to 'move' the cards,
    and may perform further logic. */
    protected abstract void ProcessNewCards(List<Card> newCards);

    // Unregister(card) is virtual to allow additional checks and cleanup specific to implementations
    // overrides to this method should always include a call to `base.Unregister(card)`
    public virtual void Unregister(Card card)
    {
        if (card.CurrentLocation == this) return;

        if (cards.Contains(card))
        {
            cards.Remove(card);
        }
        cardsInMotion.Remove(card);
    }
}
