using System.Collections.Generic;

using UnityEngine;

[RequireComponent( typeof( Deck ) )]
public class DeckView : MonoBehaviour
{
    private Deck deck;
    private Dictionary<int, CardView> fetchedCards;
    private int lastCount;

    public Vector3 StartPosition;
    public GameObject cardPrefab;
    public float cardOffset;
    public bool faceUp = false;
    public bool reverseLayerOrder = false;

    private void Start()
    {
        fetchedCards = new Dictionary<int, CardView>();
        deck = GetComponent<Deck>();
        ShowHand();
        lastCount = deck.CardCount;

        deck.AddCard += deck_AddCard;
        deck.RemoveCard += deck_RemoveCard;
    }


    private void deck_RemoveCard( object sender, CardEventArgs e )
    {
        if ( fetchedCards == null )
            return;

        if ( fetchedCards.ContainsKey( e.CardIdx ) )
        {
            Destroy( fetchedCards[ e.CardIdx ].Card );
            fetchedCards.Remove( e.CardIdx );
        }
    }

    private void deck_AddCard( object sender, CardEventArgs e )
    {
        Vector3 offsetPosition = StartPosition + new Vector3( cardOffset * deck.CardCount, 0f );
        AddCard( offsetPosition, e.CardIdx, deck.CardCount );
    }

    public void ClearHand()
    {
        deck.Reset();
        foreach ( CardView card in fetchedCards.Values )
        {
            Destroy( card.Card );
        }

        fetchedCards.Clear();
    }

    public void Toggle( int cardIdx, bool isFaceUp )
    {
        if ( fetchedCards == null )
            return;
        fetchedCards[ cardIdx ].IsFaceUp = isFaceUp;
    }


    private void Update()
    {
        if ( lastCount != deck.CardCount )
        {
            lastCount = deck.CardCount;
            ShowHand();
        }
    }

    public void ShowHand()
    {
        int cardCount = 0;
        if ( deck == null )
            return;

        if ( deck.HasCards )
        {
            foreach ( int cardIdx in deck.GetCards() )
            {

                Vector3 offsetPosition = StartPosition + new Vector3( cardOffset * cardCount, 0f );
                AddCard( offsetPosition, cardIdx, cardCount );
                cardCount++;
            }
        }
    }

    private void AddCard( Vector3 offsetPosition, int cardIdx, int positionalIndex )
    {
        if ( fetchedCards.ContainsKey( cardIdx ) )
        {
            if ( !faceUp )
            {
                CardModel model = fetchedCards[ cardIdx ].Card.GetComponent<CardModel>();
                //if ( positionalIndex == 0 )
                //    fetchedCards[ cardIdx ].IsFaceUp = true;
                model.ToggleFace( fetchedCards[ cardIdx ].IsFaceUp );

            }
            return;
        }
        GameObject card = (GameObject)Instantiate( cardPrefab );

        card.transform.position = offsetPosition;

        CardModel cardModel = card.GetComponent<CardModel>();
        cardModel.cardIdx = cardIdx;

        cardModel.ToggleFace( faceUp );


        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        if ( reverseLayerOrder )
        {
            spriteRenderer.sortingOrder = 51 - positionalIndex;
        }
        else
        {
            spriteRenderer.sortingOrder = positionalIndex;
        }

        AddFetched( cardIdx, new CardView( card ) );



    }

    private void AddFetched( int cardIdx, CardView cardView )
    {
        fetchedCards.Add( cardIdx, cardView );
    }
}

