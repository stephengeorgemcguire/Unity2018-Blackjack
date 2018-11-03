using UnityEngine;

public class DebugDealer : MonoBehaviour
{


    public Deck dealer;
    public Deck player;

    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 10, 10, 256, 28 ), "Hit" ) )
        {
            player.Push( dealer.Pop() );
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
