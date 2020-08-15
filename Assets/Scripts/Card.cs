using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private static readonly GameObject abstractCard = Resources.Load<GameObject>("Models/Abstract Card");
    private static readonly Sprite back = Resources.Load<Sprite>("Graphics/Sprites/Card Back");

    // state variables
    // convertedXxx will apply when e.g. a Jack is used to temporarily change the suit of a played card
    private Sprite convertedFace;
    private Suit? convertedSuit;
    private int? convertedValue;
    // initial assigned values cannot be changed
    private readonly GameObject cardObject;
    private readonly Sprite face;
    private readonly Suit suit;
    private readonly int value;

    // accessors - these use converted values if set, otherwise base values
    public Suit Suit => (convertedSuit == null) ? suit : (Suit)convertedSuit;
    public int Value => (convertedValue == null) ? value : (int)convertedValue;

    public Card(Sprite face)
    {
        this.face = face;
        string[] filenameParts = face.name.Split(' ');
        this.suit = (Suit)Suit.Parse(typeof(Suit), filenameParts[0]);
        this.value = int.Parse(filenameParts[1]);
        cardObject = MonoBehaviour.Instantiate(abstractCard);
    }

    // converting the card's suit also decreases its effective value by 2
    public void Convert(Suit newSuit)
    {
        convertedSuit = newSuit;
        Convert(value - 2);
    }

    public void Convert(int newValue)
    {
        convertedValue = newValue;
    }

    public void ResetProperties()
    {
        convertedSuit = null;
        convertedValue = null;
    }

    public void TurnFaceDown()
    {
        cardObject.GetComponent<SpriteRenderer>().sprite = back;
    }

    public void TurnFaceUp()
    {
        cardObject.GetComponent<SpriteRenderer>().sprite = face;
    }
}
