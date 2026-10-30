using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsDealer
{
    /// <summary>Utility functions that work on any sequence of cards.</summary>
    public static class CardTools
    {
        /// <summary>The four suits, in build order.</summary>
        public static IReadOnlyList<Suit> Suits { get; } = new[] { Suit.Diamonds, Suit.Clubs, Suit.Hearts, Suit.Spades };

        /// <summary>The thirteen non-joker values, in build order (Two through Ace).</summary>
        public static IReadOnlyList<CardValue> Values { get; } = new[]
        {
            CardValue.Two, CardValue.Three, CardValue.Four, CardValue.Five, CardValue.Six, CardValue.Seven,
            CardValue.Eight, CardValue.Nine, CardValue.Ten, CardValue.Jack, CardValue.Queen, CardValue.King, CardValue.Ace,
        };

        /// <summary>
        /// Builds a fresh, ordered 52-card deck plus <paramref name="jokers"/> jokers at the bottom.
        /// The Ace of Spades is on top (last).
        /// </summary>
        public static List<Card> BuildCards(int jokers = 0)
        {
            if (jokers < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(jokers), jokers, "Must be zero or more.");
            }

            var cards = new List<Card>(52 + jokers);

            for (int i = 0; i < jokers; i++)
            {
                cards.Add(Card.Joker);
            }

            foreach (CardValue value in Values)
            {
                foreach (Suit suit in Suits)
                {
                    cards.Add(new Card(value, suit));
                }
            }

            return cards;
        }

        /// <summary>Returns the cards sorted lowest-first. The sort is stable.</summary>
        public static List<Card> SortCards(IEnumerable<Card> cards, Ranks? ranks = null) =>
            cards.OrderBy(c => c, ranks ?? Ranks.Default).ToList();

        /// <summary>Whether the cards are in ascending or descending order.</summary>
        public static bool IsSorted(IReadOnlyList<Card> cards, Ranks? ranks = null)
        {
            ranks ??= Ranks.Default;
            bool ascending = true;
            bool descending = true;

            for (int i = 1; i < cards.Count && (ascending || descending); i++)
            {
                int cmp = ranks.Compare(cards[i - 1], cards[i]);
                ascending &= cmp <= 0;
                descending &= cmp >= 0;
            }

            return ascending || descending;
        }

        /// <summary>Whether both sequences contain exactly the same cards, in any order.</summary>
        public static bool CompareStacks(IEnumerable<Card> x, IEnumerable<Card> y)
        {
            var counts = new int[Card.KeyCount];
            int total = 0;

            foreach (Card card in x)
            {
                counts[card.Key]++;
                total++;
            }

            foreach (Card card in y)
            {
                if (--counts[card.Key] < 0)
                {
                    return false;
                }

                total--;
            }

            return total == 0;
        }

        /// <summary>
        /// Returns the indices of cards matching <paramref name="term"/> (see <see cref="Card.Matches"/>).
        /// </summary>
        /// <param name="cards">The cards to search.</param>
        /// <param name="term">A name, abbreviation, value or suit.</param>
        /// <param name="limit">Maximum number of results; 0 for no limit.</param>
        /// <param name="sort">Sort the results by card rank instead of position.</param>
        /// <param name="ranks">Ranks used when sorting.</param>
        public static List<int> FindCard(IReadOnlyList<Card> cards, string term, int limit = 0,
            bool sort = false, Ranks? ranks = null) =>
            FindList(cards, new[] { term }, limit, sort, ranks);

        /// <summary>
        /// Returns the indices of cards matching any of <paramref name="terms"/>, without duplicates.
        /// <paramref name="limit"/> applies to each term separately.
        /// </summary>
        public static List<int> FindList(IReadOnlyList<Card> cards, IEnumerable<string> terms, int limit = 0,
            bool sort = false, Ranks? ranks = null)
        {
            if (limit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limit), limit, "Must be zero (no limit) or more.");
            }

            var found = new List<int>();
            var seen = new bool[cards.Count];

            foreach (string term in terms)
            {
                int count = 0;

                for (int i = 0; i < cards.Count && (limit == 0 || count < limit); i++)
                {
                    if (cards[i].Matches(term))
                    {
                        count++;

                        if (!seen[i])
                        {
                            seen[i] = true;
                            found.Add(i);
                        }
                    }
                }
            }

            if (sort)
            {
                Ranks r = ranks ?? Ranks.Default;
                found = found.OrderBy(i => cards[i], r).ToList();
            }

            return found;
        }

        /// <summary>Writes one card name per line.</summary>
        public static void SaveCards(IEnumerable<Card> cards, TextWriter writer)
        {
            foreach (Card card in cards)
            {
                writer.WriteLine(card.Name);
            }
        }

        /// <summary>Writes one card name per line to a file.</summary>
        public static void SaveCards(IEnumerable<Card> cards, string path)
        {
            using var writer = new StreamWriter(path);
            SaveCards(cards, writer);
        }

        /// <summary>Reads cards written by <see cref="SaveCards(IEnumerable{Card}, TextWriter)"/>. Blank lines are skipped.</summary>
        /// <exception cref="FormatException">A line is not a valid card.</exception>
        public static List<Card> LoadCards(TextReader reader)
        {
            var cards = new List<Card>();
            string? line;

            while ((line = reader.ReadLine()) is not null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    cards.Add(Card.Parse(line));
                }
            }

            return cards;
        }

        /// <summary>Reads cards from a file written by <see cref="SaveCards(IEnumerable{Card}, string)"/>.</summary>
        public static List<Card> LoadCards(string path)
        {
            using var reader = new StreamReader(path);
            return LoadCards(reader);
        }

        //===============================================================================
        // Randomness
        //===============================================================================

#if NET6_0_OR_GREATER
        internal static Random SharedRandom => Random.Shared;
#else
        [ThreadStatic]
        private static Random? t_random;

        // System.Random is not thread-safe, so older frameworks get one instance per thread.
        internal static Random SharedRandom =>
            t_random ??= new Random(Guid.NewGuid().GetHashCode());
#endif

        /// <summary>Shuffles the list in place (Fisher–Yates).</summary>
        public static void Shuffle(IList<Card> cards, Random? random = null)
        {
            random ??= SharedRandom;

            for (int n = cards.Count - 1; n > 0; n--)
            {
                int k = random.Next(n + 1);
                (cards[k], cards[n]) = (cards[n], cards[k]);
            }
        }
    }
}
