using ExtensionMethods;
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
        }
    }

    /* CardZones will need to specify card selection behaviour
    e.g. when a card in the player's hand is selected,
    Play/Activate options become available.
    Implementations are allowed to be empty (no action taken). */
    public abstract void NotifySelectionByUser(Card selectedCard);

    /* This method allows for custom behaviour after new cards have been registered.
    Typically it will trigger an animation to 'move' the cards,
    and may perform further logic. */
    protected abstract void ProcessNewCards(List<Card> newCards);

    public void Unregister(Card card)
    {
        if (card.CurrentLocation == this) return;

        if (cards.Contains(card))
        {
            cards.Remove(card);
        }
        cardsInMotion.Remove(card);
    }
}
