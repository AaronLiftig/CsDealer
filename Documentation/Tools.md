
# Tools  Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Tools.cs)

The tools module contains functions for working with sequences of cards, some of which are used by the classes in the CsDealer package, such as the functions  buildCards,  sortCards, and  checkTerm for example.

### BuildCards(bool  jokers  =  false, int  numJokers  =  0)

Builds a list containing a full French deck of 52 Card instances. The cards are sorted according to  DEFAULT_RANKS.

**Parameters:**
-   **jokers**  (_bool_) – Whether or not to include jokers in the deck.
-   **num_jokers**  (_int_) – The number of jokers to include.

**Returns:**
- A list containing a full French deck of 52 Card instances.

### CheckSorted(List\<Card> cards, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Checks whether the given cards are sorted by the given ranks.

**Parameters:**
-   **cards**  – The cards to check. Can be a  **Stack**,  **Deck**, or **list** of  **Card**  isntances.
-   **ranks**  (_dict_) – The ranks to check against. Default is **DEFAULT_RANKS**.

**Returns:**
- **True** or **False**.

### CheckTerm(Card  card, string  term)

Checks a given search term against a given card’s full name, suit, value, and abbreviation.

**Parameters:**
-   **card**  (_Card_) – The card to check.
-   **term**  (_str_) – The search term to check for. Can be a card full name, suit, value, or abbreviation.

**Returns:**
**True** or **False**.

### CompareStacks(List\<Card> cardsX, List\<Card> cardsY, bool  sorted  =  false)

Checks whether two given  **Stack**,  **Deck**, or  **list**  instances, contain the same cards (based on value & suit, not instance). Does not take into account the ordering.

**Parameters:**
-   **cardsX**  – The first stack to check. Can be a  Stack,  Deck, or  list  instance.
-   **cardsY**  – The second stack to check. Can be a  Stack,  Deck, or  list  instance.
-   **sorted**  (_bool_) – Whether or not the cards are already sorted. If  True, then  compare_stacks  will skip the sorting process.

**Returns:**
**True** or **False**.

### FindCard(List\<Card> cards, string term, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Searches the given cards for cards with a value, suit, name, or abbreviation matching the given argument,  **term**.

Parameters:

-   **cards**  – The cards to search. Can be a  **Stack**,  **Deck**  or **list**.
-   **term**  (_str_) – The search term. Can be a card full name, value, suit, or abbreviation.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of indices for the cards matching the given terms, if found.

### FindList(List\<Card> cards, List\<string> terms, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Searches the given cards for cards with a value, suit, name, or abbreviation matching the given argument,  **terms**.

**Parameters:**
-   **cards**  – The cards to search. Can be a  **Stack**,  **Deck**  or  **list**.
-   **terms**  (_list_) – The search terms. Can be card full names, suits, values, or abbreviations.
-   **limit**  (_int_) – The number of items to retrieve for each term. 0 == no limit.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of indices for the cards matching the given terms, if found.

### GetCard(List\<Card> cards, int term, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Get the specified card from the stack.

**Parameters:**
-   **cards**  – The cards to get from. Can be a  Stack,  Deck  or  list.
-   **term**  (_str_) – The card’s full name, value, suit, abbreviation, or stack indice.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – If  **sort=true**, the rank dict to refer to for sorting.

**Returns:**
- A copy of the given cards, with the found cards removed, and a list of the specified cards, if found.

### GetList(List<Card> cards, List<int> terms, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)
### GetList(List<Card> cards, List<string> terms, int limit = 0, bool sort = false, Dictionary<string, Dictionary<string, int>> ranks = null)

Get the specified cards from the stack.

**Parameters:**
-   **cards**  – The cards to get from. Can be a  Stack,  Deck  or  list.
-   **terms**  (_list_) – A list of card’s full names, values, suits, abbreviations, or stack indices.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – If  sort=True, the rank dict to refer to for sorting.

**Returns:**
- A list of the specified cards, if found.

### OpenCards(string  filename  =  null)

Open cards from a txt file.

**Parameters:**
- **filename**  (_str_) – The filename of the deck file to open. If no filename given, defaults to “cards-YYYYMMDD.txt”, where “YYYYMMDD” is the year, month, and day. For example, “cards-20140711.txt”.

**Returns:**
- The opened cards, as a list.

### RandomCard(List<Card> cards, bool  remove_  =  false)

Returns a random card from the Stack. If  **remove=True**, it will also remove the card from the deck.

**Parameters:**
- **remove**  (_bool_) – Whether or not to remove the card from the deck.

**Returns:**
- A random Card object, from the Stack.

### SaveCards(List<Card> cards, string  filename  =  null)

Save the given cards, in plain text, to a txt file.

**Parameters:**
-   **cards**  – The cards to save. Can be a  **Stack**,  **Deck**, or  **list**.
-   **filename**  (_str_) – The filename to use for the cards file. If no filename given, defaults to “cards-YYYYMMDD.txt”, where “YYYYMMDD” is the year, month, and day. For example, “cards-20140711.txt”.

### SortCardIndicies(List<Card> cards, List<int> indices, Dictionary<string, Dictionary<string, int>> ranks = null)

Sorts the given Deck indices by the given ranks. Must also supply the  **Stack**,  **Deck**, or  **list**  that the indices are from.

**Parameters:**
-   **cards**  – The cards the indices are from. Can be a  **Stack**,  **Deck**, or  **list**
-   **indices**  (_list_) – The indices to sort.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- The sorted indices.

### SortCards(List\<Card> cards, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Sorts a given list of cards, either by poker ranks, or big two ranks.

**Parameters:**
-   **cards**  – The cards to sort.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- The sorted cards.
