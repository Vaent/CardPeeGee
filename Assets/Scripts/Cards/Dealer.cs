using Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Dealer
    {
        private static Deck deck;

        public void DealCards(int count)
        {
            DealCards(GameState.GetStagingArea, count);
        }

        public void DealCards(CardZone target, int count)
        {
            Debug.Log("Deal " + count + " cards to " + target);
            SoundEffects.Play(SoundEffects.DealCard);
            target.Accept(DrawCards(count));
        }

        /**
         * <summary>
         * Deals <paramref name="count"/> cards to the lower half of the staging area.
         * Useful when a distinct second set of cards needs to be dealt before the first set is returned.
         * </summary>
         */
        public void DealCardsAlternate(int count)
        {
            GameState.GetStagingArea.SetAlternate();
            DealCards(GameState.GetStagingArea, count);
        }

        // this method will select `count` cards randomly from the deck and return them to the caller
        // the cards remain in the deck until registered to another CardZone
        public List<Card> DrawCards(int count)
        {
            // TODO: handle an attempt to draw more cards than are currently in the Deck
            return deck.DrawCards(count);
        }

        public static void Register(Deck newDeck)
        {
            if (deck == null)
            {
                deck = newDeck;
            }
            else
            {
                Debug.LogError("Attempted to register a new Deck when one already exists");
            }
        }

        internal void ReturnToDeck(Card card)
        {
            deck.Accept(card);
        }

        internal void ReturnToDeck(List<Card> cards)
        {
            deck.Accept(cards);
        }
    }
}
