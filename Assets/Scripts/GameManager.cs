using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Pyramid
{
    public class GameManager
    {
        private const Int32 MaxCards = 6;
        private const Int32 MinBuyInCubits = 10;
        private Deck _deck;
        private IList<Player> _players;
        private IList<Player> _rematchPlayerList;

        public IList<Player> RematchPlayers
        {
            get { return _rematchPlayerList; }
        }
        public IList<Player> Players
        {
            get
            {
                return _players;
            }
        }

        private Int32 _pot;
        public Int32 Pot
        {
            get { return _pot; }
            set { _pot = value; }
        }
        public Int32 MinBet { get; set; }
        public bool IsTieGame { get; set; }

        public GameManager()
        {
            _deck = new Deck();
            _players = new List<Player>();
            _rematchPlayerList = new List<Player>();
            IsTieGame = false;
        }

        public void AddPlayer(Player player)
        {
            if (_players.Count < 4)
            {
                _players.Add(player);
                Debug.Log(String.Format("New player has entered the game: {0}", player.Name));
            }
        }

        public void AntiUpCubits()
        {
            ResetNotPlaying();
            foreach (var player in _players)
            {
                if (player.IsPlaying)
                {
                    if (player.Cubits > MinBuyInCubits)
                    {
                        Debug.Log(string.Format("{0} added {1} Cubits to the pot.", player.Name, MinBuyInCubits));
                        _pot += MinBuyInCubits;
                        player.RemoveCubits(MinBuyInCubits);
                        player.IsPlaying = true;
                    }
                    else
                    {
                        player.IsPlaying = false;
                    }
                }
            }
        }

        public void Bet()
        {
            MinBet = 0;
            foreach (var player in _players)
            {
                if (player.IsPlaying)
                {
                    player.Bet(MinBet);
                    _pot += player.CurrentBet;
                    if (player.CurrentBet > MinBet)
                    {
                        MinBet = player.CurrentBet;
                    }
                }
            }
        }

        public void Call()
        {
            Int32 minBet = 0;
            foreach (var player in Players)
            {
                if (player.IsPlaying
                    && player.CurrentBet > minBet)
                {
                    minBet = player.CurrentBet;
                }
            }

            foreach (var player in Players)
            {
                if (player.IsPlaying)
                {
                    if (player.IsBot)
                    {
                        _pot += player.Call(minBet);
                    }
                }
            }
        }

        public void PayWinner(Player winner)
        {
            Debug.Log(string.Format("{0} won {1} Cubits!", winner.Name, _pot));
            winner.AddCutits(_pot);
            _pot = 0;
        }

        public void DealCards()
        {
            Debug.Log("Dealing cards.");
            foreach (var player in _players)
            {
                if (player.IsPlaying)
                {
                    while (player.Hand.Count < MaxCards)
                    {
                        player.AddCard(_deck.DrawCard());
                    }
                }
            }
        }

        public void Discard()
        {
            foreach (var player in _players)
            {
                if (player.IsPlaying)
                {
                    if (player.IsBot)
                    {
                        player.SelectCardsForDiscard();
                    }
                    player.DiscardSelectedCards();
                }
            }
        }

        public Player DetermineWinner()
        {
            Debug.Log("GameManager DetermineWinner");
            Player winner = null;
            foreach (var player in _players)
            {
                if (player.IsPlaying)
                {
                    Debug.Log(player.ShowHand);
                    Debug.Log(string.Format("{0} handvalue: {1} has {2}"
                        , player.Name
                        , player.GetHandValue()
                        , player.Results));
                    if (winner != null
                        && player.GetHandValue() == winner.GetHandValue())
                    {
                        Debug.Log(string.Format("player handvalue: {0} winner handvalue: {1}"
                            , player.GetHandValue()
                            , winner.GetHandValue()));
                        // Tie! rematch.
                        IsTieGame = true;
                        if (!_rematchPlayerList.Contains(winner))
                        {
                            _rematchPlayerList.Add(winner);
                        }
                        if (!_rematchPlayerList.Contains(player))
                        {
                            _rematchPlayerList.Add(player);
                        }
                    }
                    if (winner == null
                        && player.GetHandValue() > 0)
                        winner = player;
                    if (winner != null && player.GetHandValue() > winner.GetHandValue())
                    {
                        winner = player;
                        IsTieGame = false;
                    }
                }
            }
            if (IsTieGame)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("The game was a tie.  The following players will rematch:");
                foreach (var player in _rematchPlayerList)
                {
                    sb.AppendLine(player.Name);
                    player.DiscardAllCards();
                }

                Debug.Log(sb.ToString());
            }
            else
            {
                if (winner != null)
                {
                    Debug.Log(string.Format("{0} has won the game.", winner.Name));
                }    
            }
            return winner;
        }

        public void ResetBets()
        {
            foreach (var player in _players)
            {
                player.CurrentBet = 0;
            }
        }

        public void ResetNotPlaying()
        {
            foreach (var player in _players)
            {
                if (player.Cubits > 20)
                {
                    player.IsPlaying = true;
                }
                else
                {
                    Debug.Log(string.Format("{0} Busted.  Has bought back into the game with 100 cubits.", player.Name));
                    player.AddCutits(100);
                }
            }
        }
    }
}
