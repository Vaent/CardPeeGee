using ExtensionMethods;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingArea : CardZone
{
    private Vector3 transformPosition;

    void Start()
    {
        GameState.Register(this);
        transformPosition = this.transform.position;
    }

    private IEnumerator CardMovementCoroutine(List<Card> cardsToMove)
    {
        for (var i = 0; i < cardsToMove.Count; i++)
        {
            Card card = cardsToMove[i];
            int index = Cards.IndexOf(card);
            float spacingFactor = (cardsToMove.Count < 8) ? 1.1f : (7.7f / cardsToMove.Count);
            Vector3 positionAdjustment = new Vector3(index * spacingFactor, 0, index * -0.01f);
            CardController.MovementTracker tracker = cardsInMotion[card];
            card.MoveTo(transformPosition + positionAdjustment, tracker, CardController.Orientation.FaceUp);
            while (!tracker.completed)
            {
                yield return null;
            }
            Debug.Log("StagingArea recorded movement complete for " + card);
        }
    }

    protected override void ProcessNewCards(List<Card> newCards)
    {
        Debug.Log("StagingArea received the following cards: " + newCards.Print());
        Debug.Log("StagingArea now contains the following cards: " + Cards.Print());
        StartCoroutine(CardMovementCoroutine(Cards));
    }
}
