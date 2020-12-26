using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private const int ConversionPenalty = 2;

    private static readonly GameObject abstractCard = Resources.Load<GameObject>("Models/Abstract Card");
    private static readonly Sprite back = Resources.Load<Sprite>("Graphics/Sprites/Card Back");

    // state variables
    private CardZone currentLocation;
    // convertedXxx will apply when e.g. a Jack is used to temporarily change the suit of a played card
    private Sprite convertedFace;
    private Suit? convertedSuit;
    private int? convertedValue;
    // initial assigned values cannot be changed
    private readonly GameObject cardObject;
    private readonly Sprite face;
    private readonly string name;
    private readonly Suit suit;
    private readonly int value;

    // accessors
    public CardZone CurrentLocation => currentLocation;
    // these use converted values if set, otherwise base values
    public Suit Suit => convertedSuit ?? suit;
    public int Value => convertedValue ?? value;

    public Card(Sprite face, CardZone startingLocation)
    {
        this.face = face;
        string[] filenameParts = face.name.Split(' ');
        this.suit = (Suit)Suit.Parse(typeof(Suit), filenameParts[0]);
        this.value = int.Parse(filenameParts[1]);
        this.name = (filenameParts.Length > 2) ? filenameParts[2] : filenameParts[1];
        cardObject = MonoBehaviour.Instantiate(abstractCard);

        this.currentLocation = startingLocation;
        startingLocation.Accept(new List<Card>{this});
    }

    // converting the card's suit also decreases its effective value by 2
    public void Convert(Suit newSuit)
    {
        convertedSuit = newSuit;
        Convert(value - ConversionPenalty);
    }

    public void Convert(int newValue)
    {
        convertedValue = newValue;
        // TODO: calculate and display the convertedFace sprite
    }

    public void Hide()
    {
        cardObject.SetActive(false);
    }

    public void MoveTo(Vector2 newPosition)
    {
        cardObject.SetActive(true);
        cardObject.GetComponent<Transform>().position = newPosition;
        // TODO: animate movement from current position to new position
    }

    public void MoveToFaceDown(Vector2 newPosition)
    {
        cardObject.SetActive(true);
        cardObject.GetComponent<Transform>().position = newPosition;
        TurnFaceDown();
        // TODO: animate movement from current position to new position
    }

    public void MoveToFaceUp(Vector2 newPosition)
    {
        cardObject.SetActive(true);
        cardObject.GetComponent<Transform>().position = newPosition;
        TurnFaceUp();
        // TODO: animate movement from current position to new position
    }

    public void RegisterTo(CardZone newLocation)
    {
        if (newLocation == currentLocation) return;

        CardZone previousLocation = currentLocation;
        currentLocation = newLocation;
        previousLocation.Unregister(this);
    }

    public void ResetProperties()
    {
        convertedSuit = null;
        convertedValue = null;
    }

    public string ToStringVerbose()
    {
        return name + "_" + Suit;
    }

    public override string ToString()
    {
        string displayName;
        if (convertedValue == null && name == value.ToString())
        {
            displayName = name + " of ";
        }
        else
        {
            displayName = name + " [" + Value + "] of ";
        }

        if (convertedSuit == null)
        {
            return displayName + suit + "s :: " + currentLocation;
        }
        else
        {
            return displayName + suit + "s [" + Suit + "s] :: " + currentLocation;
        }
    }

    public void TurnFaceDown()
    {
        cardObject.GetComponent<SpriteRenderer>().sprite = back;
    }

    public void TurnFaceUp()
    {
        cardObject.GetComponent<SpriteRenderer>().sprite = face;
        // TODO: if card has been converted, use convertedFace instead of face
        // NB the above will likely never apply here so is not a priority
    }
}
