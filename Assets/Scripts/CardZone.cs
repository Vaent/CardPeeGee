using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Every part of the game area which can have cards placed in it
   requires a script implementing this abstract class. */
public abstract class CardZone : MonoBehaviour
{
    private List<Card> cards = new List<Card>();
    protected Dictionary<Card, CardMover.MovementTracker> cardsInMotion = new Dictionary<Card, CardMover.MovementTracker>();

    public List<Card> Cards => new List<Card>(cards);

    public void Accept(List<Card> cards)
    {
        this.cards.AddRange(cards);
        cards.ForEach(card =>
        {
            card.RegisterTo(this);
            cardsInMotion.Add(card, new CardMover.MovementTracker());
        });
        ProcessNewCards(cards);
        StartCoroutine(ListenForMovement(cards));
    }

    private bool IsInMotion(Card card)
    {
        return cardsInMotion.ContainsKey(card)
            && !(cardsInMotion[card].completed);
    }

    protected IEnumerator ListenForMovement(List<Card> cards)
    {
        while (cards.Exists(card => IsInMotion(card)))
        {
            yield return null;
        }
        // validate ownership in case a card was diverted while moving
        cards.RemoveAll(card => !this.cards.Contains(card));
        Debug.Log(this + " finished moving " + string.Join(" | ", cards));
        if (cards.Count > 0) GameState.NotifyCardsReceived(this, cards);
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
        cardsInMotion.Remove(card);
    }
}
