using UnityEngine;

public class DebugChangeCard : MonoBehaviour
{
    public GameObject Card;

    private FlipCard flipCard;
    private CardModel cardModel;
    private int cardIdx = 0;
    private Deck deck;
    // Use this for initialization
    void Awake()
    {
        cardModel = Card.GetComponent<CardModel>();
        flipCard = Card.GetComponent<FlipCard>();
        deck = GetComponent<Deck>();
    }

    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 10, 10, 100, 28 ), "Hit me!" ) )
        {

            //if ( cardIdx >= deck.deck.Count )
            if ( cardIdx >= cardModel.CardImages.Length )
            {
                cardIdx = 0;
                flipCard
                    .InitiateFlip(
                        cardModel.CardImages[
                            cardModel.CardImages.Length - 1 ],
                        cardModel.CardBack, -1
                    );

            }
            else
            {
                if ( cardIdx > 0 )
                {
                    flipCard.InitiateFlip(
                        cardModel.CardImages[ cardIdx - 1 ],
                        cardModel.CardImages[ cardIdx ],
                        cardIdx );
                }
                else
                {
                    flipCard.InitiateFlip(
                        cardModel.CardBack,
                        cardModel.CardImages[ cardIdx ],
                        cardIdx );
                }

                cardIdx++;
            }

        }
    }



}
