using NUnit.Framework;
using CsDealer;
using System.Collections.Generic;

namespace CsDealerTest
{
    class TestStack
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

            for (int i = 0; i < names.Count; i++)
            {
                Assert.AreEqual(stack.Cards[gotCards[i]].name, names[i]);
            }
        }

        public static void GetListHelper(List<Card> left, List<Card> gotCards)
        {
            Assert.AreEqual(gotCards.Count, 4);
            Assert.AreEqual(left.Count, 0);

            for (int i = 0; i < names.Count; i++)
            {
                Assert.AreEqual(gotCards[i].name, names[i]);
            }
        }

        [Test]
        public void TestAddTop()
        {
            stack.Add(testTwoDiamonds);
            Assert.AreEqual(stack.Cards[stack.Count - 1], testTwoDiamonds);
        }

        [Test]
        public static void TestAddBottom()
        {
            stack.Add(testAceSpades, Const.BOTTOM);
            Assert.AreEqual(stack.Cards[0], testAceSpades);
        }


    }
}
