using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{

    public int WinCount { get; set; }
    public List<Hand> Hands { get; set; }
    //public Hand Hand { get; protected set; }
    public Hand CurrentHand { get; set; }
    //public List<Hand> Hands = new List<Hand>();
    //private List<Hand> hands = new List<Hand>();
    //public Hand Hand { get; protected set; }
    //private Hand Hand { get; set; }
    public Player()
    {
        this.WinCount = 0;
        this.Hands = new List<Hand>();
        CurrentHand = GetHand();

    }
    private Hand GetHand( bool isDealer = false )
    {
        Hand hand = new Hand( isDealer );
        if ( this.Hands != null )
            this.Hands.Add( hand );

        return (hand);
    }
    //public void Hit(Dealer dealer)
    //{   
    //    dealer.DealCard( Hand );
    //    this.deck.DealCard( this.Player.Hand );

    //    if ( this.Player.Hand.IsBustHand( this.Player.Hand.Value ) )
    //    {
    //        this.LastGameResults = GameResults.DealerWon;
    //        this.AllowedActions = Action.Deal;
    //    }
    //}



    //public void Stand()
    //{
    //    if ( !IsActionAllowed( Action.Stand ) )
    //    {
    //        throw new InvalidOperationException();
    //    }
    //    DealerPlay();   //Player stands.  Dealer will draw to (soft) seventeen
    //    DetermineWinner();

    //    this.AllowedActions = Action.Deal;
    //}

}




