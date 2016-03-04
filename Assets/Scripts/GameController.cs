using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pyramid;
using UnityEngine.UI;

/*
 
Pyramid (Battlestar Galactica) is a card game based on the 1970s television series "Battlestar Galactica". It was depicted as leisure activity for the characters in the television show. Later on, rules and decks of pyramid playing cards were made available commercially to fans of the show. Both are currently out of print.
The game uses a 55 count deck of hexagonal cards separated into three kinds of suits and three ranks as well as a 'capstone' card which is used as a wild card. The game is played very similarly to poker. Players are dealt hands and place bets based on how strong their hand is. There is a discard between bets and players reveal their hands to determine the winner of that round.

Winning hands are determined by a number of factors including suit, rank and matching sets. There are approximately fourteen different hand combinations possible in the game. There is no 'end' to the game, players merely quit at their leisure.

The deck consists of 55 hexagonal cards, which is divided into 3 suits: purple, green and orange. Purple ranks highest and orange ranks lowers. The deck also contains a Capstone (*).

Each suit contains:
- 3 First level cards
- 6 Second level cards
- 9 Third level cards

How To Play
Each player is dealt 6 cards. If a player feels a better hand is possible, he may discard up to 4 cards and receive replacements from the dealer. This may only be done once per hand, much the same as "Draw Poker". Players then construct their best possible pyramid and compare to the other player hands to determine the winner.
Ranks of Hands
A pyramid is made up of at least three cards.

1. First level pyramid consist of 2 third level, and 1 second level card.

2. Second level pyramid consist of 3 third level, and 2 second level card.

3. Third level pyramid consist of 3 third level, 2 second level cards, and 1 first level card.

4. Any of the above may be considered "perfect" when all of the cards used are of the same color. ("Perfect" pyramids rank higher than non-perfect pyramids.)

5. If two or more perfect pyramids are the same level, then the highest suit would win. (NOTE: Only perfect pyramids can be ranked by suit; non-perfect pyramids are considered to have no suit.)

6. If two or more pyramids are still equal, then the persons play another hand to decide the winner.

) The Capstone can be used in place of the first level card in a Perfect third level pyramid. This is the highest of ALL PYRAMIDS!
) The Capstone may also be used as a wild card to replace any card in any pyramid.
 */

public class GameController : MonoBehaviour
{
    private GameManager _gameManager;
    public Button BtnAntiUp;
    public Button BtnDealCards;
    public Button BtnDiscard;
    // TODO: Make this an array
    public Button BtnCard1;
    public Button BtnCard2;
    public Button BtnCard3;
    public Button BtnCard4;
    public Button BtnCard5;
    public Button BtnCard6;
    public Button BtnBet;
    public Button BtnFold;
    public Button BtnCall;
    public Button BtnPlayAgain;
    public Button BtnRematch;

    private GameObject PlayerCardLocation1;
    private GameObject PlayerCardLocation2;
    private GameObject PlayerCardLocation3;
    private GameObject PlayerCardLocation4;
    private GameObject PlayerCardLocation5;
    private GameObject PlayerCardLocation6;

    private UnityEngine.Object _playerCard1;
    private UnityEngine.Object _playerCard2;
    private UnityEngine.Object _playerCard3;
    private UnityEngine.Object _playerCard4;
    private UnityEngine.Object _playerCard5;
    private UnityEngine.Object _playerCard6;

    public GameObject CardBack;
    public GameObject CapStone;
    public GameObject FirstPurple;
    public GameObject FirstGreen;
    public GameObject FirstOrange;
    public GameObject SecondPurple;
    public GameObject SecondGreen;
    public GameObject SecondOrange;
    public GameObject ThirdPurple;
    public GameObject ThirdGreen;
    public GameObject ThirdOrange;
    public GameObject Selected;
    public Text TxtPotValue;
    public Text TxtPlayer0Status;
    public Text TxtPlayer1Status;
    public Text TxtPlayer2Status;
    public Text TxtPlayer3Status;
    public Text TxtGameResult;
    public InputField TxtBetAmount;

    private Player _localPlayer;

    private const float CardYPosNotSelected = -0.25f;
    private const float CardYPosSelected = -0.15f;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");
        _gameManager = new GameManager();
        Debug.Log("Game Manager created");
        _gameManager.AddPlayer(new Player() { Name = "Starbuck", IsBot = true, IsPlaying = true });
        _gameManager.AddPlayer(new Player() { Name = "Redeye", IsBot = true, IsPlaying = true });
        _gameManager.AddPlayer(new Player() { Name = "Boxey", IsBot = true, IsPlaying = true });
        _localPlayer = new Player() { Name = "Quell", IsBot = false, IsPlaying = true };
        _gameManager.AddPlayer(_localPlayer);

