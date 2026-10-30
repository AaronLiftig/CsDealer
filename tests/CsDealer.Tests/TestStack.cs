using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsDealer.Tests
{
    public class TestStack
    {
        private static readonly Card AceSpades = new(CardValue.Ace, Suit.Spades);
        private static readonly Card TwoDiamonds = new(CardValue.Two, Suit.Diamonds);
        private static readonly Card QueenHearts = new(CardValue.Queen, Suit.Hearts);
        private static readonly Card SevenClubs = new(CardValue.Seven, Suit.Clubs);

        private static readonly string[] Names = { "Ace of Spades", "2 of Diamonds", "Queen of Hearts", "7 of Clubs" };

        private Stack _empty = null!;
        private Stack _full = null!;
        private Stack _small = null!;

        [SetUp]
        public void Setup()
        {
            _empty = new Stack();
            _full = new Stack(CardTools.BuildCards());
            _small = new Stack(new[] { AceSpades, TwoDiamonds, QueenHearts, SevenClubs });
        }

        [Test]
        public void ConstructorCopiesInput()
        {
            var source = new List<Card> { AceSpades };
            var stack = new Stack(source);
            source.Add(TwoDiamonds);

            Assert.That(stack, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddTopAndBottom()
        {
            _small.Add(Card.Joker);
            Assert.That(_small.Peek(), Is.EqualTo(Card.Joker));

            _small.Add(QueenHearts, StackEnd.Bottom);
            Assert.That(_small.Peek(StackEnd.Bottom), Is.EqualTo(QueenHearts));
        }

        [Test]
        public void AddRangePreservesOrder()
        {
            _empty.AddRange(new[] { AceSpades, TwoDiamonds });
            _empty.AddRange(new[] { QueenHearts, SevenClubs }, StackEnd.Bottom);

            Assert.That(_empty, Is.EqualTo(new[] { QueenHearts, SevenClubs, AceSpades, TwoDiamonds }));
        }

        [Test]
        public void PlusReturnsNewStack()
        {
            Stack combined = _small + new[] { Card.Joker };

            Assert.That(combined, Has.Count.EqualTo(5));
            Assert.That(combined.Peek(), Is.EqualTo(Card.Joker));
            Assert.That(_small, Has.Count.EqualTo(4));
        }

        [Test]
        public void PeekEmptyThrows()
        {
            Assert.Throws<InvalidOperationException>(() => _empty.Peek());
            Assert.Throws<InvalidOperationException>(() => _empty.RandomCard());
        }

        [Test]
        public void Deal()
        {
            Stack dealt = _small.Deal(2);

            Assert.That(dealt, Is.EqualTo(new[] { SevenClubs, QueenHearts }));
            Assert.That(_small, Is.EqualTo(new[] { AceSpades, TwoDiamonds }));
            Assert.That(_small.Deal(10), Has.Count.EqualTo(2));
            Assert.That(_small.Deal(), Is.Empty);
        }

        [Test]
        public void DealBottom()
        {
            Assert.That(_small.Deal(2, StackEnd.Bottom), Is.EqualTo(new[] { AceSpades, TwoDiamonds }));
        }

        [Test]
        public void EmptyReturnsCards()
        {
            List<Card> cards = _full.Empty();

            Assert.That(cards, Has.Count.EqualTo(52));
            Assert.That(_full, Is.Empty);
        }

        [Test]
        public void ContainsAndRemove()
        {
            Assert.That(_small.Contains(AceSpades), Is.True);
            Assert.That(_small.Remove(AceSpades), Is.True);
            Assert.That(_small.Contains(AceSpades), Is.False);

            _small.RemoveAt(0);
            Assert.That(_small.Contains(TwoDiamonds), Is.False);
        }

        [Test]
        public void SameCardsIgnoresOrder()
        {
            var shuffled = new Stack(CardTools.BuildCards());
            shuffled.Shuffle(random: new Random(1));

            Assert.That(_full.HasSameCards(shuffled), Is.True);
            Assert.That(_full.HasSameCards(_small), Is.False);
            Assert.That(_full.SequenceEqual(new Stack(CardTools.BuildCards())), Is.True);
        }

        [TestCase("AS", 1)]
        [TestCase("Ace of Spades", 1)]
        [TestCase("ace", 4)]
        [TestCase("Spades", 13)]
        [TestCase("Bogus", 0)]
        public void Find(string term, int expected)
        {
            List<int> found = _full.Find(term);

            Assert.That(found, Has.Count.EqualTo(expected));
            Assert.That(found.All(i => _full[i].Matches(term)), Is.True);
        }

        [Test]
        public void FindLimit()
        {
            Assert.That(_full.Find("Spades", limit: 3), Has.Count.EqualTo(3));
        }

        [Test]
        public void FindSorted()
        {
            List<int> found = _small.Find("Ace", sort: true);
            Assert.That(found, Is.EqualTo(new[] { 0 }));

            var stack = new Stack(new[] { AceSpades, TwoDiamonds, new Card(CardValue.Ace, Suit.Diamonds) });
            List<int> sorted = stack.FindList(new[] { "Ace", "2" }, sort: true);
            Assert.That(sorted.Select(i => stack[i]), Is.EqualTo(new[] { TwoDiamonds, stack[2], AceSpades }));
        }

        [Test]
        public void FindListFullNamesAndAbbrevs()
        {
            Assert.That(_small.FindList(Names), Is.EqualTo(new[] { 0, 1, 2, 3 }));
            Assert.That(_small.FindList(new[] { "AS", "2D", "QH", "7C" }), Is.EqualTo(new[] { 0, 1, 2, 3 }));
            Assert.That(_small.FindList(new[] { "Ace", "Ace of Spades" }), Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void GetByIndex()
        {
            Card card = _small.Get(1);

            Assert.That(card, Is.EqualTo(TwoDiamonds));
            Assert.That(_small, Has.Count.EqualTo(3));
        }

        [Test]
        public void GetByTerm()
        {
            List<Card> spades = _full.Get("Spades");

            Assert.That(spades, Has.Count.EqualTo(13));
            Assert.That(_full, Has.Count.EqualTo(39));
            Assert.That(_full.Find("Spades"), Is.Empty);
        }

        [Test]
        public void GetListByTerms()
        {
            List<Card> got = _small.GetList(new[] { "QH", "Ace" }, sort: true);

            Assert.That(got, Is.EqualTo(new[] { QueenHearts, AceSpades }));
            Assert.That(_small, Is.EqualTo(new[] { TwoDiamonds, SevenClubs }));
        }

        [Test]
        public void GetListByIndices()
        {
            List<Card> got = _small.GetList(new[] { 3, 0, 3 });

            Assert.That(got, Is.EqualTo(new[] { SevenClubs, AceSpades }));
            Assert.That(_small, Is.EqualTo(new[] { TwoDiamonds, QueenHearts }));
        }

        [Test]
        public void GetListBadIndexLeavesStackUnchanged()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _small.GetList(new[] { 0, 9 }));
            Assert.That(_small, Has.Count.EqualTo(4));
        }

        [Test]
        public void Insert()
        {
            _small.Insert(1, Card.Joker);
            _small.InsertRange(0, new[] { AceSpades, AceSpades });

            Assert.That(_small[3], Is.EqualTo(Card.Joker));
            Assert.That(_small, Has.Count.EqualTo(7));
        }

        [Test]
        public void SortAndIsSorted()
        {
            _full.Shuffle(random: new Random(42));
            Assert.That(_full.IsSorted(), Is.False);

            _full.Sort();
            Assert.That(_full.IsSorted(), Is.True);
            Assert.That(_full.Peek(), Is.EqualTo(AceSpades));
            Assert.That(_full.Peek(StackEnd.Bottom), Is.EqualTo(TwoDiamonds));

            _full.Reverse();
            Assert.That(_full.IsSorted(), Is.True);
        }

        [Test]
        public void SortWithCustomRanks()
        {
            _small.Sort(Ranks.Big2);

            Assert.That(_small, Is.EqualTo(new[] { SevenClubs, QueenHearts, AceSpades, TwoDiamonds }));
        }

        [Test]
        public void SortingWithPokerRanksIsStable()
        {
            var aceDiamonds = new Card(CardValue.Ace, Suit.Diamonds);
            var stack = new Stack(new[] { AceSpades, aceDiamonds, TwoDiamonds }, Ranks.Poker, sort: true);

            Assert.That(stack, Is.EqualTo(new[] { TwoDiamonds, AceSpades, aceDiamonds }));
        }

        [Test]
        public void ShuffleWithSeedIsReproducible()
        {
            var a = new Stack(CardTools.BuildCards());
            var b = new Stack(CardTools.BuildCards());
            a.Shuffle(random: new Random(7));
            b.Shuffle(random: new Random(7));

            Assert.That(a, Is.EqualTo(b));
            Assert.That(a.HasSameCards(_full), Is.True);
        }

        [Test]
        public void RandomCard()
        {
            Card card = _small.RandomCard(remove: true);

            Assert.That(_small, Has.Count.EqualTo(3));
            Assert.That(_small.Contains(card), Is.False);
        }

        [Test]
        public void Split()
        {
            (Stack bottom, Stack top) = _full.Split();
            Assert.That(bottom, Has.Count.EqualTo(26));
            Assert.That(top, Has.Count.EqualTo(26));

            (bottom, top) = _small.Split(1);
            Assert.That(bottom, Is.EqualTo(new[] { AceSpades }));
            Assert.That(top, Has.Count.EqualTo(3));

            (bottom, top) = _empty.Split();
            Assert.That(bottom, Is.Empty);
            Assert.That(top, Is.Empty);

            Assert.Throws<ArgumentOutOfRangeException>(() => _small.Split(5));
        }

        [Test]
        public void SaveAndLoadRoundTrip()
        {
            _small.Add(Card.Joker);
            var writer = new StringWriter();
            _small.SaveCards(writer);

            var loaded = new Stack();
            loaded.LoadCards(new StringReader(writer.ToString()));

            Assert.That(loaded, Is.EqualTo(_small));
        }

        [Test]
        public void SaveAndLoadFile()
        {
            string path = Path.GetTempFileName();

            try
            {
                _full.SaveCards(path);
                var loaded = new Stack();
                loaded.LoadCards(path);

                Assert.That(loaded, Is.EqualTo(_full));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void LoadsLegacyFormat()
        {
            List<Card> cards = CardTools.LoadCards(new StringReader("Ace Spades\n10 Hearts\n\n"));

            Assert.That(cards, Is.EqualTo(new[] { AceSpades, new Card(CardValue.Ten, Suit.Hearts) }));
        }

        [Test]
        public void ToStringListsNames()
        {
            Assert.That(_small.ToString(), Is.EqualTo(string.Join(Environment.NewLine, Names)));
            Assert.That(_empty.ToString(), Is.Empty);
        }
    }
}
