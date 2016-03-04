using System;
using System.Collections.Generic;

namespace Pyramid
{
    public class Deck
    {
        private const Int32 MaxFirstLevelCardCount = 3;
        private const Int32 MaxSecondLevelCardCount = 6;
        private const Int32 MaxThirdLevelCardCount = 9;
        public Card Card;
        public List<Card> DeckOfCards;
        // Use this for initialization
        public Deck()
        {
            CreatePyramidDeck();

            ShuffleCards();
        }

        public Card DrawCard()
        {
            Card card = DeckOfCards[0];
            DeckOfCards.Remove(card);
            return card;
        }

        private void CreatePyramidDeck()
        {
            int addCardCount = 0;
            float cardXPos = -1.0f;
            float cardYPos = -1.0f;
            DeckOfCards = new List<Card>();

            // Add 1 Capstone card
            Card capstone = new Card() { IsCapstone = true, IsFaceUp = false };
            cardXPos += 0.3f;
            DeckOfCards.Add(capstone);

            // Loop through Suits 
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Level level in Enum.GetValues(typeof(Level)))
                {
                    switch (level)
                    {
                        case Level.First:
                            addCardCount = MaxFirstLevelCardCount;
                            break;
                        case Level.Second:
                            addCardCount = MaxSecondLevelCardCount;
                            break;
                        case Level.Third:
                            addCardCount = MaxThirdLevelCardCount;
                            break;
                    }
                    for (int i = 0; i < addCardCount; i++)
                    {
                        Card card = new Card() { IsCapstone = false, IsFaceUp = false, Suit = suit, Level = level };
                        cardXPos += 0.3f;
                        if (cardXPos > 2.0f)
                        {
                            cardXPos = -0.3f;
                            cardYPos += 0.3f;
                        }
                        DeckOfCards.Add(card);
                    }
                }
            }
        }

        private void ShuffleCards()
        {
            int sizeOfDeck = DeckOfCards.Count;
            List<Card> shuffledDeckOfCards = new List<Card>();
            for (int i = 0; i < 55; i++)
            {
                Random random = new Random();
                Card cardGameObject = DeckOfCards[random.Next(0, sizeOfDeck)];
                if (shuffledDeckOfCards.Count > 1)
                {
                    shuffledDeckOfCards.Insert(random.Next(0, shuffledDeckOfCards.Count - 1), cardGameObject);
                }
                else
                {
                    shuffledDeckOfCards.Insert(0, cardGameObject);
                }
                DeckOfCards.Remove(cardGameObject);
                sizeOfDeck--;
            }
            DeckOfCards = shuffledDeckOfCards;
        }
    }
}
