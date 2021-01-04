using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingArea : CardZone
{
    private Vector2 transformPosition;

    void Start()
    {
        GameState.Register(this);
        transformPosition = GetComponent<Transform>().position;
    }

    protected override void ProcessNewCards(List<Card> cards)
    {
        Debug.Log("StagingArea received the following cards: " + string.Join(" | ", cards));
        Debug.Log("StagingArea now contains the following cards: " + string.Join(" | ", Cards));
        StartCoroutine(CardMovementCoroutine(cards));
    }

    private IEnumerator CardMovementCoroutine(List<Card> cards)
    {
        for (var i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            int index = this.Cards.IndexOf(card);
            Vector2 positionAdjustment = Vector2.right * index * 1.1f;
            // TODO: further adjustment when there are too many cards to fit on screen
            CardMover.MovementTracker tracker = new CardMover.MovementTracker();
            card.MoveToFaceUp(transformPosition + positionAdjustment, tracker);
            while (!tracker.completed)
            {
                yield return null;
            }
            Debug.Log("StagingArea recorded movement complete for " + card);
        }

        // placeholder until tracking is expanded in base class
        GameState.NotifyCardsReceived(this, cards);
    }
}
