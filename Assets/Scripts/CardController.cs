using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/* Generic class for managing a card's GameObject.
The Card class complements this class by implementing game logic. */
public class CardController : MonoBehaviour, IPointerClickHandler
{
    // component references
    public Sprite back;
    public SpriteRenderer cardRenderer;
#if UNITY_EDITOR
    public int stackedCardOrder;
#endif

    // state variables
    private Card cardScript;
    private Sprite face;
    private float speedModifier = 1;
    private Orientation? targetOrientation = null;
    private Vector3? targetPosition = null;

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, (Vector3)targetPosition, Time.deltaTime * speedModifier);
            if (targetPosition.Equals(transform.position))
            {
                targetPosition = null;
            }
        }
        if (targetOrientation != null)
        {
            transform.Rotate(Vector3.right, Time.deltaTime * 360);
            ApplySpriteBasedOnOrientation();
            CheckOrientationAgainstTarget();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked " + cardScript);
        GameController.RegisterInteractionWith(cardScript);
    }

// MonoBehaviour methods end / custom methods begin

    public void ApplyColor(Color color)
    {
        cardRenderer.color = color;
    }

    private void ApplySpriteBasedOnOrientation()
    {
        Sprite expected = (transform.forward.z >= 0) ? face : back;
        /* Assigning cardRenderer.sprite is presumed to be more expensive than reading its value,
           so is only done when necessary, since this method is called on every Update() when rotating.
           The assumption has not been tested and is subject to review. */
        if (cardRenderer.sprite != expected) cardRenderer.sprite = expected;
    }

    public void ChangeFace(Sprite newSprite)
    {
        face = newSprite;
        if (cardRenderer.sprite != back) cardRenderer.sprite = face;
    }

    private void CheckOrientationAgainstTarget()
    {
        if (targetOrientation == null) return;

        if (transform.forward.z == (int)targetOrientation)
        {
            // transform orientation matches target, no further change needed
            targetOrientation = null;
        }
        else if ((transform.forward.z * (int)targetOrientation > 0) && (transform.up.z * (int)targetOrientation > 0))
        {
            // transform has rotated just beyond target orientation
            transform.rotation.SetLookRotation(Vector3.forward * (int)targetOrientation);
            targetOrientation = null;
        }
    }

    public void GoTo(Vector3 targetPosition, MovementTracker tracker = null, Orientation? targetOrientation = null)
    {
        gameObject.SetActive(true);
        StartCoroutine(MoveCoroutine(targetPosition, tracker, targetOrientation));
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        KillMovement();
    }

    public void KillMovement()
    {
        targetPosition = null;
        targetOrientation = null;
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, MovementTracker tracker, Orientation? targetOrientation)
    {
        while (this.targetPosition != null) yield return null;

        this.speedModifier = Vector3.Distance(transform.position, targetPosition) * 2;
        this.targetPosition = targetPosition;
        // rotation may continue following previous movement if targetOrientation not yet reached; therefore it should not be nullified
        if (targetOrientation != null) this.targetOrientation = targetOrientation;
        CheckOrientationAgainstTarget();
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
        newHeight *= -1;
        transform.position = new Vector3(transform.position.x, transform.position.y, newHeight);
    }

// nested types

    public enum Orientation
    {
        FaceDown = -1,
        FaceUp = 1
    }

    public class MovementTracker
    {
        public bool completed = false;
    }
}
