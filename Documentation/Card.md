# Card Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Card.cs)

This module contains the  **Card**  class. Each  **Card**  instance represents a single playing card, of a given value and suit.

## <mark>class CsDealer.Card(string value, string suit)</mark>

The Card class, each instance representing a single playing card.

**Parameters:**<br>\-   **value**  (_string_) – The card value.<br>\-   **suit**  (_string_) – The card suit.

### <mark>GetHashCode()</mark>

Returns integer representation of a card.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   int

### <mark>Repr()</mark>

Returns printable representation of a card of the form "Card(value={value}, suit={suit})". Equivalent to Python's \_\_repr\_\_ method.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   string

### <mark>ToString()</mark>

Returns string representation of a card of the form "{cardName}". Equivalent to Python's __repr__ method.

**Parameters:**<br>\-   No Parameters

**Returns:**<br>\-   string

### <mark>Equals(object other)</mark>
### <mark>Equals(object other, Dictionary<string, Dictionary<string, int>> ranks)</mark>

Compares the card against another object,  **other**, and checks whether the card is equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_) 
or<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>GreaterThanOrEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)</mark>

Compares the card against another object,  **other**, and checks whether the card is greater than or equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>GreaterThan(object other, Dictionary<string, Dictionary<string, int>> ranks = null)</mark>

Compares the card against another object,  **other**, and checks whether the card is greater than  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>LessThanOrEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)</mark>

Compares the card against another object,  **other**, and checks whether the card is less than or equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>LessThan(object other, Dictionary<string, Dictionary<string, int>> ranks = null)</mark>

Compares the card against another object,  **other**, and checks whether the card is less than  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>NotEqual(object other, Dictionary<string, Dictionary<string, int>> ranks = null)</mark>
Compares the card against another object,  **other**, and checks whether the card is not equal to  **other**, based on the given rank dict.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>CardAbbrev(string value, string suit)</mark>
Constructs an abbreviation for the card, using the given value, and suit.

**Parameters:**<br>\-   other (_object_)<br>\-   ranks (_Dictionary<string, Dictionary<string, int>>_)

**Returns:**<br>\-   bool

### <mark>CardName(string value, string suit)</mark>
Constructs a name for the card, using the given value, and suit.

**Parameters:**<br>\-   value (_string_)<br>\-   suit (_string_)

**Returns:**<br>\-   string
