using CsDealer;
using NUnit.Framework;

namespace CsDealerTest
{
    public class TestCard
    {
        private static Card testAceSpades;
        private static Card testTwoDiamonds;

        [SetUp]
        public void Setup()
        {
            testAceSpades = new("Ace", "Spades");
            testTwoDiamonds = new("2", "Diamonds");
        }

        [Test]
        public void TestCardAbbrev()
        {
            string abbrev = Card.CardAbbrev("Ace", "Spades");

            Assert.AreEqual(abbrev, "AS");
            Assert.AreNotEqual(abbrev, "2D");
        }

        [Test]
        public void TestCardName()
        {
            string name = Card.CardName("Ace", "Spades");

            Assert.AreEqual(name, "Ace of Spades");
            Assert.AreNotEqual(name, "Two of Diamonds");
        }

        [Test]
        public void TestValue()
        {
            Assert.AreEqual(testAceSpades.value, "Ace");
            Assert.AreNotEqual(testAceSpades.value, "2");
        }

        [Test]
        public void TestSuit()
        {
            Assert.AreEqual(testAceSpades.suit, "Spades");
            Assert.AreNotEqual(testAceSpades.suit, "Diamonds");
        }

        [Test]
        public void TestAbbrev()
        {
            Assert.AreEqual(testAceSpades.abbrev, "AS");
            Assert.AreNotEqual(testAceSpades.abbrev, "2D");
        }

        [Test]
        public void TestName()
        {
            Assert.AreEqual(testAceSpades.name, "Ace of Spades");
            Assert.AreNotEqual(testAceSpades.name, "2 of Diamonds");
        }

        [Test]
        public void TestEquals()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            Assert.AreEqual(testAceSpades, aceSpades);
            Assert.AreNotEqual(testAceSpades, twoDiamonds);
        }

        [Test]
        public void TestEqualsFunction()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            bool result1 = testAceSpades.Equals(aceSpades);
            bool result2 = testAceSpades.Equals(twoDiamonds);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestGreaterThanOrEqual()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            Assert.IsTrue(testAceSpades >= aceSpades);
            Assert.IsTrue(testAceSpades >= twoDiamonds);
            Assert.IsFalse(testTwoDiamonds >= aceSpades);
        }

        [Test]
        public void TestGreaterThanOrEqualFunction()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            bool result1 = testAceSpades.GreaterThanOrEqual(aceSpades);
            bool result2 = testAceSpades.GreaterThanOrEqual(twoDiamonds);
            bool result3 = testTwoDiamonds.GreaterThanOrEqual(aceSpades);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
        }

        [Test]
        public void TestGreaterThan()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            Assert.IsTrue(testAceSpades > twoDiamonds);
            Assert.IsFalse(testAceSpades > aceSpades);
        }

        [Test]
        public void TestGreaterThanFunction()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");
  
            bool result1 = testAceSpades.GreaterThan(twoDiamonds);
            bool result2 = testAceSpades.GreaterThan(aceSpades);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestLessThanOrEqual()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            Assert.IsTrue(testAceSpades <= aceSpades);
            Assert.IsTrue(testTwoDiamonds <= aceSpades);
            Assert.IsFalse(testAceSpades <= twoDiamonds);
        }

        [Test]
        public void TestLessThanOrEqualFunction()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            bool result1 = testAceSpades.LessThanOrEqual(aceSpades);           
            bool result2 = testTwoDiamonds.LessThanOrEqual(aceSpades);
            bool result3 = testAceSpades.LessThanOrEqual(twoDiamonds);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
        }

        [Test]
        public void TestLessThan()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            Assert.IsTrue(testTwoDiamonds < aceSpades);
            Assert.IsFalse(testTwoDiamonds < twoDiamonds);
        }

        [Test]
        public void TestLessThanFunction()
        {
            Card aceSpades = new("Ace", "Spades");
            Card twoDiamonds = new("2", "Diamonds");

            bool result1 = testTwoDiamonds.LessThan(aceSpades);
            bool result2 = testTwoDiamonds.LessThan(twoDiamonds);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestNotEqual()
        {
            Card twoDiamonds = new("2", "Diamonds");

            Assert.IsTrue(testAceSpades != twoDiamonds);
            Assert.IsFalse(testTwoDiamonds != twoDiamonds);
        }

        [Test]
        public void TestNotEqualFunction()
        {
            Card twoDiamonds = new("2", "Diamonds");

            bool result1 = testAceSpades.NotEqual(twoDiamonds);
            bool result2 = testTwoDiamonds.NotEqual(twoDiamonds);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestToString()
        {
            string result = testTwoDiamonds.ToString();

            Assert.AreEqual(result, "2 of Diamonds");
        }
    }
}