
//using System.Collections.Generic;

//using UnityEngine;

//[RequireComponent( typeof( Deck ) )]
//public class DeckView : MonoBehaviour
//{
//    private Deck deck;

//    public Vector3 start;
//    public GameObject cardPrefab;
//    private Dictionary<int, CardView> fetchedCards;
//    public float cardOffset;
//    public bool faceUp = false;
//    public bool reverseLayerOrder = false;
//    private int lastCount;

//    private void Start()
//    {
//        deck = GetComponent<Deck>();
//        ShowDeck();

//        deck.CardRemoved += Deck_CardRemoved;
//    }

//    public void Toggle( int card, bool isFaceUp )
//    {
//        fetchedCards[ card ].IsFaceUp = isFaceUp;
//    }


//    private void Deck_CardRemoved( object sender, CardRemovedEventArgs e )
//    {
//        if ( fetchedCards.ContainsKey( e.cardIdx ) )
//        {
//            Destroy( fetchedCards[ e.cardIdx ].Card );
//            fetchedCards.Remove( e.cardIdx );
//        }
//    }

//    private void Update()
//    {
//        if ( lastCount != deck.Count )
//        {
//            lastCount = deck.Count;
//            ShowDeck();
//        }
//    }

//    void ShowDeck()
//    {
//        int cardCount = 0;

//        if ( deck.HasCards )
//        {
//            foreach ( PlayingCard card in deck.GetCards() )
//            {
//                float co = cardOffset * cardCount;

//                Vector3 temp = start + new Vector3( co, 0f );
//                AddCard( temp, card, cardCount );
//                cardCount++;
//            }
//        }
//    }
//    private void AddCard( Vector3 position, PlayingCard card, int positionalIndex )
//    {
//        if ( fetchedCards.ContainsKey( card ) )
//        {
//            if ( !faceUp )
//            {
//                CardModel model = fetchedCards[ cardIdx ].Card.GetComponent<CardModel>();
//                model.ToggleFace( fetchedCards[ cardIdx ].IsFaceUp );
//            }
//            return;
//        }
//        GameObject cardCopy = (GameObject)Instantiate( cardPrefab );

//        cardCopy.transform.position = position;

//        CardModel cardModel = cardCopy.GetComponent<CardModel>();
//        cardModel.cardIdx = cardIdx;

//        cardModel.ToggleFace( faceUp );


//        SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
//        if ( reverseLayerOrder )
//        {
//            spriteRenderer.sortingOrder = 51 - positionalIndex;
//        }
//        else
//        {
//            spriteRenderer.sortingOrder = positionalIndex;
//        }

//        fetchedCards.Add( cardIdx, new CardView( cardCopy ) );

//        Debug.Log( "Hand Value = " + cardStack.HandValue() );

//    }
//}
