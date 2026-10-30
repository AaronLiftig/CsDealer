using System;
using System.Diagnostics;

namespace CsDealer
{
    /// <summary>
    /// An immutable playing card. <c>default(Card)</c> is the joker.
    /// </summary>
    /// <remarks>
    /// Equality is structural (same value and suit). The comparison operators use <see cref="Ranks.Default"/>;
    /// pass a <see cref="Ranks"/> instance to <see cref="CompareTo(Card, Ranks)"/> for other games.
    /// </remarks>
    [DebuggerDisplay("{Name,nq}")]
    public readonly struct Card : IEquatable<Card>, IComparable<Card>, IComparable
    {
        // 0 means "no suit" (joker); otherwise (int)Suit + 1. This keeps default(Card) a valid joker.
        private readonly byte _value;
        private readonly byte _suit;

        internal const int KeyCount = 15 * 5;

        private static readonly string[] ValueNames =
            { "Joker", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        private static readonly string[] ValueAbbrevs =
            { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        // Spelled-out names accepted by Parse ("Two" … "Ace"); kept as strings so parsing never calls Enum.ToString.
        private static readonly string[] ValueWords =
            { "", "", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };

        private static readonly string[] SuitNames = { "Diamonds", "Clubs", "Hearts", "Spades" };

        // Names and abbreviations are precomputed so the properties never allocate.
        private static readonly string[] Names = BuildTable(abbrev: false);
        private static readonly string[] Abbrevs = BuildTable(abbrev: true);

        private static string[] BuildTable(bool abbrev)
        {
            var table = new string[KeyCount];
            table[0] = abbrev ? "JKR" : "Joker";

            for (int v = 2; v <= 14; v++)
            {
                for (int s = 0; s < 4; s++)
                {
                    table[v * 5 + s + 1] = abbrev
                        ? ValueAbbrevs[v] + SuitNames[s][0]
                        : $"{ValueNames[v]} of {SuitNames[s]}";
                }
            }

            return table;
        }

        /// <summary>Creates a suited card.</summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is <see cref="CardValue.Joker"/> or either argument is undefined.</exception>
        public Card(CardValue value, Suit suit)
        {
            if (value < CardValue.Two || value > CardValue.Ace)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    "Value must be Two through Ace. Use Card.Joker for jokers.");
            }

            if ((uint)suit > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(suit), suit, "Undefined suit.");
            }

            _value = (byte)value;
            _suit = (byte)(suit + 1);
        }

        /// <summary>The joker.</summary>
        public static Card Joker => default;

        /// <summary>The card's value.</summary>
        public CardValue Value => (CardValue)_value;

        /// <summary>The card's suit, or <see langword="null"/> for a joker.</summary>
        public Suit? Suit => _suit == 0 ? null : (CsDealer.Suit)(_suit - 1);

        /// <summary>Whether this card is a joker.</summary>
        public bool IsJoker => _value == 0;

        /// <summary>Whether this card is a heart or a diamond.</summary>
        public bool IsRed => _suit == 1 || _suit == 3;

        /// <summary>The full name, e.g. <c>"Ace of Spades"</c> or <c>"Joker"</c>.</summary>
        public string Name => Names[Key];

        /// <summary>The abbreviation, e.g. <c>"AS"</c>, <c>"10H"</c> or <c>"JKR"</c>.</summary>
        public string Abbrev => Abbrevs[Key];

        /// <summary>The value's display name, e.g. <c>"Ace"</c> or <c>"10"</c>.</summary>
        public string ValueName => ValueNames[_value];

        /// <summary>The suit's display name, or <see langword="null"/> for a joker.</summary>
        public string? SuitName => _suit == 0 ? null : SuitNames[_suit - 1];

        /// <summary>A dense index in <c>[0, 75)</c> that is unique per card. Used for lookup tables.</summary>
        internal int Key => _value * 5 + _suit;

        internal string ValueAbbrev => ValueAbbrevs[_value];

