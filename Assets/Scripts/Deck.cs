using System.Collections.Generic;

using UnityEngine;

public class Deck : MonoBehaviour
{

    public event CardEventHandler RemoveCard;
    public event CardEventHandler AddCard;

    public List<int> deck;

    public bool IsGameDeck;

    public void Reset()
    {
        InitializeDeck();
    }

    // Use this for initialization
    private void Start()
    {
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        deck = new List<int>();
        if ( IsGameDeck )
        {
            CreateDeck();
            ShuffleDeck();
        }
    }

    public bool HasCards
    {
        get { return deck != null && deck.Count > 0; }
    }

    public IEnumerable<int> GetCards()
    {
        foreach ( int cardIdx in deck )
        {
            yield return cardIdx;
        }
    }

    public int CardCount
    {
        get { return deck == null ? 0 : deck.Count; }
    }


    public void CreateDeck()
    {
        deck.Clear();

        for ( int i = 0 ; i < 52 ; i++ )
        {
            deck.Add( i );
        }
    }

    private void ShuffleDeck()
    {
        for ( int i = 51 ; i > 0 ; i-- )
        {
            int k = UnityEngine.Random.Range( 0, i + 1 );
            int temp = deck[ k ];
            deck[ k ] = deck[ i ];
            deck[ i ] = temp;
        }
    }

    public int GetCardRank( int cardIdx )
    {
        return cardIdx % 13;
    }

    public int HandValue()
    {
        int total = 0;
        int aces = 0;

        foreach ( int cardIdx in GetCards() )
        {
            int cardRank = GetCardRank( cardIdx );    // 13 cards in each suit

            if ( cardRank == 0 ) // Ace == 0
                aces++;
            if ( cardRank < 10 ) // Ace - 9
                cardRank += 1;
            else // 10 - King
                cardRank = 10;

            total += cardRank;
        }

        // We have already added one to each Ace in the hand
        for ( int i = 0 ; i < aces ; i++ )
        {
            if ( total + 10 <= 21 )
                total += 10;    // Adding ten yields a value of eleven for an ace
        }
        return total;
    }
    public int Pop()
    {
        int temp = deck[ 0 ];
        deck.RemoveAt( 0 );

        RemoveCard?.Invoke( this, new CardEventArgs( temp ) );
        return temp;

    }

    public void Push( int cardIdx )
    {
        deck.Add( cardIdx );
        AddCard?.Invoke( this, new CardEventArgs( cardIdx ) );

    }



    // Number of cards remaining in deck.
    //public int Count
    //{
    //    get { return deck == null ? 0 : deck.Count; }
    //}

}

