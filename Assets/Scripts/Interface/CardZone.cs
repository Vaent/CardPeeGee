using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Every part of the game area which can have cards placed in it should implement this interface
   in a script which determines how cards are handled in that area. */
public interface CardZone
{
    // this method allows cards to be 'sent to' the zone
    // typically it will trigger an animation to 'move' the cards, and register them in a property
    void Accept(List<Card> cards);
}
