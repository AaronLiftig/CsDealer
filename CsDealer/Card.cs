using System;
using System.Collections.Generic;

namespace CsDealer
{
    public class Card
    {
        public string value;
        public string suit;
        public string abbrev;
        public string name;

        public Card(string value, string suit)
        {
            this.value = char.ToUpper(value[0]) + value[1..].ToLower();

            if (suit != null)
            {
                this.suit = char.ToUpper(suit[0]) + suit[1..].ToLower();
            }
            else
            {
                this.suit = suit;
            }

            abbrev = CardAbbrev(this.value, this.suit);
            name = CardName(this.value, this.suit);
        }


        public static bool operator ==(Card leftCard, Card rightCard)
        {
            return leftCard.value == rightCard.value && leftCard.suit == rightCard.suit;
        }


        public static bool operator !=(Card leftCard, Card rightCard)
        {
            return leftCard.value != rightCard.value || leftCard.suit != rightCard.suit;
        }


        public static bool operator >=(Card leftCard, Card rightCard)
        {
            return Const.DEFAULT_RANKS["values"][leftCard.value] > Const.DEFAULT_RANKS["values"][rightCard.value]
                || (
                    Const.DEFAULT_RANKS["values"][leftCard.value] >= Const.DEFAULT_RANKS["values"][rightCard.value]
                    &&
                    Const.DEFAULT_RANKS["suits"][leftCard.suit] >= Const.DEFAULT_RANKS["suits"][rightCard.suit]
                    );
        }


        public static bool operator >(Card leftCard, Card rightCard)
        {
            return Const.DEFAULT_RANKS["values"][leftCard.value] > Const.DEFAULT_RANKS["values"][rightCard.value]
                || (
                    Const.DEFAULT_RANKS["values"][leftCard.value] >= Const.DEFAULT_RANKS["values"][rightCard.value]
                    &&
                    Const.DEFAULT_RANKS["suits"][leftCard.suit] > Const.DEFAULT_RANKS["suits"][rightCard.suit]
                    );
        }


        public static bool operator <=(Card leftCard, Card rightCard)
        {
            return Const.DEFAULT_RANKS["values"][leftCard.value] < Const.DEFAULT_RANKS["values"][rightCard.value]
                || (
                    Const.DEFAULT_RANKS["values"][leftCard.value] <= Const.DEFAULT_RANKS["values"][rightCard.value]
                    &&
                    Const.DEFAULT_RANKS["suits"][leftCard.suit] <= Const.DEFAULT_RANKS["suits"][rightCard.suit]
                    );
        }


        public static bool operator <(Card leftCard, Card rightCard)
        {
            return Const.DEFAULT_RANKS["values"][leftCard.value] < Const.DEFAULT_RANKS["values"][rightCard.value]
                || (
                    Const.DEFAULT_RANKS["values"][leftCard.value] <= Const.DEFAULT_RANKS["values"][rightCard.value]
                    &&
                    Const.DEFAULT_RANKS["suits"][leftCard.suit] < Const.DEFAULT_RANKS["suits"][rightCard.suit]
                    );
        }


        public override int GetHashCode()
        {
            Tuple<string, string> cardTuple = new(value, suit);

            return cardTuple.GetHashCode();
        }


        public string Repr()
        {
            return $"Card(value={value}, suit={suit})";
        }


        public override string ToString()
        {
            return $"{name}";
        }


        public override bool Equals(object other)
        {
            Dictionary<string, Dictionary<string, int>> ranks = Const.DEFAULT_RANKS;

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] == ranks["values"][_other.value]
                        &&
                        ranks["suits"][suit] == ranks["suits"][_other.suit];
                }
                else
                {
                    return ranks["values"][value] == ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool Equals(object other, Dictionary<string, Dictionary<string, int>> ranks)
        {
            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] == ranks["values"][_other.value]
                        &&
                        ranks["suits"][suit] == ranks["suits"][_other.suit];
                }
                else
                {
                    return ranks["values"][value] == ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool GreaterThanOrEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] >= ranks["values"][_other.value]
                        || (
                            ranks["suits"][suit] >= ranks["suits"][_other.suit]
                            &&
                            ranks["suits"][suit] >= ranks["suits"][_other.suit]
                            );
                }
                else
                {
                    return ranks["values"][value] >= ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool GreaterThan(object other, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] > ranks["values"][_other.value]
                        || (
                            ranks["suits"][suit] >= ranks["suits"][_other.suit]
                            &&
                            ranks["suits"][suit] > ranks["suits"][_other.suit]
                            );
                }
                else
                {
                    return ranks["values"][value] > ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool LessThanOrEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] <= ranks["values"][_other.value]
                        || (
                            ranks["suits"][suit] <= ranks["suits"][_other.suit]
                            &&
                            ranks["suits"][suit] <= ranks["suits"][_other.suit]
                            );
                }
                else
                {
                    return ranks["values"][value] <= ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool LessThan(object other, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] < ranks["values"][_other.value]
                        || (
                            ranks["suits"][suit] <= ranks["suits"][_other.suit]
                            &&
                            ranks["suits"][suit] < ranks["suits"][_other.suit]
                            );
                }
                else
                {
                    return ranks["values"][value] < ranks["values"][_other.value];
                }
            }
            else
            {
                return false;
            }
        }


        public bool NotEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)
        {
            if (ranks == null)
            {
                ranks = Const.DEFAULT_RANKS;
            }

            if (other is Card)
            {
                Card _other = (Card)other;

                if (ranks.ContainsKey("suits"))
                {
                    return ranks["values"][value] != ranks["values"][_other.value]
                        &&
                        ranks["suits"][suit] != ranks["suits"][_other.suit];
                }
                else
                {
                    return ranks["values"][value] != ranks["values"][_other.value];
                }
            }
            else
            {
                return true;
            }
        }

        //===============================================================================
        // Helper Functions
        //===============================================================================

        public static string CardAbbrev(string value, string suit)
        {
            if (value == "Joker")
            {
                return "JKR";
            }
            else if (value == "10")
            {
                return $"10{suit[0]}";
            }
            else
            {
                return $"{value[0]}{suit[0]}";
            }
        }


        public static string CardName(string value, string suit)
        {
            if (value == "Joker")
            {
                return "Joker";
            }
            else
            {
                return $"{value} of {suit}";
            }
        }
    }
}