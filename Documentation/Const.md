# Const Module

[Source](../CsDealer/Const.cs)

These are the few constants that are used by the CsDealer package. The poker ranks, and big two ranks could be used for sorting, or by anyone making a game that relies on those ranks. CsDealer references **DEFAULT_RANKS** for sorting order, and ordering of newly instantiated decks by default.

### SUITS (_List\<string>_)

{ "Diamonds", "Clubs", "Hearts", "Spades" }

### VALUES (_List\<string>_)

{ "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" }

### BIG2_RANKS (_Dictionary<string, Dictionary<string, int>>_)

<pre>
{
  {"values",
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
  }
  {"suits",
    {
      {"Spades", 4},
      {"Hearts", 3},
      {"Clubs", 2},
      {"Diamonds", 1}
    }
  }
}
</pre>

### DEFAULT_RANKS (_Dictionary<string, Dictionary<string, int>>_)

<pre>
{
  {"values", 
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
    {
  },
  {"suits", 
    {
      {"Spades", 4},
      {"Hearts", 3},
      {"Clubs", 2},
      {"Diamonds", 1}
    }
  }
}
</pre>

### POKER_RANKS (_Dictionary<string, Dictionary<string, int>>_)

<pre>
{"values",
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
</pre>

### TOP (_string_)

"top"

### BOTTOM (_string_)

"bottom"
