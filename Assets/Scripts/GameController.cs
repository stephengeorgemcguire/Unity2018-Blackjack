using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Deck Player;
    public Deck Dealer;
    public Deck Deck;

    private const bool ON = true;
    private const bool OFF = false;


    public Button HitButton;
    public Button HoldButton;
    public Button PlayAgainButton;
    public Button DoubleDownButton;
    public Button SplitButton;

    public Text PlayerScore;
    public Text DealerScore;
    public Text GameResult;
    public Text BankValue;
    public Text CurrentBet;
    public Text WinningPct;

    private float BeginningPurse;
    private float Bank;
    public float Bet;
    public float InitialBank;
    private float HandResult;


    private int handsPlayed = 0;
    private int handsWon = 0;
    private bool PlayerBust { get; set; }

    private void Start()
    {
        Bank = InitialBank;
        Play();
    }

    #region Player Actions
    public void Play()
    {
        InitializeDeal();
        if ( Deck.CardCount < 15 )
            Deck.Reset();

        DealHand();


        if ( !canSplit() )
            ToggleButton( SplitButton, OFF );


        if ( IsBlackjack() )
        {
            TogglePlayButtons( OFF );
            ToggleButton( PlayAgainButton, ON );
            return;
        }
        // See if we can split the hand
        HandStatus();


    }

    private void InitializeDeal()
    {
        ClearHands();
        ResetResults();
        TogglePlayButtons( ON );
        ToggleButton( PlayAgainButton, OFF );

        PlayerBust = false;
        if ( Bank > 0 ) // If we have money in the bank
        {
            // If we can not cover the bet, use whatever is left in the bank
            Bank = Bank < Bet ? Bank : Bank -= Bet;
            updateBankDisplay();
        }
        else
        {
            ToggleButton( PlayAgainButton, OFF );
            return;
        }




    }
    private void DealHand()
    {
        try
        {
            handsPlayed++;
            for ( int i = 0 ; i < 2 ; i++ )
            {
                HitPlayer();
                HitDealer();
            }
        }
        catch ( ApplicationException ex )
        {
            throw new ApplicationException( "GameController.cs - DealHand()", ex );
        }
    }

    private bool canSplit()
    {
        try
        {


            if ( Player.deck == null )
                return false;

            int card1 = Player.deck[ 0 ];
            int card2 = Player.deck[ 1 ];

            card1 = Player.GetCardRank( card1 );
            card2 = Player.GetCardRank( card2 );

            return (card1 == card2);
        }
        catch ( ApplicationException ex )
        {
            throw new ApplicationException( "GameController.cs - canSplit(): ", ex );
        }
    }

    public void Hit()
    {
        HitPlayer();
        ToggleButton( DoubleDownButton, OFF );
        HandStatus();


    }
    public void HitPlayer()
    {

        Player.Push( Deck.Pop() );



    }



    public void DoubleDown()
    {
        Bet += Bet;
        Bank -= Bet;
        updateBankDisplay();
        Hit();    // Player gets one card
        Hold();
    }

    public void Hold()
    {
        TogglePlayButtons( OFF );

        ShowDealerHand();
        StartCoroutine( DealerPlay() );
        //DealerPlay();
        ToggleButton( PlayAgainButton, ON );
    }

    public void Split()
    {
        // TODO:
    }

    private void HandStatus()
    {
        int handValue = Player.HandValue();
        PlayerScore.text = handValue.ToString();
        if ( handValue == 21 )
            Hold();
        else if ( handValue > 21 )
        {
            PlayerBust = true;
            Hold();
        }
    }
    public void QuitGame()
    {
        // TODO:
    }

    #endregion

    #region Game Play Actions

    private void ToggleButton( Button button, bool toggle )
    {
        button.interactable = toggle;
    }


    private void ShowDealerHand()
    {
        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.ShowHand();
    }

    private void ClearHands()
    {
        try
        {

            Player.GetComponent<DeckView>().ClearHand();
            Dealer.GetComponent<DeckView>().ClearHand();
        }
        catch ( Exception ex )
        {
            throw new ApplicationException( "GameController.cs - ClearHands(): ", ex );
        }
    }

    private void TogglePlayButtons( bool isInteractable )
    {
        // These are the initial states of the Player buttons before each hand
        ToggleButton( HitButton, isInteractable );
        ToggleButton( HoldButton, isInteractable );
        ToggleButton( DoubleDownButton, isInteractable );
        ToggleButton( PlayAgainButton, isInteractable );
        ToggleButton( SplitButton, isInteractable );

    }
    private void ResetResults()
    {
        PlayerScore.text = "";
        DealerScore.text = "";
        GameResult.text = "";
        CurrentBet.text = Bet.ToString();
    }
    private void ToggleView( int cardIdx )
    {
        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.Toggle( cardIdx, true );
    }

    private bool IsBlackjack()
    {
        bool isBlackjack = true;

        if ( Player.HandValue() == 21 && Dealer.HandValue() == 21 )
            Push();
        else if ( Player.HandValue() == 21 )
            Win( 2.5f );
        else if ( Dealer.HandValue() == 21 )
            Lose();
        else
            isBlackjack = false;

        return isBlackjack;
    }


    #endregion

    #region Dealer Actions
    private void HitDealer()
    {
        int cardIdx = Deck.Pop();

        Dealer.Push( cardIdx );
        if ( Dealer.CardCount >= 2 )
        {
            ToggleView( cardIdx );
        }
    }
    IEnumerator DealerPlay()
    {
        if ( !PlayerBust )
        {
            int firstCard = Dealer.deck[ 0 ];
            ToggleView( firstCard );
            updateDealerScore();
            while ( Dealer.HandValue() < 17 )
            {
                HitDealer();
                updateDealerScore();

                yield return new WaitForSeconds( 1f );
            }
        }
        EndGame();
    }

    private void updateDealerScore()
    {
        DealerScore.text = Dealer.HandValue().ToString();
    }

    private void EndGame()
    {
        DetermineWinner();
        UpdateStats();
    }
    #endregion

    #region Game Results
    private void UpdateStats()
    {
        try
        {
            if ( handsPlayed > 0 )
                WinningPct.text = (((float)handsWon / (float)handsPlayed) * 100.00).ToString( "0.00" ) + "%";

        }
        catch ( Exception ex )
        {
            throw new ApplicationException( string.Format( "Can not calculate Winning %.  HandsPlayed: {0}; HandsWon: {1}. GameController.cs - UpdateStats() ", (int)handsPlayed, (int)handsWon ), ex );
        }
    }


    private void DetermineWinner()
    {
        if ( Player.HandValue() > 21 )
            Lose();
        else if ( Dealer.HandValue() > 21 )
        {
            Win();
        }
        else // Dealer and Player are still in
        {
            if ( Player.HandValue() == Dealer.HandValue() )
                Push();
            else if ( Player.HandValue() > Dealer.HandValue() )
                Win();
            else
                Lose();
        }


    }

    private void Lose()
    {
        GameResult.color = Color.red;
        GameResult.text = "Lose";
    }

    private void Push()
    {
        Bank += Bet;
        updateBankDisplay();
        handsPlayed--;
        GameResult.color = Color.white;
        GameResult.text = "Push";
    }

    private void BlackJack()
    {
        // Blackjack pays out 2.5 times the original bet
        Win( (float)2.5 );
    }

    private void Win( float payOff = (float)2.0 )
    {
        Bank += (Bet * payOff);
        updateBankDisplay();
        handsWon++;
        GameResult.color = Color.green;
        GameResult.text = "Win!";
    }

    private void updateBankDisplay()
    {
        BankValue.text = Bank.ToString( "$0.00" );
    }
    #endregion
}
