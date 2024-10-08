﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsDealer
{
    public class Stack
    {
        public List<Card> _cards;
        public Dictionary<string, Dictionary<string, int>> ranks;

        public Stack(List<Card> cards = null, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (cards == null)
            {
                cards = new List<Card>();
            }

            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            _cards = cards;
            this.ranks = ranks;

            if (sort)
            {
                Sort(this.ranks);
            }
        }


        public static Stack operator +(Stack stack, Stack other)
        {
            Stack newStack;

            List<Card> cardList = new();

            cardList.AddRange(stack.Cards);

            cardList.AddRange(other.Cards);

            newStack = new Stack(cards: cardList);

            return newStack;
        }


        public static Stack operator +(Stack stack, List<Card> other)
        {
            Stack newStack;

            List<Card> cardList = new();

            cardList.AddRange(stack.Cards);

            List<Card> otherCards = other;
            cardList.AddRange(otherCards);

            newStack = new Stack(cards: cardList);

            return newStack;
        }


        public bool Contains(Card card)
        {
            List<string> reprList = new();
            List<Card> cards = Cards;

            foreach (Card c in cards)
            {
                reprList.Add(c.Repr());
            }

            return reprList.Contains(card.Repr());
        }


        public void Remove(int index)
        {
            if (index < 0)
            {
                index += Size;
            }

            Cards.RemoveAt(index);
        }


        public static bool operator ==(Stack leftStack, object rightObj)
        {
            return leftStack.Equals(rightObj);
        }

        public int Count
        {
            get
            {
                return Cards.Count;
            }
        }


        public static bool operator !=(Stack leftStack, object rightObj)
        {
            return !(leftStack.Equals(rightObj));
        }


        public override bool Equals(object rightObj)
        {
            List<Card> leftCards = Cards;
            List<Card> rightCards;

            if (rightObj.GetType() == typeof(Deck))
            {
                Deck rightCasted = (Deck)rightObj;

                if (Size == rightCasted.Size)
                {
                    rightCards = rightCasted.Cards;

                    for (int i = 0; i < Size; i++)
                    {
                        if (leftCards[i] != rightCards[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (rightObj.GetType() == typeof(Stack))
            {
                Stack rightCasted = (Stack)rightObj;

                if (Size == rightCasted.Size)
                {
                    rightCards = rightCasted.Cards;

                    for (int i = 0; i < Size; i++)
                    {
                        if (leftCards[i] != rightCards[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (rightObj is List<Card>)
            {
                rightCards = rightObj as List<Card>;

                if (Size == rightCards.Count)
                {
                    for (int i = 0; i < Size; i++)
                    {
                        if (leftCards[i] != rightCards[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public override int GetHashCode()
        {
            return Cards.GetHashCode();
        }


        public string Repr()
        {
            StringBuilder sb = new();

            sb.Append("Stack(cards=[");

            for (int i = 0; i < Cards.Count; i++)
            {
                sb.Append($"{Cards[i].Repr()}");

                if (i != Cards.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append("])");

            return sb.ToString();
        }


        public override string ToString()
        {
            StringBuilder sb = new();

            for (int i = 0; i < Cards.Count; i++)
            {
                sb.Append($"{Cards[i].name}");

                if (i != Cards.Count - 1)
                {
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }


        public void Add(Card cards, string end = Const.TOP)
        {
            Exception e = new ArgumentException("The 'end' parameter must be either"
                + $" {Const.TOP} or {Const.BOTTOM}");

            if (end == Const.TOP)
            {
                Cards.Add(cards);
            }
            else if (end == Const.BOTTOM)
            {
                Cards.Insert(0, cards);
            }
            else
            {
                throw e;
            }
        }


        public void Add(List<Card> cards, string end = Const.TOP)
        {
            Exception e = new ArgumentException("The 'end' parameter must be either"
                + $" {Const.TOP} or {Const.BOTTOM}");

            List<Card> cardList = cards;

            if (end == Const.TOP)
            {
                Cards = cardList.Concat(Cards).ToList();
            }
            else if (end == Const.BOTTOM)
            {
                Cards.AddRange(cardList);
            }
            else
            {
                throw e;
            }
        }


        public List<Card> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
            }
        }


        public Stack Deal(int num = 1, string end = Const.TOP)
        {
            if (num <= 0)
            {
                throw new ArgumentException("The 'num' parameter must be >= 1.");
            }

            Card[] dealtCards;
            int size = Size;
            Card card;

            if (num <= size)
            {
                dealtCards = new Card[num];
            }
            else
            {
                num = size;
                dealtCards = new Card[size];
            }

            if (size != 0)
            {
                for (int n = 0; n < num; n++)
                {
                    try
                    {
                        if (end == Const.TOP)
                        {
                            card = Cards[size - 1];
                            Cards.RemoveAt(size - 1);
                        }
                        else
                        {
                            card = Cards[0];
                            Cards.RemoveAt(0);
                        }

                        dealtCards[n] = card;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }

                return new Stack(cards: dealtCards.ToList());
            }
            else
            {
                return new Stack();
            }
        }


        public List<Card> Empty(bool returnCards = false)
        {
            if (!returnCards) 
            {
                Cards = new List<Card>();
                return null;
            }
            else
            {
                List<Card> cards = Cards.ToList();
                Cards = new List<Card>();
                return cards;
            }
        }


        public List<int> FindList(List<string> terms, int limit = 0, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<int> foundIndicies = new();
            int count = 0;
            string term;

            if (limit == 0)
            {
                for (int t = 0; t < terms.Count; t++)
                {
                    term = terms[t];

                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (Tools.CheckTerm(cards[i], term))
                        {
                            foundIndicies.Add(i);
                        }
                    }
                }
            }
            else
            {
                for (int t = 0; t < terms.Count; t++)
                {
                    term = terms[t];

                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (count < limit)
                        {
                            if (Tools.CheckTerm(cards[i], term))
                            {
                                foundIndicies.Add(i);
                                count += 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    count = 0;
                }
            }

            if (sort)
            {
                foundIndicies = Tools.SortCardIndicies(cards, foundIndicies, ranks);
            }

            return foundIndicies;
        }


        public List<Card> Get(int term, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> indices = new();

            int index = term;

            if (index < 0)
            {
                index += cards.Count;
            }

            gotCards.Add(cards[index]);

            indices.Add(index);

            for (int i = 0; i < cards.Count; i++)
            {
                if (indices.Contains(i))
                {
                    continue;
                }

                remainingCards.Add(cards[i]);
            }

            Cards = remainingCards;

            if (sort)
            {
                gotCards = Tools.SortCards(gotCards, ranks);
            }

            return gotCards;
        }


        public List<Card> Get(string term, int limit = 0, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> indices;

            indices = Tools.FindCard(cards, term, limit: limit);

            foreach (int index in indices)
            {
                gotCards.Add(cards[index]);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (indices.Contains(i))
                {
                    continue;
                }

                remainingCards.Add(cards[i]);
            }

            Cards = remainingCards;

            if (sort)
            {
                gotCards = Tools.SortCards(gotCards, ranks);
            }

            return gotCards;
        }


        public List<int> Find(string term, int limit = 0, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<int> foundIndicies = new();
            int count = 0;

            if (limit == 0)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (Tools.CheckTerm(cards[i], term))
                    {
                        foundIndicies.Add(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (count < limit)
                    {
                        if (Tools.CheckTerm(cards[i], term))
                        {
                            foundIndicies.Add(i);
                            count += 1;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }

            if (sort)
            {
                foundIndicies = Tools.SortCardIndicies(cards, foundIndicies, ranks);
            }

            return foundIndicies;
        }


        public List<Card> GetList(List<int> terms, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> allIndices = new();
            int index;

            for (int t = 0; t < terms.Count; t++)
            {
                index = terms[t];

                if (index < 0)
                {
                    index += cards.Count;
                }

                if (allIndices.Contains(index))
                {
                    continue;
                }

                gotCards.Add(cards[index]);
                allIndices.Add(index);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (allIndices.Contains(i))
                {
                    continue;
                }

                remainingCards.Add(cards[i]);
            }

            if (sort)
            {
                gotCards = Tools.SortCards(gotCards, ranks);
            }

            Cards = remainingCards;
            return gotCards;
        }


        public List<Card> GetList(List<string> terms, int limit = 0, bool sort = false, 
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> cards = Cards;
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> indices;
            List<int> allIndices = new();
            List<int> tempIndices = new();
            string term;

            for (int t = 0; t < terms.Count; t++)
            {
                term = terms[t];

                indices = Find(term, limit: limit);
                tempIndices.Clear();

                foreach (int index in indices)
                {
                    if (allIndices.Contains(index))
                    {
                        continue;
                    }

                    tempIndices.Add(index);
                }

                foreach (int index in tempIndices)
                {
                    gotCards.Add(cards[index]);
                }

                allIndices.AddRange(tempIndices);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (allIndices.Contains(i))
                {
                    continue;
                }

                remainingCards.Add(cards[i]);
            }

            if (sort)
            {
                gotCards = Tools.SortCards(gotCards, ranks);
            }

            Cards = remainingCards;
            return gotCards;
        }


        public void Insert(Card card, int index = -1)
        {
            int size = Size;

            if (index < 0 && size + index >= 0)
            {
                index += size;
            }
            else if (index < 0 || index >= size)
            {
                throw new ArgumentException("Parameter 'index' must be between"
                    + $" {-size} and {size - 1}, inclusive.");
            }

            List<Card> cards = new() { card };

            if (index == size - 1)
            {
                Cards.AddRange(cards);
            }
            else if (index == 0)
            {
                Cards = cards.Concat(Cards).ToList();
            }
            else
            {
                var splitCards = Split(index, false);
                List<Card> beforeCards = splitCards.Item1.Cards;
                List<Card> afterCards = splitCards.Item2.Cards;
                Cards = beforeCards.Concat(cards).Concat(afterCards).ToList();
            }
        }


        public void InsertList(List<Card> cards, int index = -1)
        {
            int size = Size;

            if (index < 0 && size + index >= 0)
            {
                index += size;
            }
            else if (index < 0 || index >= size)
            {
                throw new ArgumentException("Parameter 'index' must be between"
                    + $" {-size} and {size - 1}, inclusive.");
            }

            if (index == size - 1)
            {
                Cards.AddRange(cards);
            }
            else if (index == 0)
            {
                Cards = cards.Concat(Cards).ToList();
            }
            else
            {
                var splitCards = Split(index, false);
                List<Card> beforeCards = splitCards.Item1.Cards;
                List<Card> afterCards = splitCards.Item2.Cards;
                Cards = beforeCards.Concat(cards).Concat(afterCards).ToList();
            }
        }


        public bool IsSorted(Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = this.ranks;
            }

            return Tools.CheckSorted(Cards, ranks);
        }


        public void OpenCards(string filename = null)
        {
            Cards = Tools.OpenCards(filename);
        }


        public Card RandomCard(bool remove_ = false)
        {
            return Tools.RandomCard(Cards, remove_);
        }


        public void Reverse()
        {
            Cards.Reverse();
        }


        public void SaveCards(string filename = null)
        {
            Tools.SaveCards(Cards, filename);
        }


        public void SetCards(List<Card> cards)
        {
            Cards = cards;
        }


        private static readonly Random random = new();

        public void Shuffle(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                int n = Cards.Count;
                while (n > 1)
                {
                    n--;
                    int k = random.Next(n + 1);
                    Card card = Cards[k];
                    Cards[k] = Cards[n];
                    Cards[n] = card;
                }
            }
        }


        public int Size
        {
            get
            {
                return Cards.Count;
            }
        }


        public void Sort(Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = this.ranks;
            }

            Cards = Tools.SortCards(Cards, ranks);
        }


        public Tuple<Stack, Stack> Split(int index = 0, bool halve = true)
        {
            int size = Size;

            if (size > 1)
            {
                if (index < 0 && size + index >= 0)
                {
                    index += size;
                }
                else if (index < 0 || index >= size)
                {
                    throw new ArgumentException("Parameter 'index' must be between"
                        + $" {-size} and {size - 1}, inclusive.");
                }

                if (index == 0 && halve == true)
                {
                    int mid;

                    if (size % 2 == 0)
                    {
                        mid = size / 2;
                    }
                    else
                    {
                        mid = (size - 1) / 2;
                    }

                    return Tuple.Create(new Stack(cards: Cards.GetRange(0, mid)),
                        new Stack(cards: Cards.GetRange(mid, size - mid)));
                }
                else
                {
                    return Tuple.Create(new Stack(cards: Cards.GetRange(0, index)),
                        new Stack(cards: Cards.GetRange(index, size - index)));
                }
            }
            else
            {
                return Tuple.Create(new Stack(cards: Cards), new Stack());
            }
        }

        //===============================================================================
        // Helper Functions
        //===============================================================================

        public static Stack ConvertToStack(Deck deck)
        {
            return new Stack(deck.Cards);
        }
    }
}