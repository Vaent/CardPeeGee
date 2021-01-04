using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private readonly CharacterCardZone characterCard = new GameObject().AddComponent<CharacterCardZone>();
    private readonly HandZone hand = new GameObject().AddComponent<HandZone>();
    private int hp;

    public CardZone CharacterCard => characterCard;

    public Player(Card characterCard)
    {
        Debug.Log("Creating new Player from " + characterCard);
        this.characterCard.Accept(new List<Card>{characterCard});
    }

    public void AddToHand(List<Card> cards)
    {
        hand.Accept(cards);
    }

    public void Damage(int amount)
    {
        hp -= amount;
        // TODO: check for death
    }

    public void Heal(int amount)
    {
        Debug.Log("Healing Player... initial HP " + hp + ", heal amount " + amount);
        hp += amount;
        Debug.Log("Player has " + hp + " HP after healing");
        // TODO: check for victory
    }

    public bool IsAlive()
    {
        return hp > 0;
    }

    class CharacterCardZone : CardZone
    {
        private static Vector2 characterCardPosition = new Vector2(-1, 1);

        protected override void ProcessNewCards(List<Card> cards)
        {
            if (Cards.Count != 1) throw new System.Exception("CharacterCardZone can only contain a single element, it now contains " + Cards);
            StartCoroutine(CardMovementCoroutine(cards[0]));
        }

        private IEnumerator CardMovementCoroutine(Card card)
        {
            CardMover.MovementTracker tracker = new CardMover.MovementTracker();
            card.MoveToFaceUp(characterCardPosition, tracker);
            while (!tracker.completed)
            {
                yield return null;
            }

            // placeholder until tracking is expanded in base class
            GameState.NotifyCardsReceived(this, Cards);
        }
    }

    class HandZone : CardZone
    {
        protected override void ProcessNewCards(List<Card> cards)
        {
            // TODO: determine the order of all cards now in the Hand
            // TODO: reposition all cards' sprites ; new cards move to the Hand
        }
    }
}
