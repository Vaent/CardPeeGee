using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardZone
{
    // component references
    public StagingArea stagingArea;

    // state variables
    private bool isLoading;

    void Start()
    {
        this.isLoading = true;
        GameState.Register(this);
        CardUtil.NewPack(this);
        this.isLoading = false;
        Debug.Log("Deck contains the following after loading: " + Cards.Print());
    }

    void OnMouseDown()
    {
        GameState.Next();
    }

    public void DealCards(int count)
    {
        DealCards(stagingArea, count);
    }

    public void DealCards(CardZone target, int count)
    {
        Debug.Log("Deal " + count + " cards to " + target);
        target.Accept(DrawCards(count));
    }

    // this method will select `count` cards randomly from the deck and return them to the caller
    // the cards remain in the deck until registered to another CardZone
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = new List<Card>();
        List<Card> cardsInDeck = Cards;
        var rand = new System.Random();
        for (var i = 0; i < count; i++)
        {
            var randomIndex = rand.Next(cardsInDeck.Count);
            drawnCards.Add(cardsInDeck[randomIndex]);
            cardsInDeck.RemoveAt(randomIndex);
        }
        return drawnCards;
    }

    public override void NotifySelectionByUser(Card selectedCard) { }

    protected override void ProcessNewCards(List<Card> newCards)
    {
        if (this.isLoading) return;

        Debug.Log("Cards were returned to the Deck: " + newCards.Print());
        Debug.Log("Deck now contains the following: " + Cards.Print());
        newCards.ForEach(card =>
        {
            CardController.MovementTracker tracker = cardsInMotion[card];
            card.ResetCardProperties();
            card.ResetDisplayProperties();
            card.MoveTo(this.transform.position + Vector3.forward, tracker, false);
        });
    }
}
