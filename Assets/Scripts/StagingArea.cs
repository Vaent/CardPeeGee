using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagingArea : CardZone
{
    void Start()
    {
        GameState.Register(this);
    }

    protected override void ProcessNewCards(List<Card> cards)
    {
        Debug.Log("StagingArea received the following cards: " + string.Join(" | ", cards));
        Debug.Log("StagingArea now contains the following cards: " + string.Join(" | ", Cards));
        // TODO: move cards to staging area
    }
}
