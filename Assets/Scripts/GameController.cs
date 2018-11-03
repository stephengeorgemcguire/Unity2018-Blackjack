using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Deck Player;
    public Deck Dealer;
    public Deck Deck;

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
        DealHand();
    }

    #region Player Actions

    public void HitPlayer()
    {
        Player.Push( Deck.Pop() );
        HandStatus();
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

    public void DoubleDown()
    {
        HitPlayer();    // Player gets one card

        Hold();
    }

    public void Hold()
    {
        TurnOnPlayButtons( false );
        ShowDealerHand();
        StartCoroutine( DealerPlay() );
        DetermineWinner();
        UpdateStats();
    }

    private void UpdateStats()
    {

    }

    private void ShowDealerHand()
    {
        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.ShowHand();
    }

    public void Split()
    {
        // TODO:
    }

    public void PlayAgain()
    {
        ResetResults();
        ClearHands();

        TurnOnPlayButtons( true );

        if ( Deck.CardCount < 15 )
            Deck.Reset();

        DealHand();

    }

    public void QuitGame()
    {
        // TODO:
    }

    #endregion

    #region Game Play Actions
    private void ClearHands()
    {

        Player.GetComponent<DeckView>().ClearHand();
        Dealer.GetComponent<DeckView>().ClearHand();
    }

    private void ResetResults()
    {
        PlayerScore.text = "";
        DealerScore.text = "";
        GameResult.text = "";
        CurrentBet.text = Bet.ToString();
    }


    private void DealHand()
    {
        TurnOnPlayButtons( true );
        PlayerBust = false;
        handsPlayed++;
        Bank -= Bet;
        for ( int i = 0 ; i < 2 ; i++ )
        {
            HitPlayer();
            HitDealer();
        }
        if ( IsBlackjack() )
            TurnOnPlayButtons( false );
    }

    private void ToggleView( int cardIdx )
    {
        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.Toggle( cardIdx, true );
    }

    private void TurnOnPlayButtons( bool isInteractable )
    {
        HitButton.interactable = isInteractable;
        HoldButton.interactable = isInteractable;
        DoubleDownButton.interactable = isInteractable;
        SplitButton.interactable = isInteractable;
        PlayAgainButton.interactable = !isInteractable;
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

    private bool IsBlackjack()
    {
        bool isBlackjack = true;

        if ( Player.HandValue() == 21 && Dealer.HandValue() == 21 )
            Push();
        else if ( Player.HandValue() == 21 )
            BlackJack();
        else if ( Dealer.HandValue() == 21 )
            PlayerLoses();
        else
            isBlackjack = false;

        return isBlackjack;
    }

    private void BlackJack()
    {
        PlayerWins( (float)2.5 );
    }
    private void DetermineWinner()
    {
        if ( Player.HandValue() > 21 )
            PlayerLoses();
        else if ( Dealer.HandValue() > 21 )
        {
            PlayerWins();
        }
        else
        {
            if ( Player.HandValue() == Dealer.HandValue() )
                Push();
            else if ( Player.HandValue() > Dealer.HandValue() )
                PlayerWins();
            else
                PlayerLoses();
        }
        WinningPct.text = ((float)handsWon / (float)handsPlayed).ToString();

    }

    private void PlayerLoses()
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

    private void PlayerWins( float payoff = (float)2.0 )
    {
        Bank += Bet * payoff;
        handsWon++;
        GameResult.color = Color.green;
        GameResult.text = "Win!";
    }
    #endregion
}
