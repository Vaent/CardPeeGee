using Audio;
using System;
using System.Collections.Generic;

namespace Cards
{
    public class Dealer
    {
//        public void DealCards(int count)
//        {
//            DealCards(stagingArea, count);
//        }

//        public void DealCards(CardZone target, int count)
//        {
//            Debug.Log("Deal " + count + " cards to " + target);
//            SoundEffects.Play(SoundEffects.DealCard);
//            target.Accept(DrawCards(count));
//        }

//        /**
//         * <summary>
//         * Deals <paramref name="count"/> cards to the lower half of the staging area.
//         * Useful when a distinct second set of cards needs to be dealt before the first set is returned.
//         * </summary>
//         */
//        public void DealCardsAlternate(int count)
//        {
//            stagingArea.SetAlternate();
//            DealCards(stagingArea, count);
//        }

//        // this method will select `count` cards randomly from the deck and return them to the caller
//        // the cards remain in the deck until registered to another CardZone
//        public List<Card> DrawCards(int count)
//        {
//            // TODO: handle an attempt to draw more cards than are currently in the Deck
//#if UNITY_EDITOR
//            #region Deck stacking extension
//            List<Card> stackingCards = Cards.FindAll(card => card.CompareTag(STACK_DECK));
//            if (stackingCards.Count > 0)
//            {
//                stackingCards.Sort((a, b) => a.StackedCardOrder - b.StackedCardOrder);
//                var cardsToUse = stackingCards.GetRange(0, Min(count, stackingCards.Count));
//                cardsToUse.ForEach(card => card.Unstack());
//                if (cardsToUse.Count < count)
//                {
//                    var nonStackingCards = Cards;
//                    nonStackingCards.RemoveAll(card => stackingCards.Contains(card));
//                    var shortfall = count - cardsToUse.Count - nonStackingCards.Count;
//                    if (shortfall <= 0)
//                    {
//                        cardsToUse.AddRange(nonStackingCards.SelectRandom(count - cardsToUse.Count));
//                    }
//                    else
//                    {
//                        cardsToUse.AddRange(nonStackingCards);
//                        Debug.LogWarning($"Too few 'non-stacking' cards in the deck; {shortfall} of the lowest priority 'stacking' cards will also be dealt");
//                        var lastStackingCards = stackingCards.GetRange(stackingCards.Count - shortfall, shortfall);
//                        lastStackingCards.ForEach(card => card.Unstack());
//                        cardsToUse.AddRange(lastStackingCards);
//                    }
//                }
//                return cardsToUse;
//            }
//            #endregion
//#endif
//            return Cards.SelectRandom(count);
//        }
    }
}
