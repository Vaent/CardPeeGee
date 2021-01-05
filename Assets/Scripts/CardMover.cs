using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    private Card cardScript;
    private float speedModifier = 1;
    private bool rotate = false;
    private Vector2? targetPosition = null;

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetPosition, Time.deltaTime * speedModifier);
            if (targetPosition.Equals((Vector2)transform.position))
            {
                targetPosition = null;
                if (rotate == true)
                {
                    cardScript.Flip();
                    rotate = false;
                }
            }
            // TODO: rotate gradually instead of flipping at destination
        }
    }

    public void GoTo(Vector2 targetPosition, bool rotate)
    {
        GoTo(targetPosition, rotate, null);
    }

    public void GoTo(Vector2 targetPosition, bool rotate, CardMover.MovementTracker tracker)
    {
        if (this.rotate) rotate = !rotate;
        StartCoroutine(MoveCoroutine(targetPosition, rotate, tracker));
    }

    public void KillMovement()
    {
        targetPosition = null;
        rotate = false;
    }

    private IEnumerator MoveCoroutine(Vector2 targetPosition, bool rotate, CardMover.MovementTracker tracker)
    {
        // Debug.Log("Checking for null targetPosition... " + this.targetPosition);
        while (this.targetPosition != null) yield return null;
        // Debug.Log("Ready to start new movement");

        this.speedModifier = Vector2.Distance(transform.position, targetPosition) * 2;
        this.targetPosition = targetPosition;
        this.rotate = rotate;
        while (!targetPosition.Equals(transform.position)) yield return null;
        Debug.Log("Finished moving " + cardScript.ToStringVerbose());
        if (tracker != null) tracker.completed = true;
    }

    public void RegisterController(Card cardScript)
    {
        this.cardScript = cardScript;
    }

    public class MovementTracker
    {
        public bool completed = false;
    }
}
