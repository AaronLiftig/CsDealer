using CsDealer;
using NUnit.Framework;
using System.Collections.Generic;

namespace CsDealerTest
{
    public class TestDeck
    {
        private static Deck deck;
        private static Deck emptyDeck;

        [SetUp]
        public void Setup()
        {
            deck = new Deck();
            emptyDeck = new Deck(build: false);
        }

        [Test]
        public void TestAdd()
        {
            emptyDeck += deck;

            Assert.AreEqual(emptyDeck.Cards, deck.Cards);
        }

        [Test]
        public void TestBuild()
        {
            emptyDeck.Build();

            Assert.AreEqual(emptyDeck.Count, 52);
        }

        [Test]
        public void TestDeal()
        {
            List<string> cardNames = new() { "Ace of Spades", "Ace of Hearts", "Ace of Clubs", "Ace of Diamonds" };

            Stack dealtCards = deck.Deal(4);
            
            for (int i = 0; i < cardNames.Count; i++)
            {
                Assert.AreEqual(dealtCards.Cards[i].name, cardNames[i]);
            }
        }
    }
}
