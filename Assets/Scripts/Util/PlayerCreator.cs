using Cards;
using System.Collections.Generic;
using static Text.Excerpts.PlayerCreator;
using static Text.TextManager;
using UnityEngine;

public class PlayerCreator : IGamePhase
{
    private static readonly int initialHandNumberOfCards = 5;
    private static readonly int initialHpBaseValue = 15;
    private static readonly int initialHpNumberOfCards = 3;

    private Text.BaseExcerpt characterIdentifiedExcerpt;
    private Text.BaseExcerpt hpCalculatedExcerpt;
    private TextMesh hpDisplay;
    private bool isInProgress = false;
    private bool movedCharacterCard;
    private bool movedRejectedCandidates;
    private Player player;

    private PlayerCreator()
    {
        hpDisplay = GameObject.Find("hptext").GetComponent<TextMesh>();
    }

// class methods

    public static IGamePhase GetClean()
    {
        return new PlayerCreator();
    }

// public instance methods

    public void BeginCreating()
    {
        DisplayText(CharacterSearch);
        GameState.GetDeck.DealCards(1);
    }

    public void CharacterCardCallback(Card card)
    {
        player = new Player(card, hpDisplay);
        if (GameState.GetStagingArea.Cards.Count > 0)
        {
            GameState.GetDeck.Accept(GameState.GetStagingArea.Cards);
        }
        else
        {
            movedRejectedCandidates = true;
        }
    }

    public void HPCallback() => GameState.GetDeck.Accept(GameState.GetStagingArea.Cards);

// private instance methods

    private void CalculateAndApplyInitialHP(List<Card> cards)
    {
        int hp = initialHpBaseValue + CardUtil.SumValues(cards);
        List<int> cardValues = cards.ConvertAll(card => card.Value);
        hpCalculatedExcerpt = HPCalculated(hp, initialHpBaseValue, cardValues);
        DisplayTextAsExtension(hpCalculatedExcerpt, HPSearch);
        player.Heal(hp);
        Timer.DelayThenInvoke(2, HPCallback);
    }

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
                CalculateAndApplyInitialHP(cards);
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
        if (player == null)
        {
            if (destination.Equals(GameState.GetStagingArea))
            {
                CheckCharacterCard(cards[0]);
            }
        }
        else if (!player.IsAlive())
        {
            NotifyGetHP(destination, cards);
        }
        else
        {
            NotifyGetHand(destination, cards);
        }
    }

    // cards are not discarded during player creation
    public void RegisterDiscardAction(Card card) { }

    // cards are not interactable during player creation
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
}
