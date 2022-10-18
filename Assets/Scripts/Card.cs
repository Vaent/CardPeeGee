using static Constant;
using System;
using UnityEngine;

/* Class which acts as the virtual representation of a card,
housing its individual characteristics, game logic relating to cards etc.
The CardController class complements this class by handling the
"physical" GameObject which is displayed. */
public class Card
{
    public static readonly int conversionPenalty = 2;
    private static readonly GameObject abstractCard = Resources.Load<GameObject>("Models/Abstract Card");

    // state variables
    private CardZone currentLocation;
    // convertedXxx will apply when e.g. a Jack is used to temporarily change the suit of a played card
    private Suit? convertedSuit;
    private int? convertedValue;
    // initial assigned values cannot be changed
    private readonly CardController cardController;
    private Sprite face;
    private readonly string name;
    private readonly Suit suit;
    private readonly int value;

    // accessors
    public CardZone CurrentLocation => currentLocation;
    public string Name => name;
    public Suit NaturalSuit => suit;
    public int NaturalValue => value;
#if UNITY_EDITOR
    public int StackedCardOrder => cardController.stackedCardOrder;
#endif

    // the following use converted values if set, otherwise base values
    public Suit Suit => convertedSuit ?? suit;
    public int Value => convertedValue ?? value;

    public Card(Sprite face, CardZone startingLocation)
    {
        this.face = face;
        string[] filenameParts = face.name.Split(' ');
        this.suit = (Suit)Suit.Parse(typeof(Suit), filenameParts[0]);
        this.value = int.Parse(filenameParts[1]);
        this.name = (filenameParts.Length > 2) ? filenameParts[2] : filenameParts[1];
        GameObject cardObject = MonoBehaviour.Instantiate(abstractCard);
        cardObject.name = this.ToString();
        cardController = cardObject.GetComponent<CardController>();
        cardController.Register(this, face);

        this.currentLocation = startingLocation;
    }

    public void ApplyColor(Color color)
    {
        cardController.ApplyColor(color);
    }

    public bool CompareTag(string tag)
    {
        return cardController.CompareTag(tag);
    }

    public void Convert(int newValue)
    {
        if (newValue < 0 || newValue > 11) throw new ArgumentOutOfRangeException($"A card cannot have a value of {value}.");

        convertedValue = newValue;
        Sprite convertedFace = CardUtil.GetCardFace(this.Suit, this.Value);
        cardController.ChangeFace(convertedFace);
    }

    public void Convert(Suit newSuit)
    {
        try
        {
            convertedSuit = newSuit;
            // converting the card's suit also decreases its effective value
            Convert(value - conversionPenalty);
        }
        catch (ArgumentException ex)
        {
            Debug.LogError($"Failed to convert card; reverting to natural state.\n{ex}");
            ResetCardProperties();
            throw new InvalidOperationException($"Unable to convert {this} to {newSuit}.\n{ex}");
        }
    }

    public void Hide()
    {
        cardController.Kill();
    }

    public bool IsConvertedSuit()
    {
        return convertedSuit != null;
    }

    public void MoveTo(Vector3 newPosition, CardController.MovementTracker tracker = null, CardController.Orientation? targetOrientation = null)
    {
        SetHeight(newPosition.z);
        cardController.GoTo(newPosition, tracker, targetOrientation);
    }

    public Vector3 Position()
    {
        return cardController.transform.position;
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
        cardController.ChangeFace(face);
    }

    public void ResetDisplayProperties()
    {
        SetHeight(0);
        Resize(1);
        ApplyColor(Color.white);
    }

    /* Call this function to make the card proportionally larger or smaller
    than the default size (not the card's current size) */
    public void Resize(float newScale)
    {
        cardController.Resize(newScale);
    }

    public void SetHeight(float newHeight)
    {
        cardController.SetHeight(newHeight);
    }

    public override string ToString()
    {
        return name + " of " + suit + "s";
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

#if UNITY_EDITOR
    public void Unstack()
    {
        if (cardController.CompareTag(STACK_DECK))
        {
            cardController.tag = UNTAGGED;
            cardController.stackedCardOrder = 0;
        }
    }
#endif
}
