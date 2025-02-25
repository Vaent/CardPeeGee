using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StagingArea : CardZone
{
    private readonly Vector3 alternateModeOffset = new Vector3(0.72f, -2.8f);
    private List<Card> cardsInAlternateSpace = new List<Card>();
    private bool isAlternateMode;
    private Vector3 transformPosition;

    void Start()
    {
        GameState.Register(this);
        transformPosition = this.transform.position;
    }

    private IEnumerator CardMovementCoroutine()
    {
        List<Card> cardsToMove = isAlternateMode
            ? cardsInAlternateSpace
            : Cards.Where(card => !cardsInAlternateSpace.Contains(card)).ToList();
        for (var index = 0; index < cardsToMove.Count; index++)
        {
            Card card = cardsToMove[index];
            // If an encounter is in progress, avoid the encounter text, otherwise cards can extend further right
            float rightLimit = GameState.CurrentPhase is EncounterPhase ? 3.8f : 7.7f;
            float spacingFactor = (i * 1.1f < rightLimit) ? 1.1f : (rightLimit / i);
            Vector3 positionAdjustment = new Vector3(index * spacingFactor, 0, index * -0.01f);
            if (isAlternateMode) positionAdjustment += alternateModeOffset;
            CardController.MovementTracker tracker = cardsInMotion[card];
            card.MoveTo(transformPosition + positionAdjustment, tracker, CardController.Orientation.FaceUp);
            while (!tracker.completed)
            {
                yield return null;
            }
            Debug.Log("StagingArea recorded movement complete for " + card);
        }
        isAlternateMode = false;
    }

    protected override void ProcessNewCards(List<Card> newCards)
    {
        Debug.Log("StagingArea received the following cards: " + newCards.Print());
        Debug.Log("StagingArea now contains the following cards: " + Cards.Print());
        if (isAlternateMode) cardsInAlternateSpace.AddRange(newCards);
        StartCoroutine(CardMovementCoroutine());
    }

    internal void SetAlternate()
    {
        isAlternateMode = true;
    }

    public override void Unregister(Card card)
    {
        base.Unregister(card);
        cardsInAlternateSpace.Remove(card);
    }
}
