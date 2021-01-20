using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Generic class for managing a card's GameObject.
The Card class complements this class by implementing game logic. */
public class CardController : MonoBehaviour
{
    // component references
    public Sprite back;
    public SpriteRenderer cardRenderer;

    // state variables
    private Card cardScript;
    private Sprite face;
    private float speedModifier = 1;
    private bool rotate = false;
    private Vector3? targetPosition = null;

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)targetPosition, Time.deltaTime * speedModifier);
            if (targetPosition.Equals(transform.position))
            {
                targetPosition = null;
                if (rotate == true)
                {
                    Flip();
                    rotate = false;
                }
            }
            // TODO: rotate gradually instead of flipping at destination
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked " + cardScript);
        cardScript.DoClicked();
    }

// MonoBehaviour methods end / custom methods begin

    private void DetermineRotation(bool endFaceUp)
    {
        rotate = (cardRenderer.sprite == back) ? endFaceUp : !endFaceUp;
    }

    public void Flip()
    {
        cardRenderer.sprite = (cardRenderer.sprite == face) ? back : face;
    }

    public void GoTo(Vector3 targetPosition)
    {
        GoTo(targetPosition, null, (cardRenderer.sprite == face));
    }

    public void GoTo(Vector3 targetPosition, CardController.MovementTracker tracker, bool endFaceUp)
    {
        gameObject.SetActive(true);
        StartCoroutine(MoveCoroutine(targetPosition, tracker, endFaceUp));
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        KillMovement();
    }

    public void KillMovement()
    {
        targetPosition = null;
        rotate = false;
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, CardController.MovementTracker tracker, bool endFaceUp)
    {
        while (this.targetPosition != null) yield return null;

        this.speedModifier = Vector3.Distance(transform.position, targetPosition) * 2;
        this.targetPosition = targetPosition;
        DetermineRotation(endFaceUp);
        while (!targetPosition.Equals(transform.position)) yield return null;
        Debug.Log("Finished moving " + cardScript.ToStringVerbose());
        if (tracker != null) tracker.completed = true;
    }

    public void Register(Card cardScript, Sprite face)
    {
        this.cardScript = cardScript;
        this.face = face;
    }

    public void Resize(float newScale)
    {
        transform.localScale = Vector3.one * newScale;
    }

    public void SetHeight(float newHeight)
    {
        // N.B. camera is positioned on the negative z-axis, pointed toward zero
        newHeight = newHeight * -1;
        transform.position = new Vector3(transform.position.x, transform.position.y, newHeight);
    }

    public void TurnFaceDown()
    {
        cardRenderer.sprite = back;
    }

    public void TurnFaceUp()
    {
        cardRenderer.sprite = face;
        // TODO: if card has been converted, use convertedFace instead of face
        // NB the above will likely never apply here so is not a priority
    }

    public class MovementTracker
    {
        public bool completed = false;
    }
}
