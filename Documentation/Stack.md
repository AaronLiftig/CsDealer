# Stack Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Stack.cs)

This module contains the **Stack** class, which is the backbone of the PyDealer package. A **Stack** is essentially just a generic “card container”, with all of the methods users may need to work with the cards they contain. A **Stack** can be used as a hand, or a discard pile, etc.

## class CsDealer.Stack(List\<Card> cards = null, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

The Stack class, representing a collection of cards. This is the main ‘card container’ class, with methods for manipulating it’s contents.

**Parameters:**
- **cards**  (_List\<Card>_) – A list of cards to be the initial contents of the Stack.
- **sort**  (_bool_) – If **sort=True**, The rank dict to reference for sorting. Defaults to **DEFAULT_RANKS**.
- **ranks** (_Dictionary\<string, Dictionary\<string, int>>_) - Whether or not to sort the stack upon instantiation.

### Add(Card  cards, string  end  =  Const.TOP)
### Add(List\<Card> cards, string  end  =  Const.TOP)

Adds the given list of  **Card**  instances to the top of the stack.

**Parameters:**
-  **cards**  – The cards to add to the  **Stack**. Can be a single  **Card**  instance, or a  **list**  of cards.
-  **end**  (_string_) – The end of the  **Stack**  to add the cards to. Can be  **TOP**  (“top”) or  **BOTTOM**  (“bottom”).

### cards

The cards property.

**Returns:**
- The cards in the Stack/Deck.

### Deal(int  num  =  1, string  end  =  Const.TOP)

Returns a list of cards, which are removed from the Stack.

**Parameters:**
-   **num**  (_int_) – The number of cards to deal.
-   **end**  (_string_) – Which end to deal from. Can be  **0**  (top) or  **1**  (bottom).

**Returns:**
- The given number of cards from the stack.

### Empty(bool  returnCards  =  false)

Empties the stack, removing all cards from it, and returns them.

**Parameters:**
- **return_cards**  (_bool_) – Whether or not to return the cards.

**Returns:**
- If  **return_cards=True**, a list containing the cards removed from the Stack.

### Find(string term, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Searches the stack for cards with a value, suit, name, or abbreviation matching the given argument, ‘term’.

**Parameters:**
-  **term**  (_string_) – The search term. Can be a card full name, value, suit, or abbreviation.
- **limit**  (_int_) – The number of items to retrieve for each term.  **0**  equals no limit.
- **sort**  (_bool_) – Whether or not to sort the results.
- **ranks**  (_Dictionary\<string, Dictionary\<string, int>>_) – The rank dict to reference for sorting. If **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of stack indices for the cards matching the given terms, if found.

### FindList(List\<string> terms, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Searches the stack for cards with a value, suit, name, or abbreviation matching the given argument, ‘terms’.

**Parameters:**
-   **terms**  (_list_) – The search terms. Can be card full names, suits, values, or abbreviations.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of stack indices for the cards matching the given terms, if found.

### Get(int term, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)
### Get(string term, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Get the specified card from the stack.

**Parameters:**
-   **term**  – The search term. Can be a card full name, value, suit, abbreviation, or stack indice.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of the specified cards, if found.

### GetList(List\<string> terms, int limit = 0, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)
### GetList(List\<int> terms, bool sort = false, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Get the specified cards from the stack.

**Parameters:**
-   **term**  – The search term. Can be a card full name, value, suit, abbreviation, or stack indice.
-   **limit**  (_int_) – The number of items to retrieve for each term.
-   **sort**  (_bool_) – Whether or not to sort the results, by poker ranks.
-   **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- A list of the specified cards, if found.

### Insert(Card  card, int  index  =  -1)

Insert a given card into the stack at a given indice.

**Parameters:**
-   **card**  (_Card_) – The card to insert into the stack.
-   **indice**  (_int_) – Where to insert the given card.

### InsertList(List\<Card> cards, int  index  =  -1)

Insert a list of given cards into the stack at a given indice.

**Parameters:**
-   **cards**  (_List\<Card>_) – The list of cards to insert into the stack.
-   **indice**  (_int_) – Where to insert the given cards.

### IsSorted(Dictionary\<string, Dictionary\<string, int>> ranks  =  null)

Checks whether the stack is sorted.

**Parameters:**
-  **ranks**  (_dict_) – The rank dict to reference for checking. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- Whether or not the cards are sorted.

### OpenCards(string  filename  =  null)

Open cards from a txt file.

**Parameters:**
- **filename**  (_str_) – The filename of the deck file to open. If no filename given, defaults to “cards-YYYYMMDD.txt”, where “YYYYMMDD” is the year, month, and day. For example, “cards-20140711.txt”.

### RandomCard(bool  remove_  =  false)

Returns a random card from the Stack. If  **remove=True**, it will also remove the card from the deck.

**Parameters:**
- **remove**  (_bool_) – Whether or not to remove the card from the deck.

**Returns:**
- A random Card object, from the Stack.

### Reverse()

Reverse the order of the Stack in place.

### SaveCards(string  filename  =  null)

Save the current stack contents, in plain text, to a txt file.

**Parameters:**
- **filename**  (_str_) – The filename to use for the file. If no filename given, defaults to “cards-YYYYMMDD.txt”, where “YYYYMMDD” is the year, month, and day. For example, “cards-20140711.txt”.

### SetCards(List\<Card> cards)

Change the Deck’s current contents to the given cards.

**Parameters:**
- **cards**  (_list_) – The Cards to assign to the stack.

### Shuffle(int  times  =  1)

Shuffles the Stack.

**Note:**
Shuffling large numbers of cards (100,000+) may take a while.

**Parameters:**
- **times**  (_int_) – The number of times to shuffle.

### Size

Counts the number of cards currently in the stack.

**Returns:**
- The number of cards in the stack.

### Sort(Dictionary\<string, Dictionary\<string, int>> ranks  =  null)

Sorts the stack, either by poker ranks, or big two ranks.

**Parameters:**
- **ranks**  (_dict_) – The rank dict to reference for sorting. If  **null**, it will default to  **DEFAULT_RANKS**.

**Returns:**
- The sorted cards.

### Split(int  index  =  0, bool  halve  =  true)

Splits the Stack, either in half, or at the given indice, into two separate Stacks.

**Parameters:**
- **indice**  (_int_) – Optional. The indice to split the Stack at. Defaults to the middle of the  Stack.

**Returns:**
- The two parts of the Stack, as separate Stack instances.

### ConvertToStack(Deck  deck)

Convert a  Deck  to a  Stack.

**Parameters:**
- **deck**  (_Deck_) – The  Deck  to convert.

**Returns:**
- A new  Stack  instance, containing the cards from the given  Deck  instance.
