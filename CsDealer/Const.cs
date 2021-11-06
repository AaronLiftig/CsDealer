using System.Collections.Generic;

namespace CsDealer
{
    public static class Const
    {
        //===============================================================================
        // Card Data
        //===============================================================================

        public static readonly List<string> SUITS =
            new() { "Diamonds", "Clubs", "Hearts", "Spades" };

        public static readonly List<string> VALUES =
            new() {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};


        //===============================================================================
        // Card Rank Dicts
        //===============================================================================

        public static readonly Dictionary<string, Dictionary<string, int>> POKER_RANKS = 
            new() 
            {
                {"values", new Dictionary<string, int>()
                    {
                        {"Ace", 13},
                        {"King", 12},
                        {"Queen", 11},
                        {"Jack", 10},
                        {"10", 9},
                        {"9", 8},
                        {"8", 7},
                        {"7", 6},
                        {"6", 5},
                        {"5", 4},
                        {"4", 3},
                        {"3", 2},
                        {"2", 1},
                        {"Joker", 0}
                    }
                }
            };

        public static readonly Dictionary<string, Dictionary<string, int>> BIG2_RANKS =
            new()
            {
                {"values", new Dictionary<string, int>()
                    {
                        {"2", 13},
                        {"Ace", 12},
                        {"King", 11},
                        {"Queen", 10},
                        {"Jack", 9},
                        {"10", 8},
                        {"9", 7},
                        {"8", 6},
                        {"7", 5},
                        {"6", 4},
                        {"5", 3},
                        {"4", 2},
                        {"3", 1},
                        {"Joker", 0}
                    }
                },

                {"suits", new Dictionary<string, int>()
                    {
                        {"Spades", 4},
                        {"Hearts", 3},
                        {"Clubs", 2},
                        {"Diamonds", 1}
                    }
                }
            };


        public static readonly Dictionary<string, Dictionary<string, int>> DEFAULT_RANKS =
            new()
            {
                {"values", POKER_RANKS["values"]},
                {"suits", BIG2_RANKS["suits"]}
            };

        //===============================================================================
        // Misc.
        //===============================================================================

        // Stack/Deck ends.
        public const string TOP = "top";
        public const string BOTTOM = "bottom";
    }
}