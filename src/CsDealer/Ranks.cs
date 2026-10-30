using System;
using System.Collections.Generic;

namespace CsDealer
{
    /// <summary>
    /// A ranking of card values (and optionally suits) used for sorting and comparing cards.
    /// Higher numbers rank higher. Cards are compared by value first, then by suit if suits are ranked.
    /// </summary>
    /// <remarks>Instances are immutable and thread-safe. Use one as an <see cref="IComparer{T}"/> with LINQ or <c>List.Sort</c>.</remarks>
    public sealed class Ranks : IComparer<Card>, IEqualityComparer<Card>
    {
        private readonly int[] _values = new int[15];
        private readonly int[]? _suits;

        /// <summary>Creates a ranking. Values or suits that are not listed rank as 0.</summary>
        /// <param name="values">Rank of each card value.</param>
        /// <param name="suits">Rank of each suit, or <see langword="null"/> to ignore suits.</param>
        public Ranks(IReadOnlyDictionary<CardValue, int> values, IReadOnlyDictionary<Suit, int>? suits = null)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (KeyValuePair<CardValue, int> pair in values)
            {
                if ((uint)pair.Key >= (uint)_values.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(values), pair.Key, "Undefined card value.");
                }

                _values[(int)pair.Key] = pair.Value;
            }

            if (suits is not null)
            {
                _suits = new int[4];

                foreach (KeyValuePair<Suit, int> pair in suits)
                {
                    if ((uint)pair.Key > 3)
                    {
                        throw new ArgumentOutOfRangeException(nameof(suits), pair.Key, "Undefined suit.");
                    }

                    _suits[(int)pair.Key] = pair.Value;
                }
            }
        }

        private static readonly Dictionary<CardValue, int> PokerValues = new()
        {
            [CardValue.Ace] = 13,
            [CardValue.King] = 12,
            [CardValue.Queen] = 11,
            [CardValue.Jack] = 10,
            [CardValue.Ten] = 9,
            [CardValue.Nine] = 8,
            [CardValue.Eight] = 7,
            [CardValue.Seven] = 6,
            [CardValue.Six] = 5,
            [CardValue.Five] = 4,
            [CardValue.Four] = 3,
            [CardValue.Three] = 2,
            [CardValue.Two] = 1,
            [CardValue.Joker] = 0,
        };

        private static readonly Dictionary<CardValue, int> Big2Values = new()
        {
            [CardValue.Two] = 13,
            [CardValue.Ace] = 12,
            [CardValue.King] = 11,
            [CardValue.Queen] = 10,
            [CardValue.Jack] = 9,
            [CardValue.Ten] = 8,
            [CardValue.Nine] = 7,
            [CardValue.Eight] = 6,
            [CardValue.Seven] = 5,
            [CardValue.Six] = 4,
            [CardValue.Five] = 3,
            [CardValue.Four] = 2,
            [CardValue.Three] = 1,
            [CardValue.Joker] = 0,
        };

        private static readonly Dictionary<Suit, int> Big2Suits = new()
        {
            [Suit.Spades] = 4,
            [Suit.Hearts] = 3,
            [Suit.Clubs] = 2,
            [Suit.Diamonds] = 1,
        };

        /// <summary>Poker ranking: Ace high, suits ignored.</summary>
        public static Ranks Poker { get; } = new(PokerValues);

        /// <summary>Big Two ranking: 2 high, then Ace; Spades &gt; Hearts &gt; Clubs &gt; Diamonds.</summary>
        public static Ranks Big2 { get; } = new(Big2Values, Big2Suits);

        /// <summary>The default ranking: poker values, with Big Two suit order breaking ties.</summary>
        public static Ranks Default { get; } = new(PokerValues, Big2Suits);

        /// <summary>Whether this ranking orders suits.</summary>
        public bool RanksSuits => _suits is not null;

        /// <summary>The rank of a value.</summary>
        public int ValueRank(CardValue value) => _values[(int)value];

        /// <summary>The rank of a suit, or 0 when suits are not ranked or <paramref name="suit"/> is <see langword="null"/>.</summary>
        public int SuitRank(Suit? suit) => _suits is null || suit is null ? 0 : _suits[(int)suit.Value];

        /// <inheritdoc/>
        public int Compare(Card x, Card y)
        {
            int result = _values[(int)x.Value].CompareTo(_values[(int)y.Value]);

            if (result != 0 || _suits is null)
            {
                return result;
            }

            return SuitRank(x.Suit).CompareTo(SuitRank(y.Suit));
        }

        /// <summary>Whether the two cards rank equally.</summary>
        public bool Equals(Card x, Card y) => Compare(x, y) == 0;

        /// <inheritdoc/>
        public int GetHashCode(Card card) => _values[(int)card.Value] * 31 + SuitRank(card.Suit);
    }
}
