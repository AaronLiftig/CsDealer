# Card Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Card.cs)

This module contains the  **Card**  class. Each  **Card**  instance represents a single playing card, of a given value and suit.

## class CsDealer.Card(string value, string suit)

The Card class, each instance representing a single playing card.

**Parameters:**<br>\-   **value**  (_string_) – The card value.<br>\-   **suit**  (_string_) – The card suit.

### GetHashCode()

Returns integer representation of a card.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   int

### Repr()

Returns printable representation of a card of the form "Card(value={value}, suit={suit})". Equivalent to Python's \_\_repr\_\_ method.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   string

### ToString()

Returns string representation of a card of the form "{cardName}". Equivalent to Python's __repr__ method.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   string

### Equals(object other)
### Equals(object other, Dictionary\<string, Dictionary\<string, int>> ranks)

Compares the card against another object,  **other**, and checks whether the card is equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_) 
or<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### GreaterThanOrEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is greater than or equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### GreaterThan(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is greater than  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### LessThanOrEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is less than or equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### LessThan(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is less than  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### NotEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)
Compares the card against another object,  **other**, and checks whether the card is not equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### CardAbbrev(string value, string suit)
Constructs an abbreviation for the card, using the given value, and suit.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**<br>\-   bool

### CardName(string value, string suit)
Constructs a name for the card, using the given value, and suit.

**Parameters:**<br>\-   value (_string_)<br>\-   suit (_string_)

**Returns:**<br>\-   string
