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
    public Text WagerResult;
    public Text CurrentWager;
    public Text CurrentBank;
    public Text WinningPct;

    private float BeginningPurse;
    private float Purse;
    public float Wager;
    public float InitialBank;
    private float HandResult;

    private int handsPlayed;
    private int handsWon;


    private void Start()
    {
        DealHand();
    }

    #region Player Actions

    public void HitPlayer()
    {
        Player.Push( Deck.Pop() );
        PlayerScore.text = Player.HandValue().ToString();
        if ( Player.HandValue() > 21 )
        {
            TogglePlayButtons( false );
            DetermineWinner();
        }
    }

    public void DoubleDown()
    {
        TogglePlayButtons( false );
        HitPlayer();    // Player gets one card
    }

    public void Hold()
    {
        TogglePlayButtons( false );
        StartCoroutine( DealerPlay() );
    }

    public void Split()
    {
        // TODO:
    }

    public void PlayAgain()
    {
        ResetResults();
        ClearHands();

        TogglePlayButtons( true );

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
        WagerResult.text = "";
    }

    #endregion
    private void DealHand()
    {
        handsPlayed++;
        Purse -= Wager;
        for ( int i = 0 ; i < 2 ; i++ )
        {
            HitPlayer();
            HitDealer();
        }
        if ( IsBlackjack() )
            TogglePlayButtons( false );
    }

    private void ToggleView( int cardIdx )
    {
        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.Toggle( cardIdx, true );
    }

    private void TogglePlayButtons( bool isInteractable )
    {
        HitButton.interactable = isInteractable;
        HoldButton.interactable = isInteractable;
        DoubleDownButton.interactable = isInteractable;
        SplitButton.interactable = isInteractable;
        PlayAgainButton.interactable = !isInteractable;
    }


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
        //ToggleView( Dealer.deck. );  // Show the dealers first card

        DeckView cardView = Dealer.GetComponent<DeckView>();
        cardView.ShowHand();
        while ( Dealer.HandValue() < 17 )
        {

            HitDealer();
            DealerScore.text = Dealer.HandValue().ToString();
            yield return new WaitForSeconds( 1f );
        }
        DetermineWinner();
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
        HandResult = Wager * (float)2.5;
    }
    private void DetermineWinner()
    {
        if ( Player.HandValue() > 21 )
            PlayerLoses();
        else if ( Dealer.HandValue() > 21 )
            PlayerWins();
        else
        {
            if ( Player.HandValue() == Dealer.HandValue() )
                Push();
            else if ( Player.HandValue() > Dealer.HandValue() )
                PlayerWins();
            else
                PlayerLoses();
        }

    }

    private void PlayerLoses()
    {
        GameResult.color = Color.red;
        GameResult.text = "Lose";
    }

    private void Push()
    {
        Purse += Wager;
        GameResult.color = Color.white;
        GameResult.text = "Push";

    }

    private void PlayerWins()
    {
        Purse += Wager * 2;
        GameResult.color = Color.green;
        GameResult.text = "Win!";
    }
    #endregion
}
