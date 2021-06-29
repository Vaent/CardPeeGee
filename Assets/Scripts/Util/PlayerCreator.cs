using System.Collections.Generic;
using static Text.PlayerCreator.TextReference;
using UnityEngine;

/* Singleton which manages the process of building a new Player */
public class PlayerCreator
{
    private static PlayerCreator instance;

    private Phase currentPhase;
    private Deck deck;
    private TextMesh hpDisplay;
    private bool movedCharacterCard;
    private bool movedRejectedCandidates;
    private Player player;
    private StagingArea stagingArea;

    private PlayerCreator(Deck deck, StagingArea stagingArea)
    {
        hpDisplay ??= GameObject.Find("hptext").GetComponent<TextMesh>();
        currentPhase = Phase.GetCharacterCard;
        this.deck = deck;
        this.stagingArea = stagingArea;
    }

// class methods have public visibility

    public static void Close()
    {
        instance = null;
    }

    public static void Initialise(Deck deck, StagingArea stagingArea)
    {
        if (instance != null) Debug.LogWarning("Discarding existing (incomplete) progress");
        instance = new PlayerCreator(deck, stagingArea);
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
            Text.PlayerCreator.Display((int)CharacterIdentified, candidate.ToStringForDisplay());
            Timer.DelayThenInvoke(2, CharacterCardCallback, candidate);
        }
        else
        {
            deck.DealCards(1);
        }
    }

    private void GetCharacterCard()
    {
        Text.PlayerCreator.Display((int)CharacterSearch);
        deck.DealCards(1);
    }

    private void GetHP(List<Card> cards)
    {
        int hp = 15 + CardUtil.SumValues(cards);
        List<int> cardValues = cards.ConvertAll(card => card.Value);
        Text.PlayerCreator.Display((int)HPCalculated, hp, string.Join(" + ", cardValues));
        player.Heal(hp);
        Timer.DelayThenInvoke(2, HPCallback);
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
            Text.PlayerCreator.Hide((int)HPSearch);
            Text.PlayerCreator.Hide((int)HPCalculated);
            Text.PlayerCreator.Display((int)DealHand);
            deck.DealCards(5);
        }
        else if (cardZone.Equals(stagingArea))
        {
            player.AddToHand(cards);
        }
        else if (cardZone.Equals(player.Hand))
        {
            Text.PlayerCreator.TearDown();
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
                Text.PlayerCreator.Hide((int)CharacterSearch);
                Text.PlayerCreator.Hide((int)CharacterIdentified);
                Text.PlayerCreator.Display((int)HPSearch);
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
