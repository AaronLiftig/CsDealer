# Card Module

[Source](https://github.com/AaronL87/CsDealer/blob/master/CsDealer/Card.cs)

This module contains the  **Card**  class. Each  **Card**  instance represents a single playing card of a given value and suit.

## class CsDealer.Card(string value, string suit)

The Card class, each instance representing a single playing card.

**Parameters:**
- **value**  (_str_) – The card value.
- **suit**  (_str_) – The card suit.

### GetHashCode()

Returns integer representation of a card.

**Returns:**
- int

### Repr()

Returns printable representation of a card in the form "Card(value={value}, suit={suit})". Equivalent to Python's \_\_repr\_\_ method.

**Returns:**
- string

### ToString()

Returns string representation of a card in the form "{cardName}". Equivalent to Python's \_\_str\_\_ method.

**Returns:**
- string

### Equals(object other)
### Equals(object other, Dictionary\<string, Dictionary\<string, int>> ranks)

Compares the card against another object,  **other**, and checks whether the card is equal to  **other**, based on the given rank dict.

**Parameters:**
- other (_object_) 
or
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### GreaterThanOrEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is greater than or equal to  **other**, based on the given rank dict.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### GreaterThan(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is greater than  **other**, based on the given rank dict.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### LessThanOrEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is less than or equal to  **other**, based on the given rank dict.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### LessThan(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)

Compares the card against another object,  **other**, and checks whether the card is less than  **other**, based on the given rank dict.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### NotEqual(object other, Dictionary\<string, Dictionary\<string, int>> ranks = null)
Compares the card against another object,  **other**, and checks whether the card is not equal to  **other**, based on the given rank dict.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### CardAbbrev(string value, string suit)
Constructs an abbreviation for the card, using the given value, and suit.

**Parameters:**
- other (_object_)
- ranks (_Dictionary\<string, Dictionary\<string, int>>_)

**Returns:**
- bool

### CardName(string value, string suit)
Constructs a name for the card, using the given value and suit.

**Parameters:**
- value (_string_)
- suit (_string_)

**Returns:**
- string
