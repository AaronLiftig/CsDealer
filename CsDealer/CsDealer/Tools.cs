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


        public static bool CheckSorted(List<Card> cards,
            Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            List<Card> sortedCards = SortCards(cards, ranks);
            List<Card> copiedCards = cards.ToList();
            copiedCards.Reverse();
            List<Card> reversedCards = copiedCards;

            if (cards.SequenceEqual(sortedCards) || reversedCards.SequenceEqual(sortedCards))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool CheckTerm<T>(Card card, T term)
        {
            List<T> checkList = new();

            if (term is string)
            { // There may an issue with the types in these string arrays
                string[] checkParts = new string[] {card.name, card.suit, card.value, card.abbrev};
                checkParts = Array.ConvertAll(checkParts, t => t.ToLower());

                string castedTerm = (string)(object)term;
                castedTerm = castedTerm.ToLower();
                term = (T)Convert.ChangeType(castedTerm, typeof(T));

                checkList = checkParts.Cast<T>().ToList();
            }
            else if (term is char)
            {
                char[] checkParts = new char[] { card.suit[0], card.value[0] };
                checkParts = Array.ConvertAll(checkParts, t => char.ToLower(t));

                char castedTerm = (char)(object)term;
                castedTerm = char.ToLower(castedTerm);
                term = (T)Convert.ChangeType(castedTerm, typeof(T));

                checkList = checkParts.Cast<T>().ToList();
            }
            else
            {
                return false;
            }

            foreach (T t in checkList)
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


        public static List<int> Find(List<Card> cards, object term, int limit = 0,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<int> foundIndicies = new();
            int count = 0;

            if (term is string)
            {
                term = (string)term;
            }
            else if (term is char)
            {
                term = (char)term;
            }
            else
            {
                throw new ArgumentException($"The term {term} is not of type string or char.");
            }

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


        public static List<int> FindList(List<Card> cards, List<object> terms, int limit = 0,
            bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<int> foundIndicies = new();
            int count = 0;
            object term;

            if (limit == 0)
            {
                for (int t = 0; t < terms.Count; t++)
                {
                    term = terms[t];

                    if (term is string)
                    {
                        term = (string)term;
                    }
                    else if (term is char)
                    {
                        term = (char)term;
                    }
                    else
                    {
                        throw new ArgumentException($"The term '{term}' in the {t} index in 'terms' list"
                            + " is not of type string or char.");
                    }

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

                    if (term is string)
                    {
                        term = (string)term;
                    }
                    else if (term is char)
                    {
                        term = (char)term;
                    }
                    else
                    {
                        throw new ArgumentException($"The term '{term}' in the {t} index in 'terms' list"
                            + " is not of type string or char.");
                    }

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


        public static Tuple<List<Card>, List<Card>> GetCard(List<Card> cards, object term,
            int limit = 0, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> indices = new();

            if (term is int)
            {
                int index = (int)term;

                if (index < 0)
                {
                    index += cards.Count;
                }

                gotCards.Add(cards[index]);

                indices.Add(index);
            }
            else if (term is string || term is char)
            {
                indices = Find(cards, term, limit: limit);

                foreach (int index in indices)
                {
                    gotCards.Add(cards[index]);
                }
            }
            else
            {
                throw new ArgumentException($"The term '{term}' is not of type string, char, or int.");
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


        public static Tuple<List<Card>, List<Card>> GetList(List<Card> cards, List<object> terms,
            int limit = 0, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
        // Has additional functionality that terms list can be mixed with indicies and card descriptions
        {
            List<Card> gotCards = new();
            List<Card> remainingCards = new();
            List<int> allIndices = new();
            List<int> tempIndices = new();
            object term;

            for (int t = 0; t < terms.Count; t++)
            {
                term = terms[t];

                if (term is int)
                {
                    int index = (int)term;

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
                else if (term is string || term is char)
                {
                    List<int> indices = Find(cards, term, limit: limit);
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
                else
                {
                    throw new ArgumentException($"The term '{term}' in index {t} is not of type string,"
                        + " char, or int.");
                }
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
                string[] card = cardData[i].Split(' ');
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
                cards = (List<Card>)cards.OrderBy(x => x != null ? ranks["suits"][x.suit] : 0);
            }

            if (ranks.ContainsKey("values"))
            {
                cards = (List<Card>)cards.OrderBy(x => ranks["values"][x.value]);
            }

            return cards;
        }
    }
}