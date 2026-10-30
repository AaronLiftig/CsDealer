using NUnit.Framework;
using System;
using System.Linq;

namespace CsDealer.Tests
{
    public class TestDeck
    {
        [Test]
        public void BuildsStandardDeck()
        {
            var deck = new Deck();

            Assert.That(deck, Has.Count.EqualTo(52));
            Assert.That(deck.Distinct().Count(), Is.EqualTo(52));
            Assert.That(deck.DecksUsed, Is.EqualTo(1));
            Assert.That(new Deck(build: false), Is.Empty);
        }

        [Test]
        public void BuildsJokers()
        {
            var deck = new Deck(jokers: 2);

            Assert.That(deck, Has.Count.EqualTo(54));
            Assert.That(deck.Count(c => c.IsJoker), Is.EqualTo(2));
        }

        [Test]
        public void DealsFromTop()
        {
            var deck = new Deck();
            Stack dealt = deck.Deal(4);

            Assert.That(dealt.Select(c => c.Name), Is.EqualTo(new[]
            {
                "Ace of Spades", "Ace of Hearts", "Ace of Clubs", "Ace of Diamonds",
            }));
            Assert.That(deck, Has.Count.EqualTo(48));
        }

        [Test]
        public void DealsFromBottom()
        {
            var deck = new Deck();
            Stack dealt = deck.Deal(2, StackEnd.Bottom);

            Assert.That(dealt.Select(c => c.Abbrev), Is.EqualTo(new[] { "2D", "2C" }));
        }

        [Test]
        public void DealWithoutRebuildStopsWhenEmpty()
        {
            var deck = new Deck();

            Assert.That(deck.Deal(60), Has.Count.EqualTo(52));
            Assert.That(deck, Is.Empty);
            Assert.That(deck.Deal(), Is.Empty);
        }

        [Test]
        public void DealRebuilds()
        {
            var deck = new Deck(rebuild: true, reshuffle: true);
            Stack dealt = deck.Deal(53);

            Assert.That(dealt, Has.Count.EqualTo(53));
            Assert.That(deck, Has.Count.EqualTo(51));
            Assert.That(deck.DecksUsed, Is.EqualTo(2));

            Assert.That(deck.Deal(200), Has.Count.EqualTo(200));
        }

        [Test]
        public void DealRejectsBadArguments()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Deck().Deal(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Deck().Deal(1, (StackEnd)5));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Deck(jokers: -1));
        }

        [Test]
        public void PlusKeepsDeckSettings()
        {
            var deck = new Deck(build: false, rebuild: true, jokers: 1);
            Deck combined = deck + new Deck();

            Assert.That(combined, Has.Count.EqualTo(52));
            Assert.That(combined.Rebuild, Is.True);
            Assert.That(combined.Jokers, Is.EqualTo(1));
        }
    }
}
