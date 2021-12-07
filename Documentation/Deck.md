# Deck Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Deck.cs)

This module contains the **Deck** class. Each **Deck** instance contains a full, 52 card French deck of playing cards upon instantiation. The **Deck** class is a subclass of the **Stack** class, with a few extra/differing methods.

## class CsDealer.Deck(List<Card> cards = null, bool jokers = false, int numJokers = 0, bool  build  =  true, bool  rebuild  =  false, bool  reshuffle  =  false, Dictionary<string, Dictionary<string, int>> ranks  =  null)

The Deck class, representing the deck that the cards will be in. It is a sublcass of Stack, sharing all of the same methods, in addition to a couple of others you would expect a deck class to have.

**Warning:**
At the moment, adding Jokers may cause some (most) functions/methods to throw errors.

**Parameters:**<br>\-   **cards**  – A list of cards to be the initial contents of the Deck. If provided, the deck will not automatically build a new deck. Can be a  Stack,  Deck, or  list  instance.<br>\-   **jokers**  (_bool_) – Whether or not to include jokers in the deck.<br>\-   **numJokers**  (_int_) – How many jokers to add to the deck.<br>\-   **build**  (_bool_) – Whether or not to build the deck on instantiation.<br>\-   **rebuild**  (_bool_) – Whether or not to rebuild the deck when it runs out of cards due to dealing.<br>\-   **reshuffle**  (_bool_) – Whether or not to shuffle the deck after rebuilding.<br>\-   **ranks**  (_dict_) – The rank dict that will be referenced by the sorting methods etc. Defaults to  DEFAULT_RANKS

### Build(bool  jokers  =  false, int  numJokers  =  0)

Builds a standard 52 card French deck of Card instances.

**Parameters:**<br>\-   **jokers**  (_bool_) – Whether or not to include jokers in the deck.<br>\-   **numJokers**  (_int_) – The number of jokers to include.

**Returns:**<br>\-   a list of cards, which are removed from the deck.

### Deal(int num = 1, bool rebuild = false, bool shuffle = false, string end = Const.TOP)

**Parameters:**<br>\-   **num**  (_int_) – The number of cards to deal.<br>\-   **rebuild**  (_bool_) – Whether or not to rebuild the deck when cards run out.<br>\-   **shuffle**  (_bool_) – Whether or not to shuffle on rebuild.<br>\-   **end**  (_str_) – The end of the  Stack  to add the cards to. Can be  TOP  (“top”) or  BOTTOM  (“bottom”).

**Returns:**<br>\-   A given number of cards from the deck.
