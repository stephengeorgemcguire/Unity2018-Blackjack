namespace Assets.Scripts
{
    public static class GameParameters
    {
        public const int DealerStandLimit = 17;  // Dealer must stand on 17 or greater
        public const int Blackjack = 21;
        public const int DeckLowCardCount = 12; // Create new deck when cards remaining get below
        public const int InitialNumberCardsInHand = 2; //Each player gets two cards in initial deal
    }
}
