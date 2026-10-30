using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsDealer
{
    /// <summary>
    /// A general-purpose ordered pile of cards: a hand, a discard pile, a deck, etc.
    /// Index 0 is the bottom; the last index is the top.
    /// </summary>
    public class Stack : IList<Card>, IReadOnlyList<Card>
    {
        private protected List<Card> _cards;
        private Ranks _ranks;

        /// <summary>Creates an empty stack.</summary>
        public Stack()
            : this(null)
        {
        }

        /// <summary>Creates a stack containing a copy of <paramref name="cards"/>.</summary>
        /// <param name="cards">Initial contents, bottom first.</param>
        /// <param name="ranks">Default ranking for sorting; <see cref="CsDealer.Ranks.Default"/> if omitted.</param>
        /// <param name="sort">Sort the cards immediately.</param>
        public Stack(IEnumerable<Card>? cards, Ranks? ranks = null, bool sort = false)
        {
            _cards = cards is null ? new List<Card>() : new List<Card>(cards);
            _ranks = ranks ?? Ranks.Default;

            if (sort)
            {
                Sort();
            }
        }

        // Takes ownership of the list without copying.
        internal Stack(List<Card> cards, Ranks ranks)
        {
            _cards = cards;
            _ranks = ranks;
        }

        /// <summary>The ranking used when no ranks are passed to a method.</summary>
        public Ranks Ranks
        {
            get => _ranks;
            set => _ranks = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>The number of cards.</summary>
        public int Count => _cards.Count;

        /// <summary>Whether the stack has no cards.</summary>
        public bool IsEmpty => _cards.Count == 0;

        /// <summary>Gets or sets the card at <paramref name="index"/> (0 is the bottom).</summary>
        public Card this[int index]
        {
            get => _cards[index];
            set => _cards[index] = value;
        }

        /// <summary>Returns the card at an end without removing it.</summary>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        public Card Peek(StackEnd end = StackEnd.Top)
        {
            ThrowIfEmpty();
            return _cards[IndexOf(end)];
        }

        /// <summary>Concatenates the stack with more cards, returning a new stack.</summary>
        public static Stack operator +(Stack stack, IEnumerable<Card> other) =>
            new(stack._cards.Concat(other), stack._ranks);

        //===============================================================================
        // Adding and removing
        //===============================================================================

        /// <summary>Adds a card to the top.</summary>
        public void Add(Card card) => _cards.Add(card);

        /// <summary>Adds a card to the given end.</summary>
        public void Add(Card card, StackEnd end)
        {
            if (IsTop(end))
            {
                _cards.Add(card);
            }
            else
            {
                _cards.Insert(0, card);
            }
        }

        /// <summary>
        /// Adds cards to the given end. Order is preserved, so with <see cref="StackEnd.Top"/> the last card
        /// ends up on top, and with <see cref="StackEnd.Bottom"/> the first card ends up at the bottom.
        /// </summary>
        public void AddRange(IEnumerable<Card> cards, StackEnd end = StackEnd.Top)
        {
            if (IsTop(end))
            {
                _cards.AddRange(cards);
            }
            else
            {
                _cards.InsertRange(0, cards);
            }
        }

        /// <summary>Inserts a card at <paramref name="index"/> (0 to <see cref="Count"/>).</summary>
        public void Insert(int index, Card card) => _cards.Insert(index, card);

        /// <summary>Inserts cards at <paramref name="index"/> (0 to <see cref="Count"/>).</summary>
        public void InsertRange(int index, IEnumerable<Card> cards) => _cards.InsertRange(index, cards);

        /// <summary>Removes the first occurrence of a card.</summary>
        public bool Remove(Card card) => _cards.Remove(card);

        /// <summary>Removes the card at <paramref name="index"/>.</summary>
        public void RemoveAt(int index) => _cards.RemoveAt(index);

        /// <summary>Removes all cards.</summary>
        public void Clear() => _cards.Clear();

        /// <summary>Removes and returns all cards.</summary>
        public List<Card> Empty()
        {
            List<Card> cards = _cards;
            _cards = new List<Card>();
            return cards;
        }

        /// <summary>Replaces the contents with a copy of <paramref name="cards"/>.</summary>
        public void SetCards(IEnumerable<Card> cards)
        {
            if (cards is null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            _cards = new List<Card>(cards);
        }

        //===============================================================================
        // Dealing
        //===============================================================================

        /// <summary>
        /// Removes up to <paramref name="count"/> cards from an end and returns them, in the order dealt.
        /// Deals fewer cards if the stack runs out.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
        public virtual Stack Deal(int count = 1, StackEnd end = StackEnd.Top)
        {
            ThrowIfNotPositive(count);

            var dealt = new List<Card>(Math.Min(count, _cards.Count));
            TakeInto(dealt, Math.Min(count, _cards.Count), end);
            return new Stack(dealt, _ranks);
        }

        /// <summary>Removes <paramref name="count"/> cards from an end and appends them to <paramref name="dealt"/>.</summary>
        private protected void TakeInto(List<Card> dealt, int count, StackEnd end)
        {
            if (IsTop(end))
            {
                int start = _cards.Count - count;

                for (int i = _cards.Count - 1; i >= start; i--)
                {
                    dealt.Add(_cards[i]);
                }

                _cards.RemoveRange(start, count);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dealt.Add(_cards[i]);
                }

                _cards.RemoveRange(0, count);
            }
        }

        //===============================================================================
        // Searching
        //===============================================================================

        /// <summary>Whether the stack contains the card.</summary>
        public bool Contains(Card card) => _cards.Contains(card);

        /// <summary>The index of the first occurrence of the card, or -1.</summary>
        public int IndexOf(Card card) => _cards.IndexOf(card);

        /// <summary>Indices of cards matching <paramref name="term"/>. See <see cref="CardTools.FindCard"/>.</summary>
        public List<int> Find(string term, int limit = 0, bool sort = false, Ranks? ranks = null) =>
            CardTools.FindCard(_cards, term, limit, sort, ranks ?? _ranks);

        /// <summary>Indices of cards matching any term. See <see cref="CardTools.FindList"/>.</summary>
        public List<int> FindList(IEnumerable<string> terms, int limit = 0, bool sort = false, Ranks? ranks = null) =>
            CardTools.FindList(_cards, terms, limit, sort, ranks ?? _ranks);

        /// <summary>Removes and returns the card at <paramref name="index"/>.</summary>
        public Card Get(int index)
        {
            Card card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }

        /// <summary>Removes and returns the cards matching <paramref name="term"/>.</summary>
        public List<Card> Get(string term, int limit = 0, bool sort = false, Ranks? ranks = null) =>
            GetList(Find(term, limit), sort, ranks);

        /// <summary>Removes and returns the cards matching any of <paramref name="terms"/>.</summary>
        public List<Card> GetList(IEnumerable<string> terms, int limit = 0, bool sort = false, Ranks? ranks = null) =>
            GetList(FindList(terms, limit), sort, ranks);

        /// <summary>Removes and returns the cards at the given indices. Duplicate indices are ignored.</summary>
        /// <exception cref="ArgumentOutOfRangeException">An index is out of range. The stack is left unchanged.</exception>
        public List<Card> GetList(IEnumerable<int> indices, bool sort = false, Ranks? ranks = null)
        {
            var taken = new bool[_cards.Count];
            var got = new List<Card>();

            foreach (int index in indices)
            {
                if ((uint)index >= (uint)_cards.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(indices), index, "Index is out of range.");
                }

                if (!taken[index])
                {
                    taken[index] = true;
                    got.Add(_cards[index]);
                }
            }

            // Compact the remaining cards in a single pass.
            int write = 0;

            for (int read = 0; read < _cards.Count; read++)
            {
                if (!taken[read])
                {
                    _cards[write++] = _cards[read];
                }
            }

            _cards.RemoveRange(write, _cards.Count - write);

            return sort ? CardTools.SortCards(got, ranks ?? _ranks) : got;
        }

        /// <summary>Returns a random card, optionally removing it.</summary>
        /// <exception cref="InvalidOperationException">The stack is empty.</exception>
        public Card RandomCard(bool remove = false, Random? random = null)
        {
            ThrowIfEmpty();

            int index = (random ?? CardTools.SharedRandom).Next(_cards.Count);
            Card card = _cards[index];

            if (remove)
            {
                _cards.RemoveAt(index);
            }

            return card;
        }

        //===============================================================================
        // Ordering
        //===============================================================================

        /// <summary>Whether the cards are in ascending or descending order.</summary>
        public bool IsSorted(Ranks? ranks = null) => CardTools.IsSorted(_cards, ranks ?? _ranks);

        /// <summary>Sorts the cards lowest-first (stable).</summary>
        public void Sort(Ranks? ranks = null) => _cards = CardTools.SortCards(_cards, ranks ?? _ranks);

        /// <summary>Reverses the order of the cards.</summary>
        public void Reverse() => _cards.Reverse();

        /// <summary>Shuffles the cards. Pass a seeded <see cref="Random"/> for reproducible results.</summary>
        public void Shuffle(int times = 1, Random? random = null)
        {
            for (int i = 0; i < times; i++)
            {
                CardTools.Shuffle(_cards, random);
            }
        }

        /// <summary>
        /// Returns two new stacks: cards below <paramref name="index"/> and the rest.
        /// With no index, splits in half (the extra card of an odd stack goes to the second half).
        /// The stack itself is unchanged.
        /// </summary>
        public (Stack Bottom, Stack Top) Split(int? index = null)
        {
            int at = index ?? _cards.Count / 2;

            if ((uint)at > (uint)_cards.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Must be between 0 and {_cards.Count}.");
            }

            return (new Stack(_cards.GetRange(0, at), _ranks), new Stack(_cards.GetRange(at, _cards.Count - at), _ranks));
        }

        /// <summary>Whether both stacks hold exactly the same cards, in any order.</summary>
        public bool HasSameCards(IEnumerable<Card> other) => CardTools.CompareStacks(_cards, other);

        //===============================================================================
        // Persistence and formatting
        //===============================================================================

        /// <summary>Saves the cards to a text file, one name per line.</summary>
        public void SaveCards(string path) => CardTools.SaveCards(_cards, path);

        /// <summary>Saves the cards to a writer, one name per line.</summary>
        public void SaveCards(TextWriter writer) => CardTools.SaveCards(_cards, writer);

        /// <summary>Replaces the contents with cards loaded from a text file.</summary>
        public void LoadCards(string path) => _cards = CardTools.LoadCards(path);

        /// <summary>Replaces the contents with cards loaded from a reader.</summary>
        public void LoadCards(TextReader reader) => _cards = CardTools.LoadCards(reader);

        /// <summary>The card names, one per line, bottom first.</summary>
        public override string ToString() => string.Join(Environment.NewLine, _cards);

        //===============================================================================
        // Collection plumbing
        //===============================================================================

        /// <summary>Returns a struct enumerator (bottom to top) that does not allocate.</summary>
        public List<Card>.Enumerator GetEnumerator() => _cards.GetEnumerator();

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cards.GetEnumerator();

        bool ICollection<Card>.IsReadOnly => false;

        /// <inheritdoc/>
        public void CopyTo(Card[] array, int arrayIndex) => _cards.CopyTo(array, arrayIndex);

        /// <summary>Copies the cards into a new list.</summary>
        public List<Card> ToList() => new(_cards);

        private int IndexOf(StackEnd end) => IsTop(end) ? _cards.Count - 1 : 0;

        private protected static bool IsTop(StackEnd end) => end switch
        {
            StackEnd.Top => true,
            StackEnd.Bottom => false,
            _ => throw new ArgumentOutOfRangeException(nameof(end), end, "Undefined stack end."),
        };

        private protected static void ThrowIfNotPositive(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must be at least 1.");
            }
        }

        private void ThrowIfEmpty()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }
        }
    }
}
