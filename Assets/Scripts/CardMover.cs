using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    // component references
    public Transform cardTransform;

    // state variables
    private Card cardScript;
    private float speedModifier = 1;
    private bool rotate = false;
    private Vector2 targetPosition;

    public void RegisterController(Card cardScript)
    {
        this.cardScript = cardScript;
    }

    void Update()
    {
        cardTransform.position = Vector2.MoveTowards(cardTransform.position, targetPosition, Time.deltaTime * speedModifier);
        if (rotate == true && targetPosition.Equals(cardTransform.position))
        {
            cardScript.Flip();
            rotate = false;
        }
        // TODO: rotate gradually instead of flipping at destination
    }

    public void GoTo(Vector2 targetPosition, bool rotate)
    {
        this.speedModifier = Vector2.Distance(cardTransform.position, targetPosition) * 2;
        this.targetPosition = targetPosition;
        this.rotate = rotate;
    }
}
