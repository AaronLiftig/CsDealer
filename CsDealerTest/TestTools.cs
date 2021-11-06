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

        public void FindListHelper(Stack stack, List<int> gotCards)
        {
            Assert.AreEqual(gotCards.Count, 4);

            int i = 0;
            foreach(string name in names)
            {
                Assert.AreEqual(stack.Cards[gotCards[i]].name, name);
                i += 1;
            }
        }

        public void GetListHelper(List<Card> left, List<Card> gotCards)
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
    }
}
