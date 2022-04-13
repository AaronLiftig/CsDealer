# Deck Module

[Source](../CsDealer/Deck.cs)

This module contains the **Deck** class. Each **Deck** instance contains a full, 52 card French deck of playing cards upon instantiation. The **Deck** class is a subclass of the **Stack** class with a few extra/differing methods.

## class CsDealer.Deck(List\<Card> cards = null, bool jokers = false, int numJokers = 0, bool  build  =  true, bool  rebuild  =  false, bool  reshuffle  =  false, Dictionary\<string, Dictionary\<string, int>> ranks  =  null)

The Deck class, representing the deck that the cards will be in. It is a sublcass of Stack, sharing all of the same methods, in addition to a couple of others you would expect a deck class to have.

**Warning:**
At the moment, adding Jokers may cause some (most) functions/methods to throw errors.

**Parameters:**
- **cards** (_List\<Card>_)  – A list of cards to be the initial contents of the Deck. If provided, the deck will not automatically build a new deck.
- **jokers**  (_bool_) – Whether or not to include jokers in the deck.
- **numJokers**  (_int_) – How many jokers to add to the deck.
- **build**  (_bool_) – Whether or not to build the deck on instantiation.
- **rebuild**  (_bool_) – Whether or not to rebuild the deck when it runs out of cards due to dealing.
- **reshuffle**  (_bool_) – Whether or not to shuffle the deck after rebuilding.
- **ranks**  (_Dictionary\<string, Dictionary\<string, int>>_) – The rank dict that will be referenced by the sorting methods etc. Defaults to DEFAULT_RANKS

### Build(bool jokers = false, int numJokers = 0)

Builds a standard 52 card French deck of Card instances.

**Parameters:**
- **jokers**  (_bool_) – Whether or not to include jokers in the deck.
- **numJokers**  (_int_) – The number of jokers to include.

**Returns:**
- _List\<Card>_ - A list of cards that were removed from the deck.

### Deal(int num = 1, bool rebuild = false, bool shuffle = false, string end = Const.TOP)

**Parameters:**
- **num**  (_int_) – The number of cards to deal.
- **rebuild**  (_bool_) – Whether or not to rebuild the deck when cards run out.
- **shuffle**  (_bool_) – Whether or not to shuffle on rebuild.
- **end**  (_string_) – The end of the  Stack  to add the cards to. Can be  TOP  (“top”) or  BOTTOM  (“bottom”).

**Returns:**
- _Stack_ - A given number of cards from the deck.

### ConvertToDeck(Stack stack)

Convert a Stack to a Deck.

**Parameters:**
- **stack** (_Stack_) – The Stack to convert.

**Returns:**
- _Stack_ - A new Deck instance containing the cards from the given Stack instance.
