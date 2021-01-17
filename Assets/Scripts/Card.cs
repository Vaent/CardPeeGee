using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class which acts as the virtual representation of a card,
housing its individual characteristics, game logic relating to cards etc.
The CardController class complements this class by handling the
"physical" GameObject which is displayed. */
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
    private readonly CardController cardController;
    private readonly GameObject cardObject;
    private readonly SpriteRenderer cardRenderer;
    private readonly Sprite face;
    private readonly string name;
    private readonly Suit suit;
    private readonly int value;

    // accessors
    public CardZone CurrentLocation => currentLocation;
    public string Name => name;
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
        cardController = cardObject.GetComponent<CardController>();
        cardController.RegisterController(this);
        cardRenderer = cardObject.GetComponent<SpriteRenderer>();

        this.currentLocation = startingLocation;
    }

    public void Convert(int newValue)
    {
        convertedValue = newValue;
        // TODO: calculate and display the convertedFace sprite
    }

    // converting the card's suit also decreases its effective value by 2
    public void Convert(Suit newSuit)
    {
        convertedSuit = newSuit;
        Convert(value - ConversionPenalty);
    }

    public void DoClicked()
    {
        currentLocation.NotifySelectionByUser(this);
    }

    public void Flip()
    {
        cardRenderer.sprite = (cardRenderer.sprite == face ? back : face);
    }

    public void Hide()
    {
        cardObject.SetActive(false);
        cardController.KillMovement();
    }

    public void MoveTo(Vector2 newPosition)
    {
        cardObject.SetActive(true);
        cardController.GoTo(newPosition, false);
    }

    public void MoveToFaceDown(Vector2 newPosition)
    {
        MoveToFaceDown(newPosition, null);
    }

    public void MoveToFaceDown(Vector2 newPosition, CardController.MovementTracker tracker)
    {
        cardObject.SetActive(true);
        cardController.GoTo(newPosition, (cardRenderer.sprite == face), tracker);
    }

    public void MoveToFaceUp(Vector2 newPosition)
    {
        MoveToFaceUp(newPosition, null);
    }

    public void MoveToFaceUp(Vector2 newPosition, CardController.MovementTracker tracker)
    {
        cardObject.SetActive(true);
        cardController.GoTo(newPosition, (cardRenderer.sprite != face), tracker);
    }

    public void RegisterTo(CardZone newLocation)
    {
        if (newLocation == currentLocation) return;

        CardZone previousLocation = currentLocation;
        currentLocation = newLocation;
        previousLocation.Unregister(this);
    }

    public void ResetCardProperties()
    {
        convertedSuit = null;
        convertedValue = null;
    }

    public void ResetDisplayProperties()
    {
        RaiseTo(0);
        Resize(1);
    }

    /* Call this function to make the card proportionally larger or smaller
    than the default size (not the card's current size) */
    public void Resize(float newScale)
    {
        cardController.Resize(newScale);
    }

    public void RaiseTo(float newHeight)
    {
        // N.B. camera is positioned on the negative z-axis, pointed toward zero
        cardController.SetHeight(newHeight * -1);
    }

    public override string ToString()
    {
        return name + "_" + Suit;
    }

    public string ToStringVerbose()
    {
        string displayName;
        int nameParsed;
        if (convertedValue == null && int.TryParse(name, out nameParsed) && nameParsed == value)
        {
            displayName = value + " of ";
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
        cardRenderer.sprite = back;
    }

    public void TurnFaceUp()
    {
        cardRenderer.sprite = face;
        // TODO: if card has been converted, use convertedFace instead of face
        // NB the above will likely never apply here so is not a priority
    }
}
