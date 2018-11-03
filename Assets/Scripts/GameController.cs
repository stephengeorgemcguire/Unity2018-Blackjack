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
    public Text BankTxt { get; set; }
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

    }

    private void InitializeDeal()
    {
        ClearHands();
        ResetResults();
        PlayerBust = false;
        if ( Bank <= 0 )
        {
            ToggleButton( PlayAgainButton, false );
            return;
        }
        handsPlayed++;
        Bank = Bank < Bet ? Bank : Bank -= Bet;


    }
    private void DealHand()
    {
        InitializeDeal();
        for ( int i = 0 ; i < 2 ; i++ )
        {
            HitPlayer();
            HitDealer();
        }

        // Blackjack only on first two cards
        if ( IsBlackjack() )
            InitializeDeal();

        int card1Idx = Player.deck[ 0 ];
        int card2Idx = Player.deck[ 1 ];
        if ( Player.GetCardRank( card1Idx ) == Player.GetCardRank( card2Idx ) )
            ToggleButton( SplitButton, true );
    }
    public void HitPlayer()
    {
        ToggleButton( DoubleDownButton, OFF );
        Player.Push( Deck.Pop() );
        HandStatus();
    }



    public void DoubleDown()
    {

        HitPlayer();    // Player gets one card
        Hold();
    }

    public void Hold()
    {
        InitializePlayButtons( false );
        ShowDealerHand();
        StartCoroutine( DealerPlay() );
        DetermineWinner();
        UpdateStats();

    }

    public void Split()
    {
        // TODO:
    }





    private void HandStatus()
    {
        PlayerScore.text = Player.HandValue().ToString();
        if ( Player.HandValue() == 21 )
            Hold();
        else if ( Player.HandValue() > 21 )
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

        Player.GetComponent<DeckView>().ClearHand();
        Dealer.GetComponent<DeckView>().ClearHand();
    }

    private void InitializePlayButtons( bool isInteractable )
    {
        // These are the initial states of the Player buttons before each hand
        ToggleButton( HitButton, isInteractable );
        ToggleButton( HoldButton, isInteractable );
        ToggleButton( DoubleDownButton, isInteractable );

        ToggleButton( SplitButton, false );
        ToggleButton( PlayAgainButton, !isInteractable );
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
            BlackJack();
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
            while ( Dealer.HandValue() < 17 )
            {

                HitDealer();
                DealerScore.text = Dealer.HandValue().ToString();
                yield return new WaitForSeconds( 1f );
            }
        }
    }
    #endregion

    #region Game Results
    private void UpdateStats()
    {

    }


    private void DetermineWinner()
    {
        if ( Player.HandValue() > 21 )
            Lose();
        else if ( Dealer.HandValue() > 21 )
        {
            Win();
        }
        else
        {
            if ( Player.HandValue() == Dealer.HandValue() )
                Push();
            else if ( Player.HandValue() > Dealer.HandValue() )
                Win();
            else
                Lose();
        }
        WinningPct.text = (((float)handsWon / (float)handsPlayed) * 100.00).ToString( "0.00" );

    }

    private void Lose()
    {
        GameResult.color = Color.red;
        GameResult.text = "Lose";
    }

    private void Push()
    {
        Bank += Bet;
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
        Bank += Bet * payOff;
        handsWon++;
        GameResult.color = Color.green;
        GameResult.text = "Win!";
    }
    #endregion
}
