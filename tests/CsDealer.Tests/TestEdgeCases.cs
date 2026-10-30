using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CsDealer.Tests
{
    /// <summary>Argument validation, collection plumbing and option paths not covered by the behavior tests.</summary>
    public class TestEdgeCases
    {
        private static readonly Card AceSpades = new(CardValue.Ace, Suit.Spades);
        private static readonly Card TwoDiamonds = new(CardValue.Two, Suit.Diamonds);

        //===============================================================================
        // Card
        //===============================================================================

        [Test]
        public void CardColorAndSuitName()
        {
            Assert.That(TwoDiamonds.IsRed, Is.True);
            Assert.That(new Card(CardValue.Two, Suit.Clubs).IsRed, Is.False);
            Assert.That(Card.Joker.IsRed, Is.False);
            Assert.That(AceSpades.SuitName, Is.EqualTo("Spades"));
            Assert.That(Card.Joker.SuitName, Is.Null);
            Assert.That(AceSpades.ValueName, Is.EqualTo("Ace"));
        }

        [Test]
        public void CardArgumentValidation()
        {
            Assert.Throws<ArgumentNullException>(() => AceSpades.Matches(null!));
            Assert.Throws<ArgumentNullException>(() => AceSpades.CompareTo(TwoDiamonds, null!));
            Assert.Throws<ArgumentNullException>(() => AceSpades.Equals(TwoDiamonds, null!));
        }

        [Test]
        public void CardObjectEqualityAndComparison()
        {
            Assert.That(AceSpades.Equals("Ace of Spades"), Is.False);
            Assert.That(AceSpades.Equals((object)AceSpades), Is.True);

            IComparable comparable = AceSpades;
            Assert.That(comparable.CompareTo(null), Is.GreaterThan(0));
            Assert.That(comparable.CompareTo(TwoDiamonds), Is.GreaterThan(0));
            Assert.Throws<ArgumentException>(() => comparable.CompareTo("AS"));

            // Non-generic sorting goes through IComparable.
            var list = new ArrayList { AceSpades, TwoDiamonds, Card.Joker };
            list.Sort();
            Assert.That(list.Cast<Card>(), Is.EqualTo(new[] { Card.Joker, TwoDiamonds, AceSpades }));
        }

        [Test]
        public void ParseTwoPartForms()
        {
            Assert.That(Card.Parse("queen", "HEARTS"), Is.EqualTo(new Card(CardValue.Queen, Suit.Hearts)));
            Assert.That(Card.Parse("Joker", null), Is.EqualTo(Card.Joker));
            Assert.That(Card.Parse("Joker", " "), Is.EqualTo(Card.Joker));

            Assert.That(Card.TryParse("Joker", "Spades", out _), Is.False);
            Assert.That(Card.TryParse("Ace", null, out _), Is.False);
            Assert.That(Card.TryParse(null, "Spades", out _), Is.False);
            Assert.Throws<FormatException>(() => Card.Parse("Ace", "Swords"));
        }

        //===============================================================================
        // Stack
        //===============================================================================

        [Test]
        public void StackRanksProperty()
        {
            var stack = new Stack(new[] { AceSpades, TwoDiamonds });

            Assert.That(stack.Ranks, Is.SameAs(Ranks.Default));
            stack.Ranks = Ranks.Big2;
            stack.Sort();
            Assert.That(stack.Peek(), Is.EqualTo(TwoDiamonds));
            Assert.Throws<ArgumentNullException>(() => stack.Ranks = null!);
        }

        [Test]
        public void StackListPlumbing()
        {
            var stack = new Stack();
            Assert.That(stack.IsEmpty, Is.True);

            stack.Add(AceSpades, StackEnd.Top);
            stack.Add(TwoDiamonds);
            Assert.That(stack.IsEmpty, Is.False);
            Assert.That(stack.IndexOf(TwoDiamonds), Is.EqualTo(1));
            Assert.That(stack.IndexOf(Card.Joker), Is.EqualTo(-1));

            stack[0] = Card.Joker;
            Assert.That(stack[0], Is.EqualTo(Card.Joker));

            var array = new Card[3];
            stack.CopyTo(array, 1);
            Assert.That(array, Is.EqualTo(new[] { default, Card.Joker, TwoDiamonds }));

            List<Card> copy = stack.ToList();
            copy.Clear();
            Assert.That(stack, Has.Count.EqualTo(2));

            Assert.That(((ICollection<Card>)stack).IsReadOnly, Is.False);

            var seen = new List<Card>();
            foreach (Card card in stack)
            {
                seen.Add(card);
            }

            Assert.That(seen, Is.EqualTo(new[] { Card.Joker, TwoDiamonds }));

            stack.Clear();
            Assert.That(stack.IsEmpty, Is.True);
        }

        [Test]
        public void StackSetCards()
        {
            var source = new List<Card> { AceSpades };
            var stack = new Stack(new[] { TwoDiamonds });

            stack.SetCards(source);
            source.Add(TwoDiamonds);

            Assert.That(stack, Is.EqualTo(new[] { AceSpades }));
            Assert.Throws<ArgumentNullException>(() => stack.SetCards(null!));
        }

        [Test]
        public void StackRejectsUndefinedEnd()
        {
            var stack = new Stack(new[] { AceSpades });

            Assert.Throws<ArgumentOutOfRangeException>(() => stack.Add(TwoDiamonds, (StackEnd)9));
            Assert.Throws<ArgumentOutOfRangeException>(() => stack.Peek((StackEnd)9));
            Assert.Throws<ArgumentOutOfRangeException>(() => stack.Deal(1, (StackEnd)9));
            Assert.That(stack, Has.Count.EqualTo(1));
        }

        [Test]
        public void DealAndSplitShareRanks()
        {
            var stack = new Stack(CardTools.BuildCards(), Ranks.Big2);

            Assert.That(stack.Deal(3).Ranks, Is.SameAs(Ranks.Big2));
            Assert.That(stack.Split().Top.Ranks, Is.SameAs(Ranks.Big2));
        }

        [Test]
        public void DealtStacksDoNotShareLists()
        {
            var stack = new Stack(CardTools.BuildCards());
            (Stack bottom, Stack top) = stack.Split();

            bottom.Clear();
            Assert.That(stack, Has.Count.EqualTo(52));
            Assert.That(top, Has.Count.EqualTo(26));
        }

        //===============================================================================
        // Deck
        //===============================================================================

        [Test]
        public void DeckFromCards()
        {
            var deck = new Deck(new[] { AceSpades, TwoDiamonds }, ranks: Ranks.Big2);

            Assert.That(deck, Has.Count.EqualTo(2));
            Assert.That(deck.DecksUsed, Is.EqualTo(0));
            Assert.That(deck.Ranks, Is.SameAs(Ranks.Big2));
            Assert.Throws<ArgumentNullException>(() => new Deck((IEnumerable<Card>)null!));
        }

        //===============================================================================
        // Ranks and CardTools
        //===============================================================================

        [Test]
        public void RanksValidation()
        {
            Assert.Throws<ArgumentNullException>(() => new Ranks(null!));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Ranks(new Dictionary<CardValue, int> { [(CardValue)20] = 1 }));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Ranks(new Dictionary<CardValue, int>(), new Dictionary<Suit, int> { [(Suit)7] = 1 }));
        }

        [Test]
        public void UnlistedValuesRankZero()
        {
            var acesOnly = new Ranks(new Dictionary<CardValue, int> { [CardValue.Ace] = 1 });

            Assert.That(acesOnly.ValueRank(CardValue.King), Is.EqualTo(0));
            Assert.That(acesOnly.Compare(new Card(CardValue.King, Suit.Spades), TwoDiamonds), Is.EqualTo(0));
            Assert.That(acesOnly.Compare(AceSpades, TwoDiamonds), Is.GreaterThan(0));
        }

        [Test]
        public void CardToolsDefaultRanks()
        {
            List<Card> sorted = CardTools.SortCards(new[] { AceSpades, TwoDiamonds });
            Assert.That(sorted, Is.EqualTo(new[] { TwoDiamonds, AceSpades }));

            List<Card> cards = CardTools.BuildCards();
            List<int> found = CardTools.FindCard(cards, "Ace", sort: true);
            Assert.That(found.Select(i => cards[i].Abbrev), Is.EqualTo(new[] { "AD", "AC", "AH", "AS" }));
        }

        [Test]
        public void ShuffleListDirectly()
        {
            List<Card> a = CardTools.BuildCards();
            List<Card> b = CardTools.BuildCards();
            CardTools.Shuffle(a, new Random(3));
            CardTools.Shuffle(b, new Random(3));

            Assert.That(a, Is.EqualTo(b));
            Assert.That(CardTools.CompareStacks(a, CardTools.BuildCards()), Is.True);

            // The shared generator also works (and is per-thread on older frameworks).
            List<Card> c = CardTools.BuildCards();
            CardTools.Shuffle(c);
            Assert.That(CardTools.CompareStacks(c, a), Is.True);
        }

        //===============================================================================
        // CardSvg
        //===============================================================================

        [Test]
        public void BackWithOptions()
        {
            string svg = CardSvg.RenderBack(new CardSvgOptions { BackColor = "#7a1f1f", Width = 50 });

            Assert.That(svg, Does.Contain("#7a1f1f"));
            Assert.That(svg, Does.Contain("width=\"50\""));
            Assert.DoesNotThrow(() => XDocument.Parse(svg));
            Assert.That(ReferenceEquals(CardSvg.RenderBack(), CardSvg.RenderBack()), Is.True);
        }

        [Test]
        public void FourColorClubs()
        {
            var options = new CardSvgOptions { FourColor = true, ClubColor = "#00aa00" };

            Assert.That(CardSvg.Render(Card.Parse("3C"), options), Does.Contain("#00aa00"));
            Assert.That(CardSvg.Render(Card.Parse("3C")), Does.Not.Contain("#00aa00"));
        }

        [TestCase(0.0)]
        [TestCase(-5.0)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void WidthMustBePositiveAndFinite(double width)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CardSvgOptions { Width = width });
        }

        [Test]
        public void ExportWithoutJokerOrBack()
        {
            string dir = Path.Combine(Path.GetTempPath(), "csdealer-" + Guid.NewGuid().ToString("N"));

            try
            {
                var paths = CardSvg.ExportAll(dir, includeJoker: false, includeBack: false);

                Assert.That(paths, Has.Count.EqualTo(52));
                Assert.That(File.Exists(Path.Combine(dir, "JKR.svg")), Is.False);
                Assert.That(File.Exists(Path.Combine(dir, CardSvg.BackFileName)), Is.False);
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }
    }
}
