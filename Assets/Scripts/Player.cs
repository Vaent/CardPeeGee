using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private readonly CharacterCard characterCard = new GameObject().AddComponent<CharacterCard>();
    private readonly Hand hand = new GameObject().AddComponent<Hand>();
    private int hp;

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

    class CharacterCard : CardZone
    {
        private static Vector2 characterCardPosition = new Vector2(-1, 1);

        protected override void ProcessNewCards(List<Card> cards)
        {
            if (cards.Count != 1) throw new System.Exception("CharacterCard can only Accept a single element List<Card>");
            cards[0].MoveToFaceUp(characterCardPosition);
        }
    }

    class Hand : CardZone
    {
        protected override void ProcessNewCards(List<Card> cards)
        {
            // TODO: determine the order of all cards now in the Hand
            // TODO: reposition all cards' sprites ; new cards move to the Hand
        }
    }
}