        /// <summary>
        /// Whether <paramref name="term"/> (case-insensitive) is this card's name, abbreviation, value or suit.
        /// </summary>
        public bool Matches(string term)
        {
            if (term is null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            return string.Equals(term, Name, StringComparison.OrdinalIgnoreCase)
                || string.Equals(term, Abbrev, StringComparison.OrdinalIgnoreCase)
                || string.Equals(term, ValueName, StringComparison.OrdinalIgnoreCase)
                || (_suit != 0 && string.Equals(term, SuitNames[_suit - 1], StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Renders this card as an SVG image. See <see cref="CardSvg"/>.</summary>
        public string ToSvg(CardSvgOptions? options = null) => CardSvg.Render(this, options);

        //===============================================================================
        // Comparison
        //===============================================================================

        /// <inheritdoc/>
        public bool Equals(Card other) => _value == other._value && _suit == other._suit;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Card other && Equals(other);

        /// <summary>Whether the two cards rank equally under <paramref name="ranks"/>.</summary>
        public bool Equals(Card other, Ranks ranks) => NotNull(ranks).Compare(this, other) == 0;

        /// <inheritdoc/>
        public override int GetHashCode() => Key;

        /// <summary>Compares using <see cref="Ranks.Default"/>.</summary>
        public int CompareTo(Card other) => Ranks.Default.Compare(this, other);

        /// <summary>Compares using the given ranks.</summary>
        public int CompareTo(Card other, Ranks ranks) => NotNull(ranks).Compare(this, other);

        private static Ranks NotNull(Ranks ranks) => ranks ?? throw new ArgumentNullException(nameof(ranks));

        int IComparable.CompareTo(object? obj) => obj switch
        {
            null => 1,
            Card other => CompareTo(other),
            _ => throw new ArgumentException("Object must be a Card.", nameof(obj)),
        };

        /// <summary>Structural equality.</summary>
        public static bool operator ==(Card left, Card right) => left.Equals(right);

        /// <summary>Structural inequality.</summary>
        public static bool operator !=(Card left, Card right) => !left.Equals(right);

        /// <summary>Compares using <see cref="Ranks.Default"/>.</summary>
        public static bool operator <(Card left, Card right) => left.CompareTo(right) < 0;

        /// <summary>Compares using <see cref="Ranks.Default"/>.</summary>
        public static bool operator >(Card left, Card right) => left.CompareTo(right) > 0;

        /// <summary>Compares using <see cref="Ranks.Default"/>.</summary>
        public static bool operator <=(Card left, Card right) => left.CompareTo(right) <= 0;

        /// <summary>Compares using <see cref="Ranks.Default"/>.</summary>
        public static bool operator >=(Card left, Card right) => left.CompareTo(right) >= 0;

        //===============================================================================
        // Formatting and parsing
        //===============================================================================

        /// <summary>Returns <see cref="Name"/>.</summary>
        public override string ToString() => Name;

        /// <summary>
        /// Parses a card from its name (<c>"Ace of Spades"</c>), abbreviation (<c>"AS"</c>, <c>"10h"</c>, <c>"TH"</c>),
        /// a value and suit separated by whitespace (<c>"Ace Spades"</c>) or <c>"Joker"</c>/<c>"JKR"</c>. Case-insensitive.
        /// </summary>
        /// <exception cref="FormatException">The text is not a card.</exception>
        public static Card Parse(string text) =>
            TryParse(text, out Card card) ? card : throw new FormatException($"'{text}' is not a valid card.");

        /// <summary>Parses a card from separate value and suit strings, e.g. <c>("Queen", "hearts")</c>.</summary>
        /// <exception cref="FormatException">The value or suit is not valid.</exception>
        public static Card Parse(string value, string? suit) =>
            TryParse(value, suit, out Card card) ? card : throw new FormatException($"'{value}', '{suit}' is not a valid card.");

        /// <summary>Tries to parse a card. See <see cref="Parse(string)"/> for the accepted formats.</summary>
        public static bool TryParse(string? text, out Card card)
        {
            card = default;

            if (text is null)
            {
                return false;
            }

            string s = text.Trim();

            if (s.Length == 0)
            {
                return false;
            }

            if (IsJokerText(s))
            {
                return true;
            }

            int of = s.IndexOf(" of ", StringComparison.OrdinalIgnoreCase);

            if (of > 0)
            {
                return TryParse(s.Substring(0, of), s.Substring(of + 4), out card);
            }

            int space = s.IndexOfAny(new[] { ' ', '\t' });

            if (space > 0)
            {
                return TryParse(s.Substring(0, space), s.Substring(space + 1), out card);
            }

            // Abbreviation: value followed by a one-letter suit.
            return s.Length >= 2 && TryParse(s.Substring(0, s.Length - 1), s.Substring(s.Length - 1), out card);
        }

        /// <summary>Tries to parse a card from separate value and suit strings.</summary>
        public static bool TryParse(string? value, string? suit, out Card card)
        {
            card = default;

            if (value is null)
            {
                return false;
            }

            value = value.Trim();

            if (IsJokerText(value))
            {
                return string.IsNullOrWhiteSpace(suit);
            }

            if (!TryParseValue(value, out CardValue v) || !TryParseSuit(suit, out Suit st))
            {
                return false;
            }

            card = new Card(v, st);
            return true;
        }

        private static bool IsJokerText(string s) =>
            s.Equals("Joker", StringComparison.OrdinalIgnoreCase) || s.Equals("JKR", StringComparison.OrdinalIgnoreCase);

        private static bool TryParseValue(string s, out CardValue value)
        {
            for (int v = 2; v <= 14; v++)
            {
                if (s.Equals(ValueNames[v], StringComparison.OrdinalIgnoreCase)
                    || s.Equals(ValueAbbrevs[v], StringComparison.OrdinalIgnoreCase)
                    || s.Equals(ValueWords[v], StringComparison.OrdinalIgnoreCase))
                {
                    value = (CardValue)v;
                    return true;
                }
            }

            if (s.Equals("T", StringComparison.OrdinalIgnoreCase))
            {
                value = CardValue.Ten;
                return true;
            }

            value = default;
            return false;
        }

        private static bool TryParseSuit(string? s, out Suit suit)
        {
            suit = default;

            if (s is null)
            {
                return false;
            }

            s = s.Trim();

            for (int i = 0; i < 4; i++)
            {
                string name = SuitNames[i];

                if (s.Equals(name, StringComparison.OrdinalIgnoreCase)
                    || (s.Length == 1 && char.ToUpperInvariant(s[0]) == name[0]))
                {
                    suit = (Suit)i;
                    return true;
                }
            }

            return false;
        }
    }
}
