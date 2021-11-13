using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsDealer
{
    public class Deck : Stack
    {
        public bool jokers;
        public int numJokers;
        public bool build;
        public bool rebuild;
        public bool reshuffle;
        public new Dictionary<string, Dictionary<string, int>> ranks;
        public int decksUsed;

        public Deck(List<Card> cards = null, bool jokers = false, int numJokers = 0,
            bool build = true, bool rebuild = false, bool reshuffle = false,
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
            this.jokers = jokers;
            this.numJokers = numJokers;
            this.rebuild = rebuild;
            this.reshuffle = reshuffle;
            this.ranks = ranks;
            decksUsed = 0;

            if (build)
            {
                Build();
            }
        }


        public static Deck operator +(Deck deck, object other)
        {
            Deck newDeck;

            if (other.GetType() == typeof(Deck))
            {
                List<Card> cardList = new();

                cardList.AddRange(deck.Cards);

                cardList.AddRange(((Deck)other).Cards);

                newDeck = new Deck(cards: cardList, build: false);
            }
            else if (other.GetType() == typeof(Stack))
            {
                List<Card> cardList = new();

                cardList.AddRange(deck.Cards);

                cardList.AddRange(((Stack)other).Cards);

                newDeck = new Deck(cards: cardList, build: false);
            }
            else if (other is List<Card>)
            {
                List<Card> cardList = new();

                cardList.AddRange(deck.Cards);

                List<Card> otherCards = other as List<Card>;
                cardList.AddRange(otherCards);

                newDeck = new Deck(cards: cardList, build: false);
            }
            else
            {
                throw new System.ArgumentException("Object on the right side of '+' must be of"
                    + " type Stack or Deck or be a list of Card instances");
            }

            return newDeck;
        }


        public new string Repr()
        {
            StringBuilder sb = new();

            sb.Append("Deck(cards=[");

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


        public void Build(bool jokers = false, int numJokers = 0)
        {
            if (jokers == false)
            {
                jokers = this.jokers;
            }

            if (numJokers == 0)
            {
                numJokers = this.numJokers;
            }

            decksUsed += 1;

            Cards.AddRange(Tools.BuildCards(jokers, numJokers));
        }


        public Stack Deal(int num = 1, bool rebuild = false, bool shuffle = false,
            string end = Const.TOP)
        {
            if (num <= 0)
            {
                throw new System.ArgumentException("The 'num' parameter must be >= 1.");
            }

            Card[] dealtCards;
            int _num = num;
            int size = Size;

            if (!rebuild)
            {
                rebuild = this.rebuild;
            }

            if (!shuffle)
            {
                shuffle = reshuffle;
            }

            if (rebuild || num <= size)
            {
                dealtCards = new Card[num];
            }
            else
            {
                dealtCards = new Card[size];
            }

            Card card;
            int n;

            while (num > 0)
            {
                n = _num - num;

                try
                {
                    if (end == Const.TOP)
                    {
                        card = Cards[Size - 1];
                        Cards.RemoveAt(Size - 1);
                    }
                    else
                    {
                        card = Cards[0];
                        Cards.RemoveAt(0);
                    }

                    dealtCards[n] = card;
                    num -= 1;
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    if (Size == 0)
                    {
                        if (rebuild)
                        {
                            Build();

                            if (shuffle)
                            {
                                Shuffle();
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return new Stack(cards: dealtCards.ToList());
        }

        //===============================================================================
        // Helper Functions
        //===============================================================================

        public static Deck ConvertToDeck(Stack stack)
        {
            List<Card> cardStack = stack.Cards;

            return new Deck(cardStack);
        }
    }
}