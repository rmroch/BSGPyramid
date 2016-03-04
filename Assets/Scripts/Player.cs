using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Pyramid
{
    public class Player
    {
        private const Int32 MaxCardCount = 6;
        private const Int32 MaxSelectedCardCount = 3;
        private Int32 _cubits;
        public Int32 CurrentBet { get; set; }
        public Int32 Cubits
        {
            get
            {
                return _cubits;
            }
        }
        public string Name { get; set; }
        public bool IsBot { get; set; }
        public bool IsPlaying { get; set; }

        public string ShowHand
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var card in Hand)
                {
                    sb.AppendLine(card.CardDescription);
                }
                return sb.ToString();
            }
        }
        public string ShowFormattedHand
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int i = 1;
                foreach (var card in Hand)
                {
                    sb.AppendLine(string.Format("{0}) {1} {2}"
                        , i++
                        , card.CardDescription
                        , card.IsSelected ? "(*)" : ""));
                }
                return sb.ToString();
            }
        }
        public string Results {
            get
            {
                if (HasFullPyramid())
                    return "a Full Pyramid!!!!!";
                switch (HasPerfectThirdLevelPyramid())
                {
                    case Suit.Purple:
                        return "a Perfect Purple Thrid Level Pyramid!!!";
                    case Suit.Green:
                        return "a Perfect Green Thrid Level Pyramid!!!";
                    case Suit.Orange:
                        return "a Perfect Orange Thrid Level Pyramid!!!";
                }

                if (HasThirdLevelPyramid())
                    return "a Third Level Pyramid!!";

                switch (HasPerfectSecondLevelPyramid())
                {
                    case Suit.Purple:
                        return "a Perfect Purple Second Level Pyramid!!";
                    case Suit.Green:
                        return "a Perfect Green Second Level Pyramid!!";
                    case Suit.Orange:
                        return "a Perfect Orange Second Level Pyramid!!";
                }

                if (HasSecondLevelPyramid())
                    return "a Second Level Pyramid!";

                switch (HasPerfectFirstLevelPyramid())
                {
                    case Suit.Purple:
                        return "a Perfect Purple First Level Pyramid!";
                    case Suit.Green:
                        return "a Perfect Green First Level Pyramid!";
                    case Suit.Orange:
                        return "a Perfect Orange First Level Pyramid!";
                }

                if (HasFirstLevelPyramid())
                {
                    return "a First Level Pyramid.";
                }

                return "nothing";
            }
        }
        private IList<Card> _hand;
        public IList<Card> Hand
        {
            get { return _hand; }
        }

        public Player()
        {
            _hand = new List<Card>();
            _cubits = 100;
        }

        public void AddCard(Card card)
        {
            if (_hand.Count >= MaxCardCount)
                return;
            _hand.Add(card);
        }

        public void SelectCardForDiscard(Card card)
        {
            if (card.IsSelected)
            {
                Debug.Log("Deselect Card");
                card.IsSelected = false;
                return;
            }
            
            if (SelectedCardCount() >= MaxSelectedCardCount)
                return;

            Debug.Log("Select Card");
            card.IsSelected = true;
        }

        public void DiscardSelectedCards()
        {
            for (int i = MaxCardCount - 1; i >= 0; i--)
            {
                if (Hand[i].IsSelected)
                {
                    _hand.RemoveAt(i);
                }
            }
        }

        public void DiscardAllCards()
        {
            _hand.Clear();
        }

        public bool HasFirstLevelPyramid()
        {
            bool has2ThirdLevel = false;
            bool has1SecondLevel = false;

            if (LevelCardCount(Level.Third) >= 2)
                has2ThirdLevel = true;

            if (LevelCardCount(Level.Second) >= 1)
                has1SecondLevel = true;

            if (has2ThirdLevel && has1SecondLevel)
                return true;

            if (has2ThirdLevel && has1SecondLevel == false && HasCapstone())
                return true;

            if (has1SecondLevel && LevelCardCount(Level.Third) == 1 && HasCapstone())
                return true;

            return false;
        }

        public bool HasSecondLevelPyramid()
        {
            bool has3ThirdLevel = false;
            bool has2SecondLevel = false;

            if (LevelCardCount(Level.Third) >= 3)
                has3ThirdLevel = true;

            if (LevelCardCount(Level.Second) >= 2)
                has2SecondLevel = true;

            if (has3ThirdLevel && has2SecondLevel)
                return true;

            if (has3ThirdLevel && LevelCardCount(Level.Second) == 1 && HasCapstone())
                return true;

            if (has2SecondLevel && LevelCardCount(Level.Third) == 2 && HasCapstone())
                return true;

            return false;
        }

        public bool HasThirdLevelPyramid()
        {
            bool has3ThirdLevel = false;
            bool has2SecondLevel = false;
            bool has1FirstLevel = false;

            if (LevelCardCount(Level.Third) >= 3)
                has3ThirdLevel = true;

            if (LevelCardCount(Level.Second) >= 2)
                has2SecondLevel = true;

            if (LevelCardCount(Level.First) >= 1)
                has1FirstLevel = true;

            if (has3ThirdLevel && has2SecondLevel && has1FirstLevel)
                return true;

            if (has3ThirdLevel && has2SecondLevel && HasCapstone())
                return true;

            if (has1FirstLevel && has3ThirdLevel && LevelCardCount(Level.Second) == 1 && HasCapstone())
                return true;

            if (has1FirstLevel && has2SecondLevel && LevelCardCount(Level.Third) == 2 && HasCapstone())
                return true;

            return false;
        }

        public Suit? HasPerfectFirstLevelPyramid()
        {
            if (HasPerfFirLvlPyr(Suit.Orange))
                return Suit.Orange;

            if (HasPerfFirLvlPyr(Suit.Green))
                return Suit.Green;

            if (HasPerfFirLvlPyr(Suit.Purple))
                return Suit.Purple;

            return null;
        }

        public Suit? HasPerfectSecondLevelPyramid()
        {
            if (HasPerfSndLvlPry(Suit.Orange))
                return Suit.Orange;

            if (HasPerfSndLvlPry(Suit.Green))
                return Suit.Green;

            if (HasPerfSndLvlPry(Suit.Purple))
                return Suit.Purple;

            return null;
        }

        public bool HasPerfSndLvlPry(Suit suit)
        {
            bool has3ThirdLevel = false;
            bool has2SecondLevel = false;
            IList<Card> hand = _hand.Where(card => card.Suit == suit).ToList();

            if (LevelCardCount(Level.Third, hand) >= 3)
                has3ThirdLevel = true;

            if (LevelCardCount(Level.Second, hand) >= 2)
                has2SecondLevel = true;

            if (has3ThirdLevel && has2SecondLevel)
                return true;

            if (has3ThirdLevel && LevelCardCount(Level.Second, hand) == 1 && HasCapstone())
                return true;

            if (has2SecondLevel && LevelCardCount(Level.Third, hand) == 2 && HasCapstone())
                return true;

            return false;
        }

        public Suit? HasPerfectThirdLevelPyramid()
        {
            if (HasPerfThrdLvlPry(Suit.Orange))
                return Suit.Orange;

            if (HasPerfThrdLvlPry(Suit.Green))
                return Suit.Green;

            if (HasPerfThrdLvlPry(Suit.Purple))
                return Suit.Purple;

            return null;
        }

        public bool HasPerfThrdLvlPry(Suit suit)
        {
            bool has3ThirdLevel = false;
            bool has2SecondLevel = false;
            bool has1FirstLevel = false;
            IList<Card> hand = _hand.Where(card => card.Suit == suit).ToList();

            if (LevelCardCount(Level.Third, hand) >= 3)
                has3ThirdLevel = true;

            if (LevelCardCount(Level.Second, hand) >= 2)
                has2SecondLevel = true;

            if (LevelCardCount(Level.First, hand) >= 1)
                has1FirstLevel = true;

            if (has3ThirdLevel && has2SecondLevel && has1FirstLevel)
                return true;

            if (has3ThirdLevel && has2SecondLevel && HasCapstone())
                return true;

            if (has1FirstLevel && has3ThirdLevel && LevelCardCount(Level.Second, hand) == 1 && HasCapstone())
                return true;

            if (has1FirstLevel && has2SecondLevel && LevelCardCount(Level.Third, hand) == 2 && HasCapstone())
                return true;

            return false;
        }

        public bool HasFullPyramid()
        {
            bool has3ThirdLevel = false;
            bool has2SecondLevel = false;

            IList<Card> hand = _hand.Where(card => card.Suit == Suit.Purple).ToList();

            if (LevelCardCount(Level.Third, hand) >= 3)
                has3ThirdLevel = true;

            if (LevelCardCount(Level.Second, hand) >= 2)
                has2SecondLevel = true;

            if (has3ThirdLevel && has2SecondLevel && HasCapstone())
                return true;

            return false;
        }

        public Int32 GetHandValue()
        {
            if (HasFullPyramid())
                return 13;

            if (HasPerfectThirdLevelPyramid() == Suit.Purple)
                return 12;
            if (HasPerfectThirdLevelPyramid() == Suit.Green)
                return 11;
            if (HasPerfectThirdLevelPyramid() == Suit.Orange)
                return 10;

            if (HasThirdLevelPyramid())
                return 9;

            if (HasPerfectSecondLevelPyramid() == Suit.Purple)
                return 8;
            if (HasPerfectSecondLevelPyramid() == Suit.Green)
                return 7;
            if (HasPerfectSecondLevelPyramid() == Suit.Orange)
                return 6;

            if (HasSecondLevelPyramid())
                return 5;

            if (HasPerfectFirstLevelPyramid() == Suit.Purple)
                return 4;
            if (HasPerfectFirstLevelPyramid() == Suit.Green)
                return 3;
            if (HasPerfectFirstLevelPyramid() == Suit.Orange)
                return 2;

            if (HasFirstLevelPyramid())
                return 1;

            return 0;
        }

        public void SelectCardsForDiscard()
        {
            if (!IsPlaying) return;
            if (!IsBot) return;

            // No discards
            if (HasFullPyramid())
            {
                UnityEngine.Debug.Log(string.Format("{0} no discards.", Name));
                return;
            }
            if (HasPerfectThirdLevelPyramid() == Suit.Purple)
            {
                UnityEngine.Debug.Log(string.Format("{0} no discards.", Name));
                return;
            }
            if (HasPerfectThirdLevelPyramid() == Suit.Green)
            {
                UnityEngine.Debug.Log(string.Format("{0} no discards.", Name));
                return;
            }
            if (HasPerfectThirdLevelPyramid() == Suit.Orange)
            {
                UnityEngine.Debug.Log(string.Format("{0} no discards.", Name));
                return;
            }
            if (HasThirdLevelPyramid())
            {
                UnityEngine.Debug.Log(string.Format("{0} no discards.", Name));
                return;
            }

            // SelectCardsForDiscard 1
            if (HasPerfectSecondLevelPyramid() == Suit.Purple)
            {
                SelectDiscardPerfectSecondLevel(Suit.Purple);
                return;
            }
            if (HasPerfectSecondLevelPyramid() == Suit.Green)
            {
                SelectDiscardPerfectSecondLevel(Suit.Green);
                return;
            }
            if (HasPerfectSecondLevelPyramid() == Suit.Orange)
            {
                SelectDiscardPerfectSecondLevel(Suit.Orange);
                return;
            }
            if (HasSecondLevelPyramid())
            {
                SelectDiscardSecondLevel();
                return;
            }

            // SelectCardsForDiscard 3
            if (HasPerfectFirstLevelPyramid() == Suit.Purple)
            {
                SelectDiscardPerfectFirstLevel(Suit.Purple);
                return;
            }
            if (HasPerfectFirstLevelPyramid() == Suit.Green)
            {
                SelectDiscardPerfectFirstLevel(Suit.Green);
                return;
            }
            if (HasPerfectFirstLevelPyramid() == Suit.Orange)
            {
                SelectDiscardPerfectFirstLevel(Suit.Orange);
                return;
            }

            SelectDiscardFirstLevel();
        }

        public void AddCutits(Int32 cubits)
        {
            _cubits += cubits;
        }

        public void RemoveCubits(Int32 cubits)
        {
            if ((_cubits - cubits) >= 0)
                _cubits -= cubits;
        }

        public Int32 Bet(Int32 minBetLevel)
        {
            if (IsPlaying)
            {
                if (IsBot)
                {
                    if (_cubits < minBetLevel)
                    {
                        Console.WriteLine("{0} folds.", Name);
                        IsPlaying = false;
                        DiscardAllCards();
                        return 0;
                    }

                    // All In big bet
                    if (HasFullPyramid()
                        || HasPerfectThirdLevelPyramid() == Suit.Purple
                        || HasPerfectThirdLevelPyramid() == Suit.Green
                        || HasPerfectThirdLevelPyramid() == Suit.Orange
                        || HasThirdLevelPyramid())
                    {
                        if (_cubits > minBetLevel + 20)
                        {
                            CurrentBet = minBetLevel + 10;
                            Console.WriteLine("{0} bets {1} Cubits.", Name, CurrentBet);
                            RemoveCubits(CurrentBet);
                            return CurrentBet;
                        }
                    }

                    // Medium bet
                    if (HasPerfectSecondLevelPyramid() == Suit.Purple
                        || HasPerfectSecondLevelPyramid() == Suit.Green
                        || HasPerfectSecondLevelPyramid() == Suit.Orange
                        || HasSecondLevelPyramid())
                    {
                        if (_cubits > minBetLevel + 10)
                        {
                            CurrentBet = minBetLevel + 10;
                            Console.WriteLine("{0} bets {1} Cubits.", Name, CurrentBet);
                            RemoveCubits(CurrentBet);
                            return CurrentBet;
                        }
                    }

                    // Small bet
                    if (HasPerfectFirstLevelPyramid() == Suit.Purple
                        || HasPerfectFirstLevelPyramid() == Suit.Green
                        || HasPerfectFirstLevelPyramid() == Suit.Orange
                        || HasFirstLevelPyramid())
                    {
                        if (_cubits > minBetLevel + 5)
                        {
                            CurrentBet = minBetLevel + 5;
                            Console.WriteLine("{0} bets {1} Cubits.", Name, CurrentBet);
                            RemoveCubits(CurrentBet);
                            return CurrentBet;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("{0} bets {1} Cubits.", Name, CurrentBet);
                    RemoveCubits(CurrentBet);
                    return CurrentBet;
                }

                // no bet
                Console.WriteLine("{0} bets {1} Cubits.", Name, minBetLevel);
                return minBetLevel;
            }
            return 0;
        }

        public Int32 Call(Int32 minBetLevel)
        {
            if (CurrentBet >= minBetLevel)
                return 0;

            if ((minBetLevel - CurrentBet) > Cubits)
            {
                IsPlaying = false;
                DiscardAllCards();
                return 0;
            }

            var additionalBet = (CurrentBet - minBetLevel);
            RemoveCubits(additionalBet);

            return additionalBet;
        }

        private bool HasPerfFirLvlPyr(Suit suit)
        {
            bool has2ThirdLevel = false;
            bool has1SecondLevel = false;
            IList<Card> hand = _hand.Where(card => card.Suit == suit).ToList();

            if (LevelCardCount(Level.Third, hand) >= 2)
                has2ThirdLevel = true;

            if (LevelCardCount(Level.Second, hand) >= 1)
                has1SecondLevel = true;

            if (has2ThirdLevel && has1SecondLevel)
                return true;

            if (has2ThirdLevel && has1SecondLevel == false && HasCapstone())
                return true;

            if (has1SecondLevel && LevelCardCount(Level.Third, hand) == 1 && HasCapstone())
                return true;

            return false;
        }

        private void SelectDiscardFirstLevel()
        {
            Console.WriteLine(String.Format("{0} discarded 3 cards.", Name));
            if (LevelCardCount(Level.First) > 1)
            {
                Int32 firstLevelCount = 0;
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone 
                        && card.Level == Level.First)
                    {
                        if (firstLevelCount > 0)
                        {
                            SelectCardForDiscard(card);
                            firstLevelCount++;
                        }
                        else
                        {
                            firstLevelCount++;
                        }
                    }
                }
                if (SelectedCardCount() == 3)
                    return;
            }
            if (LevelCardCount(Level.Second) > 2)
            {
                Int32 secondLevelCount = 0;
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Level == Level.Second)
                    {
                        if (secondLevelCount > 0)
                        {
                            SelectCardForDiscard(card);
                            secondLevelCount++;
                        }
                        else
                        {
                            secondLevelCount++;
                        }
                    }
                }
                if (SelectedCardCount() == 3)
                    return;
            }
            if (LevelCardCount(Level.Third) > 3)
            {
                Int32 thirdLevelCount = 0;
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Level == Level.Third)
                    {
                        if (thirdLevelCount > 0)
                        {
                            SelectCardForDiscard(card);
                            thirdLevelCount++;
                        }
                        else
                        {
                            thirdLevelCount++;
                        }
                    }
                }
                if (SelectedCardCount() == 3)
                    return;
            }
        }

        private void SelectDiscardSecondLevel()
        {
            Console.WriteLine(String.Format("{0} discarded 1 card.", Name));
            // Remove one card second level
            if (LevelCardCount(Level.Second) > 2)
            {
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Level == Level.Second)
                    {
                        SelectCardForDiscard(card);
                        return;
                    }
                }
            }
            // Remove one card third level
            if (LevelCardCount(Level.Third) > 3)
            {
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Level == Level.Third)
                    {
                        SelectCardForDiscard(card);
                        return;
                    }
                }
            }
        }

        private void SelectDiscardPerfectFirstLevel(Suit suit)
        {
            if (HasPerfectFirstLevelPyramid() == suit)
            {
                Console.WriteLine(String.Format("{0} discarded 3 cards.", Name));
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Suit != suit)
                    {
                        SelectCardForDiscard(card);
                    }
                }
            }
        }

        private void SelectDiscardPerfectSecondLevel(Suit suit)
        {
            if (HasPerfectSecondLevelPyramid() == suit)
            {
                Console.WriteLine(String.Format("{0} discarded 1 card.", Name));
                // Remove one card not of same suit
                foreach (var card in Hand)
                {
                    if (!card.IsCapstone
                        && card.Suit != suit)
                    {
                        SelectCardForDiscard(card);
                        return;
                    }
                }
                // Remove one card same suit second level
                if (LevelCardCount(Level.Second) > 2)
                {
                    foreach (var card in Hand)
                    {
                        if (!card.IsCapstone
                            && card.Level == Level.Second)
                        {
                            SelectCardForDiscard(card);
                            return;
                        }
                    }
                }
                // Remove one card same suit third level
                if (LevelCardCount(Level.Third) > 3)
                {
                    foreach (var card in Hand)
                    {
                        if (!card.IsCapstone
                            && card.Level == Level.Third)
                        {
                            SelectCardForDiscard(card);
                            return;
                        }
                    }
                }
            }
        }

        private int SelectedCardCount()
        {
            return _hand.Count(card => card.IsSelected);
        }

        private int LevelCardCount(Level level)
        {
            return LevelCardCount(level, _hand);
        }

        private int LevelCardCount(Level level, IList<Card> hand)
        {
            return hand.Count(card => card.Level == level);
        }

        private bool HasCapstone()
        {
            return _hand.Any(card => card.IsCapstone);
        }
    }
}
