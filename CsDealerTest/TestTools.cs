using NUnit.Framework;
using CsDealer;
using System.Collections.Generic;

namespace CsDealerTest
{
    public class TestTools
    {
        private static Card testAceSpades;
        private static Card testTwoDiamonds;
        private static Card testQueenHearts;
        private static Card testSevenClubs;

        private static List<Card> cards;
        private static List<string> names;

        private static Deck deck;
        private static Stack stack;

        [SetUp]
        public void Setup()
        {
            testAceSpades = new("Ace", "Spades");
            testTwoDiamonds = new("2", "Diamonds");
            testQueenHearts = new("Queen", "Hearts");
            testSevenClubs = new("7", "Clubs");

            cards = new() { testAceSpades, testTwoDiamonds, testQueenHearts, testSevenClubs };
            names = new() { "Ace of Spades", "2 of Diamonds", "Queen of Hearts", "7 of Clubs" };

            deck = new Deck();
            stack = new Stack(cards: cards);
        }

        public static void FindListHelper(Stack stack, List<int> gotCards)
        {
            Assert.AreEqual(gotCards.Count, 4);

            int i = 0;
            foreach(string name in names)
            {
                Assert.AreEqual(stack.Cards[gotCards[i]].name, name);
                i += 1;
            }
        }

        public static void GetListHelper(List<Card> left, List<Card> gotCards)
        {
            Assert.AreEqual(left.Count, 4);
            Assert.AreEqual(gotCards.Count, 0);

            int i = 0;
            foreach (string name in names)
            {
                Assert.AreEqual(gotCards[i].name, name);
                i += 1;
            }
        }

        [Test]
        public void TestBuildCards()
        {
            List<Card> cards = Tools.BuildCards();

            Assert.AreEqual(deck.Cards, cards);
            Assert.AreNotEqual(stack.Cards, cards);
        }

        [Test]
        public void TestSortCards()
        {
            Deck shuffledDeck = new();
            shuffledDeck.Shuffle();
            List<Card> sortedCards = Tools.SortCards(shuffledDeck.Cards);

            Assert.IsTrue(deck == sortedCards);
            Assert.IsFalse(deck == shuffledDeck);
        }

        [Test]
        public void TestCheckSorted()
        {
            bool result = Tools.CheckSorted(deck.Cards);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestCheckTerm()
        {
            bool result1 = Tools.CheckTerm(deck.Cards[0], "2 of Diamonds");
            bool result2 = Tools.CheckTerm(deck.Cards[0], "Ace of Spades");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestCompareStacks()
        {
            Deck otherDeck = new();

            bool result1 = Tools.CompareStacks(deck.Cards, otherDeck.Cards);

            Assert.IsTrue(result1);

            Deck shuffledDeck = new();
            shuffledDeck.Shuffle();

            if (!Tools.CheckSorted(shuffledDeck.Cards))
            {
                bool result2 = Tools.CompareStacks(deck.Cards, shuffledDeck.Cards, sorted: true);
                Assert.IsFalse(result2);
            }   
        }

        [Test]
        public void TestFindCardAbbrev()
        {
            List<int> found = Tools.FindCard(deck.Cards, "AS");
            int i = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(deck.Cards[i].name, "Ace of Spades");
            Assert.AreNotEqual(deck.Cards[i].name, "Two of Diamonds");
        }

        [Test]
        public void TestFindCardFull()
        {
            List<int> found = Tools.FindCard(deck.Cards, "Ace of Spades");
            int i = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(deck.Cards[i].name, "Ace of Spades");
            Assert.AreNotEqual(deck.Cards[i].name, "2 of Diamonds");
        }

        [Test]
        public void TestFindCardPartialValue()
        {
            List<int> found = Tools.FindCard(deck.Cards, "Ace");

            Assert.AreEqual(found.Count, 4);

            foreach (int i in found)
            {
                Assert.AreEqual(deck.Cards[i].value, "Ace");
                Assert.AreNotEqual(deck.Cards[i].value, "2");
            }
        }

        [Test]
        public void TestFindCardPartialSuit()
        {
            List<int> found = Tools.FindCard(deck.Cards, "Spades");

            Assert.AreEqual(found.Count, 13);

            foreach (int i in found)
            {
                Assert.AreEqual(deck.Cards[i].suit, "Spades");
                Assert.AreNotEqual(deck.Cards[i].suit, "Diamonds");
            }
        }

        [Test]
        public void TestFindCardLimit()
        {
            List<int> found = Tools.FindCard(deck.Cards, "Spades", limit: 1);

            Assert.AreEqual(found.Count, 1);
        }

        [Test]
        public void TestFindListFull()
        {
            List<int> found = Tools.FindList(stack.Cards, names);

            FindListHelper(stack, found);
        }
    }
}
