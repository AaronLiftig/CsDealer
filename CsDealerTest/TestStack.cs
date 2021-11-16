using NUnit.Framework;
using CsDealer;
using System.Collections.Generic;
using System.Linq;
using System;

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

        private static Stack stack;
        private static Stack fullStack;
        private static Stack smallStack;

        [SetUp]
        public void Setup()
        {
            testAceSpades = new("Ace", "Spades");
            testTwoDiamonds = new("2", "Diamonds");
            testQueenHearts = new("Queen", "Hearts");
            testSevenClubs = new("7", "Clubs");

            cards = new() { testAceSpades, testTwoDiamonds, testQueenHearts, testSevenClubs };
            names = new() { "Ace of Spades", "2 of Diamonds", "Queen of Hearts", "7 of Clubs" };

            stack = new Stack();
            fullStack = new Stack(cards: Tools.BuildCards());
            smallStack = new Stack(cards: cards);
        }

        public static void FindListHelper(Stack stack, List<int> found)
        {
            Assert.AreEqual(found.Count, 4);

            for (int i = 0; i < names.Count; i++)
            {
                Assert.AreEqual(stack.Cards[found[i]].name, names[i]);
            }
        }

        public static void GetListHelper(List<Card> found)
        {
            Assert.AreEqual(found.Count, 4);

            for (int i = 0; i < names.Count; i++)
            {
                Assert.AreEqual(found[i].name, names[i]);
            }
        }

        [Test]
        public void TestAddTop()
        {
            stack.Add(testTwoDiamonds);
            Assert.AreEqual(stack.Cards[stack.Count - 1], testTwoDiamonds);
            Assert.AreNotEqual(stack.Cards[stack.Count - 1], testAceSpades);
        }

        [Test]
        public void TestAddBottom()
        {
            stack.Add(testAceSpades, Const.BOTTOM);
            Assert.AreEqual(stack.Cards[0], testAceSpades);
            Assert.AreNotEqual(stack.Cards[0], testTwoDiamonds);
        }

        [Test]
        public void TestAddPlusEquals()
        {
            stack += new List<Card> { testAceSpades };
            Assert.AreEqual(stack.Cards[stack.Count - 1], testAceSpades);
            Assert.AreNotEqual(stack.Cards[stack.Count - 1], testTwoDiamonds);
        }

        [Test]
        public void TestContains()
        {
            stack.Add(new List<Card> { testAceSpades });
            bool result = stack.Contains(testAceSpades);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestDealSingle()
        {
            Stack cards = fullStack.Deal();

            Assert.AreEqual(cards.Count, 1);
            Assert.That(cards.Cards[0], Is.InstanceOf<Card>());
        }

        [Test]
        public void TestDealMultiple()
        {
            Stack cards = fullStack.Deal(7);

            Assert.AreEqual(cards.Count, 7);
            Assert.That(cards.Cards[0], Is.InstanceOf<Card>());
        }

        [Test]
        public void TestRemoveItem()
        {
            Card card = fullStack.Cards[0];
            fullStack.Remove(0);
            bool result = fullStack.Contains(card);

            Assert.IsFalse(result);
        }

        [Test]
        public void TestEmpty()
        {
            List<Card> cards = fullStack.Empty(true);

            Assert.AreEqual(cards.Count, 52);
            Assert.AreEqual(fullStack.Count, 0);
        }

        [Test]
        public void TestEquals()
        {
            Stack otherStack = new(cards: Tools.BuildCards());

            bool result = fullStack == otherStack;

            Assert.IsTrue(result);
        }

        [Test]
        public void TestFindAbbrev()
        {
            List<int> found = fullStack.Find("AS");
            int i = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(fullStack.Cards[i].name, "Ace of Spades");
        }

        [Test]
        public void TestFindFull()
        {
            List<int> found = fullStack.Find("Ace of Spades");
            int i = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(fullStack.Cards[i].name, "Ace of Spades");
        }

        [Test]
        public void TestFindPartialValue()
        {
            List<int> found = fullStack.Find("Ace");

            Assert.AreEqual(found.Count, 4);

            foreach (int i in found)
            {
                Assert.AreEqual(fullStack.Cards[i].value, "Ace");
            }
        }

        [Test]
        public void TestFindPartialSuit()
        {
            List<int> found = fullStack.Find("Spades");

            Assert.AreEqual(found.Count, 13);

            foreach (int i in found)
            {
                Assert.AreEqual(fullStack.Cards[i].suit, "Spades");
            }
        }

        [Test]
        public void TestFindLimit()
        {
            List<int> found = fullStack.Find("Spades", limit: 1);

            Assert.AreEqual(found.Count, 1);
        }

        [Test]
        public void TestFindListFull()
        {
            List<int> found = smallStack.FindList(names);

            FindListHelper(smallStack, found);
        }

        [Test]
        public void TestFindListAbbrev()
        {
            List<string> abbrevList = new() { "AS", "2D", "QH", "7C" };

            List<int> found = smallStack.FindList(abbrevList);

            FindListHelper(smallStack, found);
        }

        [Test]
        public void TestFindListPartialValue()
        {
            List<string> partialList = new() { "Ace", "2", "Queen", "7" };

            List<int> found = smallStack.FindList(partialList);

            FindListHelper(smallStack, found);
        }

        [Test]
        public void TestFindListPartialSuit()
        {
            List<string> partialList = new() { "Spades", "Diamonds", "Hearts", "Clubs" };

            List<int> found = smallStack.FindList(partialList);

            FindListHelper(smallStack, found);
        }

        [Test]
        public void TestFindListMixed()
        {
            List<string> mixedList = new() { "AS", "2 of Diamonds", "Hearts", "7" };

            List<int> found = smallStack.FindList(mixedList);

            FindListHelper(smallStack, found);
        }

        [Test]
        public void TestFindListLimit()
        {
            List<int> found = smallStack.FindList(new() { "Spades" }, limit: 1);

            Assert.AreEqual(found.Count, 1);
        }

        [Test]
        public void TestGetCardAbbrev()
        {
            List<Card> found = fullStack.Get("AS");
            Card card = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(card.name, "Ace of Spades");
        }

        [Test]
        public void TestGetCardFull()
        {
            List<Card> found = fullStack.Get("Ace of Spades");
            Card card = found[0];

            Assert.AreEqual(found.Count, 1);
            Assert.AreEqual(card.name, "Ace of Spades");
        }

        [Test]
        public void TestGetCardPartialValue()
        {
            List<Card> found = fullStack.Get("Ace");

            Assert.AreEqual(found.Count, 4);

            foreach (Card card in found)
            {
                Assert.AreEqual(card.value, "Ace");
            }
        }

        [Test]
        public void TestGetCardPartialSuit()
        {
            List<Card> found = fullStack.Get("Spades");

            Assert.AreEqual(found.Count, 13);

            foreach (Card card in found)
            {
                Assert.AreEqual(card.suit, "Spades");
            }
        }

        [Test]
        public void TestGetCardLimit()
        {
            List<Card> found = fullStack.Get("Spades", limit: 1);

            Assert.AreEqual(found.Count, 1);
        }

        [Test]
        public void TestGetListFull()
        {
            List<Card> found = smallStack.GetList(names);

            GetListHelper(found);
        }

        [Test]
        public void TestGetListAbbrev()
        {
            List<string> abbrevList = new() { "AS", "2D", "QH", "7C" };

            List<Card> found = smallStack.GetList(abbrevList);

            GetListHelper(found);
        }

        [Test]
        public void TestGetListPartialValue()
        {
            List<string> partialList = new() { "Ace", "2", "Queen", "7" };

            List<Card> found = smallStack.GetList(partialList);

            GetListHelper(found);
        }

        [Test]
        public void TestGetListPartialSuit()
        {
            List<string> partialList = new() { "Spades", "Diamonds", "Hearts", "Clubs" };

            List<Card> found = smallStack.GetList(partialList);

            GetListHelper(found);
        }

        [Test]
        public void TestGetListMixed()
        {
            List<string> mixedList = new() { "AS", "2 of Diamonds", "Hearts", "7" };

            List<Card> found = smallStack.GetList(mixedList);

            GetListHelper(found);
        }

        [Test]
        public void TestGetListLimit()
        {
            List<Card> found = smallStack.GetList(new() { "Spades" }, limit: 1);

            Assert.AreEqual(found.Count, 1);
        }

        [Test] // This test is not necessary with statically typed language
        public void TestGetItem()
        {
            Card card = fullStack.Cards[0];

            Assert.That(card, Is.InstanceOf<Card>());

            card = fullStack.Cards[fullStack.Count - 1];

            Assert.That(card, Is.InstanceOf<Card>());
        }

        [Test]
        public void TestInsert()
        {
            fullStack.Insert(testAceSpades, 1);

            Assert.AreEqual(fullStack.Cards[1], testAceSpades);
        }

        [Test]
        public void TestInsertList()
        {
            fullStack.InsertList(cards, 1);

            List<Card> stackSlice = fullStack.Cards.GetRange(1, 4);

            Assert.AreEqual(stackSlice, cards);
        }

        [Test]
        public void TestIter()
        {
            foreach (Card card in fullStack.Cards)
            {
                Assert.That(card, Is.InstanceOf<Card>());
            }
        }

        [Test]
        public void TestCount()
        {
            int result = fullStack.Count;

            Assert.AreEqual(result, 52);
        }

        [Test]
        public void TestNotEquals()
        {
            bool result1 = fullStack != stack;
            bool result2 = fullStack != fullStack;

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public void TestOpenCards()
        {
            List<int> indices = new() { 0, 1, 2, 3 };
            stack.OpenCards("CardsSave.txt");

            FindListHelper(stack, indices);
        }

        [Test]
        public void TestRandomCard()
        {
            Card card = fullStack.RandomCard();

            Assert.That(card, Is.InstanceOf<Card>());
        }

        [Test]
        public void TestRepr()
        {
            string result = stack.Repr();

            Assert.AreEqual(result, "Stack(cards=[])");
        }

        [Test]
        public void TestReverse()
        {
            Stack cardsReversedX = new(cards: smallStack.Cards);
            cardsReversedX.Cards.Reverse();

            smallStack.Reverse();
            Stack cardsReversedY = new(cards: smallStack.Cards);

            Assert.AreEqual(cardsReversedX, cardsReversedY);
        }

        [Test]
        public void TestSaveCards()
        {
            List<string> names = new() { "Ace Spades", "2 Diamonds", "Queen Hearts", "7 Clubs" };

            smallStack.SaveCards("CardsSave.txt");

            using System.IO.StreamReader file = new("CardsSave.txt");
            string line;
            int i = 0;

            while ((line = file.ReadLine()) != null)
            {
                Assert.AreEqual(line, names[i]);
                i++;
            }
        }

        [Test]
        public void TestSetCards()
        {
            stack.SetCards(cards);

            Assert.AreEqual(stack.Cards.ToList(), cards);
        }

        [Test]
        public void TestSetItem()
        {
            fullStack.Cards[0] = testAceSpades;

            Assert.AreEqual(fullStack.Cards[0], testAceSpades);
        }

        [Test]
        public void TestShuffle()
        {
            List<Card> cardsBefore = fullStack.Cards.ToList();
            fullStack.Shuffle();
            List<Card> cardsAfter = fullStack.Cards.ToList();

            Assert.AreNotEqual(cardsBefore, cardsAfter);
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(fullStack.Size, 52);
        }

        [Test]
        public void TestSort()
        {
            List<Card> ordered = new() { testTwoDiamonds,  testSevenClubs, testQueenHearts, testAceSpades };

            smallStack.Sort();

            Assert.AreEqual(smallStack.Cards.ToList(), ordered);
        }

        [Test]
        public void TestSplit()
        {
            var (s1, s2) = smallStack.Split();

            Assert.AreEqual(s1.Cards.ToList(), smallStack.Cards.GetRange(0, 2));
            Assert.AreEqual(s2.Cards.ToList(), smallStack.Cards.GetRange(2, smallStack.Count - 2));
        }

        [Test]
        public void TestToString()
        {
            string cards = "Ace of Spades\r\n2 of Diamonds\r\nQueen of Hearts\r\n7 of Clubs";
            string result = smallStack.ToString();

            Assert.AreEqual(result, cards);
        }
    }
}
