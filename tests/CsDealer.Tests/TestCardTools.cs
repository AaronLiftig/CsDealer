using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsDealer.Tests
{
    public class TestCardTools
    {
        [Test]
        public void BuildCardsOrder()
        {
            List<Card> cards = CardTools.BuildCards(1);

            Assert.That(cards, Has.Count.EqualTo(53));
            Assert.That(cards[0], Is.EqualTo(Card.Joker));
            Assert.That(cards[1].Abbrev, Is.EqualTo("2D"));
            Assert.That(cards[^1].Abbrev, Is.EqualTo("AS"));
            Assert.That(CardTools.IsSorted(cards), Is.True);
            Assert.Throws<ArgumentOutOfRangeException>(() => CardTools.BuildCards(-1));
        }

        [Test]
        public void IsSortedHandlesTrivialInputs()
        {
            Assert.That(CardTools.IsSorted(Array.Empty<Card>()), Is.True);
            Assert.That(CardTools.IsSorted(new[] { Card.Joker }), Is.True);
        }

        [Test]
        public void CompareStacks()
        {
            var a = new[] { Card.Joker, Card.Parse("AS"), Card.Parse("AS") };
            var b = new[] { Card.Parse("AS"), Card.Joker, Card.Parse("AS") };
            var c = new[] { Card.Parse("AS"), Card.Joker, Card.Joker };

            Assert.That(CardTools.CompareStacks(a, b), Is.True);
            Assert.That(CardTools.CompareStacks(a, c), Is.False);
            Assert.That(CardTools.CompareStacks(a, b.Take(2)), Is.False);
        }

        [Test]
        public void FindRejectsNegativeLimit()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CardTools.FindCard(CardTools.BuildCards(), "Ace", -1));
        }

        [Test]
        public void CustomRanks()
        {
            // Aces low, suits ignored.
            var values = Ranks.Poker;
            var aceLow = new Ranks(CardTools.Values.ToDictionary(v => v, v => v == CardValue.Ace ? 0 : values.ValueRank(v)));

            List<Card> sorted = CardTools.SortCards(new[] { Card.Parse("2C"), Card.Parse("AH") }, aceLow);

            Assert.That(sorted[0], Is.EqualTo(Card.Parse("AH")));
            Assert.That(aceLow.RanksSuits, Is.False);
            Assert.That(Ranks.Big2.SuitRank(Suit.Spades), Is.EqualTo(4));
            Assert.That(Ranks.Big2.SuitRank(null), Is.EqualTo(0));
        }

        [Test]
        public void RanksAsEqualityComparer()
        {
            var bySuitlessRank = new HashSet<Card>(CardTools.BuildCards(), Ranks.Poker);

            Assert.That(bySuitlessRank, Has.Count.EqualTo(13));
        }
    }
}
