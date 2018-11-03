using System;

public class CardEventArgs : EventArgs
{
    public int CardIdx { get; private set; }

    public CardEventArgs( int cardIdx )
    {
        CardIdx = cardIdx;
    }
}