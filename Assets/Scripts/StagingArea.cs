using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingArea : CardZone
{
    private Vector2 transformPosition;

    void Start()
    {
        GameState.Register(this);
        transformPosition = this.transform.position;
    }

    private IEnumerator CardMovementCoroutine(List<Card> cards)
    {
        for (var i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            int index = this.Cards.IndexOf(card);
            float spacingFactor = (cards.Count < 8) ? 1.1f : (7.7f / cards.Count);
            Vector2 positionAdjustment = Vector2.right * index * spacingFactor;
            // TODO: further adjustment when there are too many cards to fit on screen
            CardMover.MovementTracker tracker = cardsInMotion[card];
            card.MoveToFaceUp(transformPosition + positionAdjustment, tracker);
            while (!tracker.completed)
            {
                yield return null;
            }
            Debug.Log("StagingArea recorded movement complete for " + card);
        }
    }

    protected override void ProcessNewCards(List<Card> cards)
    {
        Debug.Log("StagingArea received the following cards: " + cards.Print());
        Debug.Log("StagingArea now contains the following cards: " + Cards.Print());
        StartCoroutine(CardMovementCoroutine(Cards));
    }
}
