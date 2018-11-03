using UnityEngine;

public class CardModel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] CardImages;
    public Sprite CardBack;
    public int cardIdx;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void ToggleFace( bool showFace )
    {
        spriteRenderer.sprite = showFace
            ? CardImages[ cardIdx ]
            : CardBack;
    }


}
