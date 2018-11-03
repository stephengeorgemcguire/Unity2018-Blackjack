using Assets.Scripts;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

//[RequireComponent( typeof( Card ) )]
public class Hand : MonoBehaviour
{

    //public event EventHandler Changed;
    public bool IsDealer { get; private set; }

    //private readonly List<PlayingCard> cards = new List<PlayingCard>();
    private readonly List<CardModel> cards = new List<CardModel>();

    public Hand( bool isDealer = false )
    {
        this.IsDealer = isDealer;
        //AddHandToPlayer();
    }

    public ReadOnlyCollection<CardModel> Cards
    {
        get { return this.cards.AsReadOnly(); }
    }

    //public int SoftValue
    //{
    //    get { return (GetHandValue()); }
    //}

    //public bool IsBlackjack()
    //{
    //    return (this.Value == GameParameters.Blackjack);
    //}
    // Return the number of Aces in the hand 
    //private int GetAcesCount()
    //{
    //    return (this.cards.Count( c => c.CardRank == Card.Rank.Ace ));
    //}

    // The hand being played.
    //public int Value
    //{
    //    get
    //    {
    //        // If the hard value of the hand is a bust, play with the Soft hand
    //        return (IsBustHand( HardValue ) ? SoftValue : HardValue);
    //    }
    //}

    //public int HardValue
    //{
    //    get
    //    {
    //        // If there is at least one Ace, add 10 to the hard.
    //        return (this.SoftValue + (GetAcesCount() >= 1 ? 10 : 0));
    //    }
    //}

    // Return the sum of the cards in the hand
    //private int GetHandValue()
    //{
    //    return (this.cards.Select( c => (int)c.CardRank ).Sum());
    //}

    public void ShowHand()
    {
        this.cards[ 0 ].ToString();
        this.cards[ 1 ].ToString();
    }
    public bool IsBustHand( int handValue )
    {
        return (handValue > GameParameters.Blackjack);
    }

    //public void Hit( Deck deck )
    //{
    //    this.cards.Add( deck.DealCard() );
    //    if ( Changed != null )
    //    {
    //        Changed( this, EventArgs.Empty );
    //    }
    //}
    //The hand takes a card
    //public void TakeCard( Card card )
    //{
    //    // 'cards' represent the cards held in 'this' hand
    //    this.cards.Add( card );

    //    //    if ( Changed != null )
    //    //    {
    //    //        Changed( this, EventArgs.Empty );
    //    //    }
    //}

    // Clear the hand of all cards
    public void Clear()
    {
        this.cards.Clear();
    }

    //public PlayingCard Split()
    //{
    //    PlayingCard splitCard = this.cards.Last();
    //    this.cards.Remove( splitCard );

    //    return (splitCard);
    //}

    //public bool CanSplit()
    //{
    //    return (cards.Last().CardRank == cards.First().CardRank);
    //}
}





