using System.Collections;

using UnityEngine;

public class FlipCard : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private CardModel card;

    public AnimationCurve ScaleCurve;
    public float FlipDuration = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        card = GetComponent<CardModel>();
    }

    public void InitiateFlip( Sprite startImage, Sprite endImage, int cardIdx )
    {
        StopCoroutine( Flip( startImage, endImage, cardIdx ) );
        StartCoroutine( Flip( startImage, endImage, cardIdx ) );
    }

    IEnumerator Flip( Sprite startImage, Sprite endImage, int cardIdx )
    {
        spriteRenderer.sprite = startImage;

        float time = 0f;
        while ( time <= 1f )
        {
            float scale = ScaleCurve.Evaluate( time );
            time = time + Time.deltaTime / FlipDuration;

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;

            if ( time >= 0.5f )
                spriteRenderer.sprite = endImage;

            yield return new WaitForFixedUpdate();
        }
        if ( cardIdx == -1 )
            card.ToggleFace( false );
        else
        {
            card.cardIdx = cardIdx;
            card.ToggleFace( true );
        }
    }
}
