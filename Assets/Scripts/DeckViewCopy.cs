//using System.Collections.Generic;

//using UnityEngine;

//[RequireComponent( typeof( Deck ) )]
//public class DeckView : MonoBehaviour
//{
//    private CardStack cardStack;
//    private Dictionary<int, CardView> fetchedCards;
//    private int lastCount;

//    public Vector3 start;
//    public GameObject cardPrefab;
//    public float cardOffset;
//    public bool faceUp = false;
//    public bool reverseLayerOrder = false;

//    private void Start()
//    {
//        fetchedCards = new Dictionary<int, CardView>();
//        cardStack = GetComponent<CardStack>();
//        ShowCards();
//        lastCount = cardStack.CardCount;

//        cardStack.CardRemoved += CardStack_CardRemoved;
//    }

//    public void Toggle( int card, bool isFaceUp )
//    {
//        fetchedCards[ card ].IsFaceUp = isFaceUp;
//    }


//    private void CardStack_CardRemoved( object sender, CardRemovedEventArgs e )
//    {
//        if ( fetchedCards.ContainsKey( e.cardIdx ) )
//        {
//            Destroy( fetchedCards[ e.cardIdx ].Card );
//            fetchedCards.Remove( e.cardIdx );
//        }
//    }

//    private void Update()
//    {
//        if ( lastCount != cardStack.CardCount )
//        {
//            lastCount = cardStack.CardCount;
//            ShowDeck();
//        }
//    }

//    void ShowDeck()
//    {
//        int cardCount = 0;

//        if ( cardStack.HasCards )
//        {
//            foreach ( int i in cardStack.GetCards() )
//            {
//                float co = cardOffset * cardCount;

//                Vector3 temp = start + new Vector3( co, 0f );
//                AddCard( temp, i, cardCount );
//                cardCount++;
//            }
//        }
//    }
//    private void AddCard( Vector3 position, int cardIdx, int positionalIndex )
//    {
//        if ( fetchedCards.ContainsKey( cardIdx ) )
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
