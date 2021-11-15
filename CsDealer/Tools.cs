using System;
using System.Collections.Generic;
using System.Linq;

namespace CsDealer
{
    public class Tools // Utility Functions
    {
        public static List<Card> BuildCards(bool jokers = false, int numJokers = 0)
        {
            List<Card> newDeck = new();

            if (jokers)
            {
                for (int i = 0; i < numJokers; i++)
                {
                    newDeck.Add(new Card("Joker", null));
                }
            }

            foreach (string value in Const.VALUES)
            {
                foreach (string suit in Const.SUITS)
                {
                    newDeck.Add(new Card(value, suit));
                }
            }

            return newDeck;
        }


        public static bool CheckSorted(List<Card> cards, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            List<Card> sortedCards = SortCards(cards, ranks);
            List<Card> reversedCards = cards.ToList();
            reversedCards.Reverse();

            if (cards.SequenceEqual(sortedCards) || reversedCards.SequenceEqual(sortedCards))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool CheckTerm(Card card, string term)
        {
            List<string> checkList = new() { card.name, card.suit, card.value, card.abbrev };

            checkList = checkList.ConvertAll(t => t.ToLower());
            term = term.ToLower();

            foreach (string t in checkList)
            {
                if (t.Equals(term))
                {
                    return true;
                }
            }

            return false;
        }


        public static bool CompareStacks(List<Card> cardsX, List<Card> cardsY, bool sorted = false)
        {
            if (cardsX.Count == cardsY.Count)
            {
                if (!sorted)
                {
                    cardsX = SortCards(cardsX, Const.DEFAULT_RANKS);
                    cardsY = SortCards(cardsY, Const.DEFAULT_RANKS);
                }

                for (int i = 0; i < cardsX.Count; i++)
                {
                    if (cardsX[i] != cardsY[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }


        public static List<int> FindCard(List<Card> cards, string term, int limit = 0,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<int> foundIndicies = new();
            int count = 0;

            if (limit == 0)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (CheckTerm(cards[i], term))
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
                        if (CheckTerm(cards[i], term))
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
                foundIndicies = SortCardIndicies(cards, foundIndicies, ranks);
            }

            return foundIndicies;
        }


        public static List<int> FindList(List<Card> cards, List<string> terms, int limit = 0,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
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
                        if (CheckTerm(cards[i], term))
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
                            if (CheckTerm(cards[i], term))
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
                foundIndicies = SortCardIndicies(cards, foundIndicies, ranks);
            }

            return foundIndicies;
        }


        public static Tuple<List<Card>, List<Card>> GetCard(List<Card> cards, int term,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
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

            if (sort)
            {
                gotCards = SortCards(gotCards, ranks: ranks);
            }

            return Tuple.Create(remainingCards, gotCards);
        }


        public static Tuple<List<Card>, List<Card>> GetCard(List<Card> cards, string term,
            int limit = 0, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> indices;

            indices = FindCard(cards, term, limit: limit);

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

            if (sort)
            {
                gotCards = SortCards(gotCards, ranks: ranks);
            }

            return Tuple.Create(remainingCards, gotCards);
        }


        public static Tuple<List<Card>, List<Card>> GetList(List<Card> cards, List<int> terms,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
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
                gotCards = SortCards(gotCards, ranks);
            }

            return Tuple.Create(remainingCards, gotCards);
        }


        public static Tuple<List<Card>, List<Card>> GetList(List<Card> cards, List<string> terms,
            int limit = 0, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> allIndices = new();
            List<int> tempIndices = new();
            string term;

            for (int t = 0; t < terms.Count; t++)
            {
                term = terms[t];

                List<int> indices = FindCard(cards, term, limit: limit);
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
                gotCards = SortCards(gotCards, ranks);
            }

            return Tuple.Create(remainingCards, gotCards);
        }


        public static List<Card> OpenCards(string filename = null)
        {
            if (filename == null)
            {
                filename = $"cards-{DateTime.Now:yyyyMMdd}.txt";
            }

            List<string> cardData = new();

            using (System.IO.StreamReader file = new(filename))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    cardData.Add(line);
                }
            }

            List<Card> cards = new();

            for (int i = 0; i < cardData.Count; i++)
            {
                string[] card = cardData[i].Split(" ");
                cards[i] = new Card(card[0], card[1]);
            }

            return cards;
        }


        public static Card RandomCard(List<Card> cards, bool remove_ = false)
        {
            Random random = new();

            if (!remove_)
            {
                int randIndex = random.Next(cards.Count);
                return cards[randIndex];
            }
            else
            {
                int randIndex = random.Next(cards.Count);
                Card card = cards[randIndex];
                cards.RemoveAt(randIndex);
                return card;
            }
        }


        public static void SaveCards(List<Card> cards, string filename = null)
        {
            if (filename == null)
            {
                filename = $"cards-{DateTime.Now:yyyyMMdd}.txt";
            }

            string cardReprs;
            List<string> cardReprsList = new();

            for (int i = 0; i < cards.Count; i++)
            {
                cardReprs = $"{cards[i].value} {cards[i].suit}";

                if (i != cards.Count - 1)
                {
                    cardReprs += Environment.NewLine;
                }

                cardReprsList.Add(cardReprs);
            }

            using System.IO.StreamWriter file = new(filename);
            foreach (string line in cardReprsList)
            {
                file.WriteLine(line);
            }
        }


        public static List<int> SortCardIndicies(List<Card> cards, List<int> indices,
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (ranks.ContainsKey("suits"))
            {
                indices = (List<int>)indices.OrderBy(x => cards[x].suit != null ? ranks["suits"][cards[x].suit] : 0);
            }

            if (ranks.ContainsKey("values"))
            {
                indices = (List<int>)indices.OrderBy(x => ranks["values"][cards[x].value]);
            }

            return indices;
        }


        public static List<Card> SortCards(List<Card> cards,
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (ranks.ContainsKey("suits"))
            {
                cards = cards.OrderBy(x => x.suit != null ? ranks["suits"][x.suit] : 0).ToList();
            }

            if (ranks.ContainsKey("values"))
            {
                cards = cards.OrderBy(x => ranks["values"][x.value]).ToList();
            }

            return cards;
        }
    }
}