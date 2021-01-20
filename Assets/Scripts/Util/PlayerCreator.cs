using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton which manages the process of building a new Player */
public class PlayerCreator
{
    private static PlayerCreator instance;

    private Phase currentPhase;
    private Deck deck;
    private TextMesh hpDisplay;
    private TextMesh messageDisplay;
    private bool movedCharacterCard;
    private bool movedRejectedCandidates;
    private Player player;
    private StagingArea stagingArea;

    private PlayerCreator(Deck deck, StagingArea stagingArea, TextMesh messageDisplay, TextMesh hpDisplay) {
        this.currentPhase = Phase.GetCharacterCard;
        this.deck = deck;
        this.stagingArea = stagingArea;
        this.messageDisplay = messageDisplay;
        this.hpDisplay = hpDisplay;
    }

// class methods have public visibility

    public static void Close()
    {
        instance = null;
    }

    public static void Initialise(Deck deck, StagingArea stagingArea, TextMesh messageDisplay, TextMesh hpDisplay)
    {
        if (instance != null) Debug.LogWarning("Discarding existing (incomplete) progress");
        instance = new PlayerCreator(deck, stagingArea, messageDisplay, hpDisplay);
        instance.GetCharacterCard();
    }

    public static void NotifyCardsReceived(CardZone cardZone, List<Card> cards)
    {
        if (instance != null)
        {
            instance.NewCards(cardZone, cards);
        }
    }

// callbacks on the instance are public

public void CharacterCardCallback(Card card)
{
    player = new Player(card, hpDisplay);
    currentPhase = Phase.GetHP;
    if (stagingArea.Cards.Count > 0)
    {
        deck.Accept(stagingArea.Cards);
    }
    else
    {
        movedRejectedCandidates = true;
    }
}

public void HPCallback()
{
    currentPhase = Phase.GetHand;
    deck.Accept(stagingArea.Cards);
}

// normal instance methods are only visible to the class/instance

    private void CheckCharacterCard(Card candidate)
    {
        if (candidate.Name.Equals("Queen") || candidate.Name.Equals("King"))
        {
            messageDisplay.text = "\n\n\n\n\nYou are the " + candidate.Name + " of " + candidate.Suit + "s";
            Timer.DelayThenInvoke(2, this.CharacterCardCallback, candidate);
        }
        else
        {
            deck.DealCards(1);
        }
    }

    private void GetCharacterCard()
    {
        messageDisplay.text = "\n\n\n\n\nFinding a Character card...";
        deck.DealCards(1);
    }

    private void GetHP(List<Card> cards)
    {
        int hp = 15 + CardUtil.SumValues(cards);
        messageDisplay.text = "\n\n\n\n\nYou have " + hp + " HP (15 + ";
        for (var i = 0; i < cards.Count; )
        {
            messageDisplay.text += cards[i].Value;
            if (++i < cards.Count) messageDisplay.text += " + ";
        }
        messageDisplay.text += ")";
        player.Heal(hp);
        Timer.DelayThenInvoke(2, this.HPCallback);
    }

    private void NewCards(CardZone cardZone, List<Card> cards)
    {
        switch (currentPhase)
        {
            case Phase.GetCharacterCard:
                if (cardZone.Equals(stagingArea))
                {
                    CheckCharacterCard(cards[0]);
                }
                break;
            case Phase.GetHP:
                NotifyGetHP(cardZone, cards);
                break;
            case Phase.GetHand:
                NotifyGetHand(cardZone, cards);
                break;
            default:
                throw new System.Exception("PlayerCreator.currentPhase not recognised");
        }
    }

    private void NotifyGetHand(CardZone cardZone, List<Card> cards)
    {
        if (cardZone.Equals(deck))
        {
            messageDisplay.text = "\n\n\n\n\nDealing your starting hand...";
            deck.DealCards(5);
        }
        else if (cardZone.Equals(stagingArea))
        {
            player.AddToHand(cards);
        }
        else if (cardZone.Equals(player.Hand))
        {
            GameState.Register(player);
        }
    }

    private void NotifyGetHP(CardZone cardZone, List<Card> cards)
    {
        if (cardZone.Equals(deck))
        {
            movedRejectedCandidates = true;
        }
        else if (cardZone.Equals(player.CharacterCard))
        {
            movedCharacterCard = true;
        }

        if (movedCharacterCard && movedRejectedCandidates)
        {
            if (cardZone.Equals(stagingArea))
            {
                GetHP(cards);
            }
            else
            {
                messageDisplay.text = "\n\n\n\n\nDealing cards for initial HP...";
                deck.DealCards(3);
            }
        }
    }

// PlayerCreator.Phase is managed internally through the currentPhase field

    private enum Phase
    {
        GetCharacterCard,
        GetHP,
        GetHand
    }
}
