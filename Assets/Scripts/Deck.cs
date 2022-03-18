using static Constant;
using ExtensionMethods;
using System.Collections.Generic;
using static System.Math;
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
        // TODO: handle an attempt to draw more cards than are currently in the Deck
#if UNITY_EDITOR
        #region Deck stacking extension
        List<Card> stackingCards = Cards.FindAll(card => card.CompareTag(STACK_DECK));
        if (stackingCards.Count > 0)
        {
            stackingCards.Sort((a, b) => a.StackedCardOrder - b.StackedCardOrder);
            var cardsToUse = stackingCards.GetRange(0, Min(count, stackingCards.Count));
            cardsToUse.ForEach(card => card.Unstack());
            if (cardsToUse.Count < count)
            {
                var nonStackingCards = Cards;
                nonStackingCards.RemoveAll(card => stackingCards.Contains(card));
                var shortfall = count - cardsToUse.Count - nonStackingCards.Count;
                if (shortfall <= 0)
                {
                    cardsToUse.AddRange(nonStackingCards.SelectRandom(count - cardsToUse.Count));
                }
                else
                {
                    cardsToUse.AddRange(nonStackingCards);
                    Debug.LogWarning($"Too few 'non-stacking' cards in the deck; {shortfall} of the lowest priority 'stacking' cards will also be dealt");
                    var lastStackingCards = stackingCards.GetRange(stackingCards.Count - shortfall, shortfall);
                    lastStackingCards.ForEach(card => card.Unstack());
                    cardsToUse.AddRange(lastStackingCards);
                }
            }
            return cardsToUse;
        }
        #endregion
#endif
        return Cards.SelectRandom(count);
    }

    protected override void MovedCards(List<Card> movingCards)
    {
        movingCards.ForEach(card =>
        {
            if (Cards.Contains(card)) card.Hide();
        });
    }

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
            card.MoveTo(this.transform.position + Vector3.forward, tracker, CardController.Orientation.FaceDown);
#if UNITY_EDITOR
            if (card.CompareTag(STACK_DECK)) card.Unstack();
#endif
        });
    }
}