        Debug.Log("Players added");
        HideButtons();
        Debug.Log("Buttons hidden");
        BtnAntiUp.gameObject.SetActive(true);

        PlayerCardLocation1 = GameObject.Find("PlayerCard1");
        PlayerCardLocation2 = GameObject.Find("PlayerCard2");
        PlayerCardLocation3 = GameObject.Find("PlayerCard3");
        PlayerCardLocation4 = GameObject.Find("PlayerCard4");
        PlayerCardLocation5 = GameObject.Find("PlayerCard5");
        PlayerCardLocation6 = GameObject.Find("PlayerCard6");

        ShowPlayers(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AntiUp()
    {
        Debug.Log("AntiUp");
        _gameManager.AntiUpCubits();
        UpdatePotValue();
        ShowPlayers(false);
        HideButtons();
        BtnDealCards.gameObject.SetActive(true);
    }

    public void DealCards()
    {
        Debug.Log("DealCards");
        ShowCardBacks();
        _gameManager.DealCards();
        HideButtons();
        BtnDiscard.gameObject.SetActive(true);
        ShowHand();
        ShowPlayers(false);
    }

    private void ShowCardBacks()
    {
        GameObject cardBack = CardBack;
        //cardBack.transform.localScale += new Vector3(1f, 1f, 0);

        _playerCard1 = Instantiate(cardBack, PlayerCardLocation1.transform.position, Quaternion.identity);
        _playerCard2 = Instantiate(cardBack, PlayerCardLocation2.transform.position, Quaternion.identity);
        _playerCard3 = Instantiate(cardBack, PlayerCardLocation3.transform.position, Quaternion.identity);
        _playerCard4 = Instantiate(cardBack, PlayerCardLocation4.transform.position, Quaternion.identity);
        _playerCard5 = Instantiate(cardBack, PlayerCardLocation5.transform.position, Quaternion.identity);
        _playerCard6 = Instantiate(cardBack, PlayerCardLocation6.transform.position, Quaternion.identity);
    }

    public void SelectCard(Int32 cardIndex)
    {
        Debug.Log("SelectCard");
        _localPlayer.SelectCardForDiscard(_localPlayer.Hand[cardIndex]);
        ShowHand();
    }

    public void Discard()
    {
        Debug.Log("Discard");
        _gameManager.Discard();
        _gameManager.DealCards();
        ShowHand();
        ShowPlayers(false);
        ShowPlayerHandValue(_gameManager.Players[3], TxtGameResult, true);
        SetCardSelectButtons(false);
        HideButtons();
        BtnBet.gameObject.SetActive(true);
        BtnFold.gameObject.SetActive(true);
        TxtBetAmount.gameObject.SetActive(true);
    }

    public void Bet()
    {
        Debug.Log("Bet");
        Int32 betAmount;
        if (Int32.TryParse(TxtBetAmount.text, out betAmount))
        {
            if (_localPlayer.Cubits >= betAmount)
            {
                _localPlayer.CurrentBet = betAmount;
                _gameManager.Bet();
                UpdatePotValue();
                ShowPlayers(false);
                BtnBet.gameObject.SetActive(false);
                BtnFold.gameObject.SetActive(false);
                TxtBetAmount.gameObject.SetActive(false);

                Debug.Log("Call");
                _gameManager.Call();
                BtnFold.gameObject.SetActive(true);
                BtnCall.gameObject.SetActive(true);
                BtnCall.GetComponentInChildren<Text>().text = string.Format("Call (+{0})", (_gameManager.MinBet - _localPlayer.CurrentBet));
                ShowPlayers(false);
                if (_localPlayer.CurrentBet >= _gameManager.MinBet)
                {
                    BtnFold.gameObject.SetActive(false);
                    BtnCall.gameObject.SetActive(false);
                    DetermineWinner();
                    ShowPlayers(true);
                }
            }
        }
    }

    public void Call()
    {
        if (_localPlayer.Cubits >= _gameManager.MinBet)
        {
            _gameManager.Pot += (_gameManager.MinBet - _localPlayer.CurrentBet);
            _localPlayer.RemoveCubits((_gameManager.MinBet - _localPlayer.CurrentBet));
            _localPlayer.CurrentBet = _gameManager.MinBet;
        }
        BtnFold.gameObject.SetActive(false);
        BtnCall.gameObject.SetActive(false);
        DetermineWinner();
        ShowPlayers(true);
    }

    public void Fold()
    {
        _localPlayer.IsPlaying = false;
        _localPlayer.DiscardAllCards();
        BtnBet.gameObject.SetActive(false);
        BtnFold.gameObject.SetActive(false);
        TxtBetAmount.gameObject.SetActive(false);
        BtnCall.gameObject.SetActive(false);
        _gameManager.Bet();
        UpdatePotValue();
        DetermineWinner();
    }

    public void PlayAgain()
    {
        Debug.Log("PlayAgain");
        TxtGameResult.gameObject.SetActive(false);
        BtnRematch.gameObject.SetActive(false);
        BtnPlayAgain.gameObject.SetActive(false);
        TxtPlayer0Status.text = string.Empty;
        TxtPlayer1Status.text = string.Empty;
        TxtPlayer2Status.text = string.Empty;
        TxtPlayer3Status.text = string.Empty;
        HideHand();

        var players = _gameManager.Players;
        _gameManager = new GameManager();
        UpdatePotValue();
        Debug.Log("Game Manager created");
        foreach (var player in players)
        {
            player.DiscardAllCards();
            _gameManager.AddPlayer(player);
        }
        Debug.Log("Players added");
        HideButtons();
        Debug.Log("Buttons hidden");
        _gameManager.ResetBets();
        BtnAntiUp.gameObject.SetActive(true);
    }

    public void Rematch()
    {
        Debug.Log("Tie - Rematch");
        _gameManager.ResetBets();
        TxtGameResult.gameObject.SetActive(false);
        BtnRematch.gameObject.SetActive(false);
        BtnPlayAgain.gameObject.SetActive(false);
        TxtPlayer0Status.text = string.Empty;
        TxtPlayer1Status.text = string.Empty;
        TxtPlayer2Status.text = string.Empty;
        TxtPlayer3Status.text = string.Empty;
        HideHand();

        var rematchPlayers = _gameManager.RematchPlayers;
        var players = _gameManager.Players;
        Int32 potVale = _gameManager.Pot;
        _gameManager = new GameManager();
        _gameManager.Pot = potVale;
        UpdatePotValue();
        Debug.Log("Game Manager created");
        foreach (var player in players)
        {
            player.DiscardAllCards();
            if (!rematchPlayers.Contains(player))
            {
                Debug.Log(string.Format("{0} not in rematch", player.Name));
                player.IsPlaying = false;
            }
            _gameManager.AddPlayer(player);
        }
        Debug.Log("Players added");
        HideButtons();
        Debug.Log("Buttons hidden");
        if (_localPlayer.IsPlaying)
        {
            BtnAntiUp.gameObject.SetActive(true);
        }
        else
        {
            _gameManager.AntiUpCubits();
            UpdatePotValue();
            _gameManager.DealCards();
            _gameManager.Discard();
            _gameManager.DealCards();
            _gameManager.Bet();
            UpdatePotValue();
            DetermineWinner();
            ShowPlayers(true);
        }
    }

    private void DetermineWinner()
    {
        Debug.Log("DetermineWinner");
        var winningPlayer = _gameManager.DetermineWinner();
        Debug.Log(string.Format("{0} won the game.", winningPlayer.Name));
        Debug.Log(winningPlayer.Results);

        if (_gameManager.IsTieGame)
        {
            Debug.Log("Tie Game");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Tie Game!");
            foreach (var player in _gameManager.RematchPlayers)
            {
                sb.AppendLine(string.Format("{0}"
                    , player.Name));
            }
            TxtGameResult.text = sb.ToString();
            BtnRematch.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("NOT Tie Game");
            TxtGameResult.text = string.Format("{0} has won {1} cubits:\n{2}"
                , winningPlayer.Name
                , _gameManager.Pot
                , winningPlayer.Results);
            winningPlayer.AddCutits(_gameManager.Pot);
            BtnPlayAgain.gameObject.SetActive(true);
        }
        TxtGameResult.gameObject.SetActive(true);
    }

    private void ShowPlayers(bool showAllPlayerResults)
    {
        ShowPlayerHandValue(_gameManager.Players[0], TxtPlayer0Status, showAllPlayerResults);
        ShowPlayerHandValue(_gameManager.Players[1], TxtPlayer1Status, showAllPlayerResults);
        ShowPlayerHandValue(_gameManager.Players[2], TxtPlayer2Status, showAllPlayerResults);
        ShowPlayerHandValue(_gameManager.Players[3], TxtPlayer3Status, true);
    }

    private void ShowPlayerHandValue(Player player, Text textBox, bool showAllPlayerResults)
    {
        if (player.IsPlaying == false)
        {
            textBox.text = string.Format("{0}\nCubits: {1}\nNOT PLAYING"
                , player.Name
                , player.Cubits);
        }
        else if (showAllPlayerResults)
        {
            textBox.text = string.Format("{0}\nCubits: {1}\nBet: {2}\n{3}"
                , player.Name
                , player.Cubits
                , player.CurrentBet
                , player.Results);
        }
        else
        {
            textBox.text = string.Format("{0}\nCubits: {1}\nBet: {2}"
                , player.Name
                , player.Cubits
                , player.CurrentBet);
        }
    }

    private void ShowHand()
    {
        Debug.Log("ShowHand");
        var player = _gameManager.Players[3];

        DestoryCards();

        Debug.Log(player.Name);
        Debug.Log(player.Cubits);
        if (player.Hand.Count > 0)
        {
            _playerCard1 = ShowCard(player.Hand[0], PlayerCardLocation1);
        }
        if (player.Hand.Count > 1)
        {
            _playerCard2 = ShowCard(player.Hand[1], PlayerCardLocation2);
        }
        if (player.Hand.Count > 2)
        {
            _playerCard3 = ShowCard(player.Hand[2], PlayerCardLocation3);
        }
        if (player.Hand.Count > 3)
        {
            _playerCard4 = ShowCard(player.Hand[3], PlayerCardLocation4);
        }
        if (player.Hand.Count > 4)
        {
            _playerCard5 = ShowCard(player.Hand[4], PlayerCardLocation5);
        }
        if (player.Hand.Count > 5)
        {
            _playerCard6 = ShowCard(player.Hand[5], PlayerCardLocation6);
        }

        SetCardSelectButtons(true);
    }

    private void DestoryCards()
    {
        Destroy(_playerCard1);
        Destroy(_playerCard2);
        Destroy(_playerCard3);
        Destroy(_playerCard4);
        Destroy(_playerCard5);
        Destroy(_playerCard6);
    }

    private UnityEngine.Object ShowCard(Card card, GameObject cardPosition)
    {
        UnityEngine.Object retVal = null;
        GameObject displayCard = null;
        Debug.Log(string.Format("{0} {1}", card.CardDescription, card.IsSelected ? "(*)" : ""));

        if (card.IsCapstone)
        {
            displayCard = CapStone;
            Debug.Log("display capstone");
        }
        else
        {

            if (card.Suit == Suit.Purple)
            {
                if (card.Level == Level.First)
                {
                    displayCard = FirstPurple;
                }
                if (card.Level == Level.Second)
                {
                    displayCard = SecondPurple;
                }
                if (card.Level == Level.Third)
                {
                    displayCard = ThirdPurple;
                }
            }
            if (card.Suit == Suit.Green)
            {
                if (card.Level == Level.First)
                {
                    displayCard = FirstGreen;
                }
                if (card.Level == Level.Second)
                {
                    displayCard = SecondGreen;
                }
                if (card.Level == Level.Third)
                {
                    displayCard = ThirdGreen;
                }
            }
            if (card.Suit == Suit.Orange)
            {
                if (card.Level == Level.First)
                {
                    displayCard = FirstOrange;
                }
                if (card.Level == Level.Second)
                {
                    displayCard = SecondOrange;
                }
                if (card.Level == Level.Third)
                {
                    displayCard = ThirdOrange;
                }
            }
        }

        displayCard.transform.localScale = new Vector3(0.25F, 0.25F, 1F);

        retVal = card.IsSelected ? Instantiate(displayCard, cardPosition.transform.position = new Vector3(cardPosition.transform.position.x, CardYPosSelected, cardPosition.transform.position.z), Quaternion.identity) : Instantiate(displayCard, cardPosition.transform.position = new Vector3(cardPosition.transform.position.x, CardYPosNotSelected, cardPosition.transform.position.z), Quaternion.identity);
        
        return retVal;
    }

    private void HideHand()
    {
        HideCard(BtnCard1);
        HideCard(BtnCard2);
        HideCard(BtnCard3);
        HideCard(BtnCard4);
        HideCard(BtnCard5);
        HideCard(BtnCard6);
        DestoryCards();
    }

    private void HideCard(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void SetCardSelectButtons(bool isActive)
    {
        Debug.Log("Set Card Select Buttons");

        BtnCard1.gameObject.SetActive(isActive);
        BtnCard2.gameObject.SetActive(isActive);
        BtnCard3.gameObject.SetActive(isActive);
        BtnCard4.gameObject.SetActive(isActive);
        BtnCard5.gameObject.SetActive(isActive);
        BtnCard6.gameObject.SetActive(isActive);
    }

    private void UpdatePotValue()
    {
        Debug.Log("UpdatePotValue");
        TxtPotValue.text = string.Format("Pot: {0}", _gameManager.Pot);
    }

    private void HideButtons()
    {
        BtnAntiUp.gameObject.SetActive(false);
        BtnDealCards.gameObject.SetActive(false);
        BtnDiscard.gameObject.SetActive(false);
    }

    public void Play()
    {
        _gameManager.AntiUpCubits();

        _gameManager.DealCards();

        _gameManager.Discard();

        _gameManager.DealCards();

        _gameManager.Bet();

        var winner = _gameManager.DetermineWinner();

        if (winner != null
            && _gameManager.IsTieGame == false)
        {
            _gameManager.PayWinner(winner);
        }
    }
}





