namespace CsDealer
{
    /// <summary>The four suits of a standard deck.</summary>
    public enum Suit : byte
    {
        /// <summary>Diamonds.</summary>
        Diamonds = 0,
        /// <summary>Clubs.</summary>
        Clubs = 1,
        /// <summary>Hearts.</summary>
        Hearts = 2,
        /// <summary>Spades.</summary>
        Spades = 3,
    }

    /// <summary>
    /// The value (face) of a card. Numeric cards use their pip count, so <c>(int)CardValue.Seven == 7</c>.
    /// </summary>
    public enum CardValue : byte
    {
        /// <summary>A joker. Jokers have no suit.</summary>
        Joker = 0,
        /// <summary>2.</summary>
        Two = 2,
        /// <summary>3.</summary>
        Three = 3,
        /// <summary>4.</summary>
        Four = 4,
        /// <summary>5.</summary>
        Five = 5,
        /// <summary>6.</summary>
        Six = 6,
        /// <summary>7.</summary>
        Seven = 7,
        /// <summary>8.</summary>
        Eight = 8,
        /// <summary>9.</summary>
        Nine = 9,
        /// <summary>10.</summary>
        Ten = 10,
        /// <summary>Jack.</summary>
        Jack = 11,
        /// <summary>Queen.</summary>
        Queen = 12,
        /// <summary>King.</summary>
        King = 13,
        /// <summary>Ace.</summary>
        Ace = 14,
    }

    /// <summary>
    /// An end of a <see cref="Stack"/>. The top is the end of the underlying list (the highest index).
    /// </summary>
    public enum StackEnd
    {
        /// <summary>The top of the stack (last index).</summary>
        Top,
        /// <summary>The bottom of the stack (index 0).</summary>
        Bottom,
    }
}
