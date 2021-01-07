using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObject : MonoBehaviour
{
    private float speedModifier = 6;
    private Vector2? targetPosition = null;

    void Start()
    {

    }

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)targetPosition, Time.deltaTime * speedModifier);
            if (targetPosition.Equals((Vector2)transform.position))
            {
                targetPosition = null;
            }
        }
    }

    public void Reposition(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
