using System;
using System.Collections.Generic;
using System.Linq;

namespace CsDealer
{
    /// <summary>
    /// A <see cref="Stack"/> that knows how to build itself from standard decks and can rebuild when it runs out.
    /// </summary>
    public class Deck : Stack
    {
        private int _jokers;

        /// <summary>Creates a deck, building one standard 52-card deck unless <paramref name="build"/> is false.</summary>
        /// <param name="jokers">Jokers added per build.</param>
        /// <param name="build">Build the first deck immediately.</param>
        /// <param name="rebuild">Build a new deck when dealing runs out of cards.</param>
        /// <param name="reshuffle">Shuffle each rebuilt deck.</param>
        /// <param name="ranks">Default ranking; <see cref="CsDealer.Ranks.Default"/> if omitted.</param>
        public Deck(int jokers = 0, bool build = true, bool rebuild = false, bool reshuffle = false, Ranks? ranks = null)
            : base(ranks ?? Ranks.Default, new List<Card>())
        {
            Jokers = jokers;
            Rebuild = rebuild;
            Reshuffle = reshuffle;

            if (build)
            {
                Build();
            }
        }

        /// <summary>Creates a deck holding a copy of <paramref name="cards"/>. Nothing is built.</summary>
        public Deck(IEnumerable<Card> cards, int jokers = 0, bool rebuild = false, bool reshuffle = false, Ranks? ranks = null)
            : base(ranks ?? Ranks.Default, new List<Card>(cards ?? throw new ArgumentNullException(nameof(cards))))
        {
            Jokers = jokers;
            Rebuild = rebuild;
            Reshuffle = reshuffle;
        }

        /// <summary>Number of jokers added by each <see cref="Build"/>.</summary>
        public int Jokers
        {
            get => _jokers;
            set => _jokers = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), value, "Must be zero or more.");
        }

        /// <summary>Whether <see cref="Deal"/> builds a new deck when this one runs out.</summary>
        public bool Rebuild { get; set; }

        /// <summary>Whether rebuilt decks are shuffled.</summary>
        public bool Reshuffle { get; set; }

        /// <summary>How many times <see cref="Build"/> has run.</summary>
        public int DecksUsed { get; private set; }

        /// <summary>Concatenates the deck with more cards, returning a new deck with the same settings.</summary>
        public static Deck operator +(Deck deck, IEnumerable<Card> other) =>
            new(deck.Concat(other), deck.Jokers, deck.Rebuild, deck.Reshuffle, deck.Ranks);

        /// <summary>Adds a fresh, ordered standard deck (plus <see cref="Jokers"/>) to the top.</summary>
        public void Build()
        {
            DecksUsed++;
            _cards.AddRange(CardTools.BuildCards(Jokers));
        }

        /// <summary>
        /// Deals cards like <see cref="Stack.Deal"/>. If <see cref="Rebuild"/> is set, builds (and optionally shuffles)
        /// new decks as needed so exactly <paramref name="count"/> cards are dealt.
        /// </summary>
        public override Stack Deal(int count = 1, StackEnd end = StackEnd.Top)
        {
            ThrowIfNotPositive(count);
            _ = IsTop(end); // validate before any rebuild side effects

            var dealt = new List<Card>(Rebuild ? count : Math.Min(count, Count));

            while (dealt.Count < count)
            {
                if (Count == 0)
                {
                    if (!Rebuild)
                    {
                        break;
                    }

                    Build();

                    if (Reshuffle)
                    {
                        Shuffle();
                    }
                }

                TakeInto(dealt, Math.Min(count - dealt.Count, Count), end);
            }

            return new Stack(Ranks, dealt);
        }
    }
}
