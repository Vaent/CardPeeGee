using System.Collections.Generic;
using static Text.Excerpts.PlayerCreator;
using static Text.TextManager;
using UnityEngine;

/* Singleton which manages the process of building a new Player */
public class PlayerCreator : IGamePhase
{
    private static readonly int initialHandNumberOfCards = 5;
    private static readonly int initialHpBaseValue = 15;
    private static readonly int initialHpNumberOfCards = 3;
    private static PlayerCreator instance;

    private Text.BaseExcerpt characterIdentifiedExcerpt;
    private Phase currentPhase;
    private Text.BaseExcerpt hpCalculatedExcerpt;
    private TextMesh hpDisplay;
    private bool isInProgress = false;
    private bool movedCharacterCard;
    private bool movedRejectedCandidates;
    private Player player;

    private PlayerCreator()
    {
        hpDisplay ??= GameObject.Find("hptext").GetComponent<TextMesh>();
        currentPhase = Phase.GetCharacterCard;
    }

// class methods have public visibility

    public static IGamePhase GetClean()
    {
        instance = new PlayerCreator();
        return instance;
    }

// callbacks on the instance are public

    public void BeginCreating()
    {
        GetCharacterCard();
    }

    public void CharacterCardCallback(Card card)
    {
        player = new Player(card, hpDisplay);
        currentPhase = Phase.GetHP;
        if (GameState.GetStagingArea.Cards.Count > 0)
        {
            GameState.GetDeck.Accept(GameState.GetStagingArea.Cards);
        }
        else
        {
            movedRejectedCandidates = true;
        }
    }

    public void HPCallback()
    {
        currentPhase = Phase.GetHand;
        GameState.GetDeck.Accept(GameState.GetStagingArea.Cards);
    }

// normal instance methods are only visible to the class/instance

    private void CheckCharacterCard(Card candidate)
    {
        if (candidate.Name.Equals("Queen") || candidate.Name.Equals("King"))
        {
            characterIdentifiedExcerpt = CharacterIdentified(candidate);
            DisplayTextAsExtension(characterIdentifiedExcerpt, CharacterSearch);
            Timer.DelayThenInvoke(2, CharacterCardCallback, candidate);
        }
        else
        {
            GameState.GetDeck.DealCards(1);
        }
    }

    private void GetCharacterCard()
    {
        DisplayText(CharacterSearch);
        GameState.GetDeck.DealCards(1);
    }

    private void GetHP(List<Card> cards)
    {
        int hp = initialHpBaseValue + CardUtil.SumValues(cards);
        List<int> cardValues = cards.ConvertAll(card => card.Value);
        hpCalculatedExcerpt = HPCalculated(hp, initialHpBaseValue, cardValues);
        DisplayTextAsExtension(hpCalculatedExcerpt, HPSearch);
        player.Heal(hp);
        Timer.DelayThenInvoke(2, HPCallback);
    }

    private void NotifyGetHand(CardZone cardZone, List<Card> cards)
    {
        if (cardZone.Equals(GameState.GetDeck))
        {
            HideText(HPSearch);
            HideText(hpCalculatedExcerpt);
            DisplayText(DealHand);
            GameState.GetDeck.DealCards(initialHandNumberOfCards);
        }
        else if (cardZone.Equals(GameState.GetStagingArea))
        {
            player.AddToHand(cards);
        }
        else if (cardZone.Equals(player.Hand))
        {
            TearDownDisplayedText();
            GameState.Register(player);
        }
    }

    private void NotifyGetHP(CardZone cardZone, List<Card> cards)
    {
        if (cardZone.Equals(GameState.GetDeck))
        {
            movedRejectedCandidates = true;
        }
        else if (cardZone.Equals(player.CharacterCard))
        {
            movedCharacterCard = true;
        }

        if (movedCharacterCard && movedRejectedCandidates)
        {
            if (cardZone.Equals(GameState.GetStagingArea))
            {
                GetHP(cards);
            }
            else
            {
                HideText(CharacterSearch);
                HideText(characterIdentifiedExcerpt);
                DisplayText(HPSearch);
                GameState.GetDeck.DealCards(initialHpNumberOfCards);
            }
        }
    }

    public void RegisterCardsReceived(CardZone destination, List<Card> cards)
    {
        switch (currentPhase)
        {
            case Phase.GetCharacterCard:
                if (destination.Equals(GameState.GetStagingArea))
                {
                    CheckCharacterCard(cards[0]);
                }
                break;
            case Phase.GetHP:
                NotifyGetHP(destination, cards);
                break;
            case Phase.GetHand:
                NotifyGetHand(destination, cards);
                break;
            default:
                throw new System.Exception("PlayerCreator.currentPhase not recognised");
        }
    }

    // cards are not discarded during player creation
    public void RegisterDiscardAction(Card card) { }

    // cards are not selectable during player creation
    public void RegisterInteractionWith(Card card) { }

    public void RegisterInteractionWithDeck()
    {
        if (!isInProgress)
        {
            isInProgress = true;
            if (GameState.GetPlayer?.IsAlive() == false)
            {
                GameState.Restart();
            }
            else
            {
                UnityEngine.Object.Destroy(GameObject.Find("eventtext3"));
                BeginCreating();
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
