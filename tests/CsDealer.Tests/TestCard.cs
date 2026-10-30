using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CsDealer.Tests
{
    public class TestCard
    {
        private static readonly Card AceSpades = new(CardValue.Ace, Suit.Spades);
        private static readonly Card TwoDiamonds = new(CardValue.Two, Suit.Diamonds);
        private static readonly Card AceHearts = new(CardValue.Ace, Suit.Hearts);

        [Test]
        public void Properties()
        {
            Assert.That(AceSpades.Value, Is.EqualTo(CardValue.Ace));
            Assert.That(AceSpades.Suit, Is.EqualTo(Suit.Spades));
            Assert.That(AceSpades.Name, Is.EqualTo("Ace of Spades"));
            Assert.That(AceSpades.Abbrev, Is.EqualTo("AS"));
            Assert.That(AceSpades.ToString(), Is.EqualTo("Ace of Spades"));
            Assert.That(new Card(CardValue.Ten, Suit.Hearts).Abbrev, Is.EqualTo("10H"));
            Assert.That(AceHearts.IsRed, Is.True);
            Assert.That(AceSpades.IsRed, Is.False);
        }

        [Test]
        public void JokerIsDefault()
        {
            Assert.That(default(Card), Is.EqualTo(Card.Joker));
            Assert.That(Card.Joker.IsJoker, Is.True);
            Assert.That(Card.Joker.Suit, Is.Null);
            Assert.That(Card.Joker.Name, Is.EqualTo("Joker"));
            Assert.That(Card.Joker.Abbrev, Is.EqualTo("JKR"));
        }

        [Test]
        public void ConstructorRejectsJokerAndUndefined()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Card(CardValue.Joker, Suit.Spades));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Card((CardValue)1, Suit.Spades));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Card(CardValue.Ace, (Suit)9));
        }

        [Test]
        public void Equality()
        {
            Assert.That(AceSpades == new Card(CardValue.Ace, Suit.Spades), Is.True);
            Assert.That(AceSpades != TwoDiamonds, Is.True);
            Assert.That(AceSpades.Equals((object)TwoDiamonds), Is.False);
            Assert.That(AceSpades.GetHashCode(), Is.EqualTo(new Card(CardValue.Ace, Suit.Spades).GetHashCode()));

            var set = new HashSet<Card>(CardTools.BuildCards(2));
            Assert.That(set, Has.Count.EqualTo(53));
        }

        [Test]
        public void DefaultComparison()
        {
            Assert.That(AceSpades > TwoDiamonds, Is.True);
            Card aceSpades = new(CardValue.Ace, Suit.Spades);
            Assert.That(AceSpades >= aceSpades, Is.True);
            Assert.That(AceSpades > aceSpades, Is.False);
            Assert.That(TwoDiamonds < AceSpades, Is.True);
            Assert.That(TwoDiamonds <= AceSpades, Is.True);
            Assert.That(AceSpades <= TwoDiamonds, Is.False);
            // Suits break ties by default: Spades > Hearts.
            Assert.That(AceSpades > AceHearts, Is.True);
            Assert.That(Card.Joker < TwoDiamonds, Is.True);
        }

        [Test]
        public void RankedComparison()
        {
            Assert.That(AceSpades.Equals(AceHearts, Ranks.Poker), Is.True);
            Assert.That(AceSpades.Equals(AceHearts, Ranks.Default), Is.False);
            Assert.That(TwoDiamonds.CompareTo(AceSpades, Ranks.Big2), Is.GreaterThan(0));
            Assert.That(TwoDiamonds.CompareTo(AceSpades, Ranks.Poker), Is.LessThan(0));
        }

        [TestCase("Ace of Spades", CardValue.Ace, Suit.Spades)]
        [TestCase("ace of spades", CardValue.Ace, Suit.Spades)]
        [TestCase("AS", CardValue.Ace, Suit.Spades)]
        [TestCase("as", CardValue.Ace, Suit.Spades)]
        [TestCase("10h", CardValue.Ten, Suit.Hearts)]
        [TestCase("TH", CardValue.Ten, Suit.Hearts)]
        [TestCase("Queen Clubs", CardValue.Queen, Suit.Clubs)]
        [TestCase("  7 of diamonds ", CardValue.Seven, Suit.Diamonds)]
        [TestCase("Two of Hearts", CardValue.Two, Suit.Hearts)]
        public void Parse(string text, CardValue value, Suit suit)
        {
            Assert.That(Card.Parse(text), Is.EqualTo(new Card(value, suit)));
        }

        [TestCase("Joker")]
        [TestCase("jkr")]
        public void ParseJoker(string text)
        {
            Assert.That(Card.Parse(text), Is.EqualTo(Card.Joker));
        }

        [TestCase("")]
        [TestCase("1S")]
        [TestCase("Ace of Swords")]
        [TestCase("Z")]
        [TestCase(null)]
        public void ParseInvalid(string? text)
        {
            Assert.That(Card.TryParse(text, out _), Is.False);
            Assert.Throws<FormatException>(() => Card.Parse(text!));
        }

        [Test]
        public void ParseRoundTripsEveryCard()
        {
            foreach (Card card in CardTools.BuildCards(1))
            {
                Assert.That(Card.Parse(card.Name), Is.EqualTo(card));
                Assert.That(Card.Parse(card.Abbrev), Is.EqualTo(card));
            }
        }

        [Test]
        public void Matches()
        {
            Assert.That(AceSpades.Matches("ace"), Is.True);
            Assert.That(AceSpades.Matches("SPADES"), Is.True);
            Assert.That(AceSpades.Matches("as"), Is.True);
            Assert.That(AceSpades.Matches("Ace of Spades"), Is.True);
            Assert.That(AceSpades.Matches("Hearts"), Is.False);
            Assert.That(Card.Joker.Matches("Spades"), Is.False);
            Assert.That(Card.Joker.Matches("joker"), Is.True);
        }
    }
}
