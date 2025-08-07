# API Documentation

## OldPhonePadSimulator Class

The main static class providing old phone keypad text conversion functionality.

### Public Methods

#### `OldPhonePad(string input)`

**Description:**  
Converts a sequence of old phone keypad input into the corresponding text.

**Parameters:**
- `input` (string): The keypad input sequence

**Returns:**
- `string`: The converted text

**Exceptions:**
- `ArgumentException`: Invalid characters in input
- `InvalidOperationException`: Unexpected processing error

**Input Character Reference:**

| Character | Function | Description |
|-----------|----------|-------------|
| `2-9` | Letter Keys | Press multiple times to cycle through letters |
| `0` | Space | Inserts a literal space character |
| ` ` (space) | Separator | Allows typing multiple characters from same key |
| `*` | Backspace | Removes the last character |
| `#` | End Input | Marks the end of input sequence |

**Key Mappings:**

| Key | Letters | Example Usage |
|-----|---------|---------------|
| 2 | ABC | `2#` → A, `22#` → B, `222#` → C |
| 3 | DEF | `3#` → D, `33#` → E, `333#` → F |
| 4 | GHI | `4#` → G, `44#` → H, `444#` → I |
| 5 | JKL | `5#` → J, `55#` → K, `555#` → L |
| 6 | MNO | `6#` → M, `66#` → N, `666#` → O |
| 7 | PQRS | `7#` → P, `77#` → Q, `777#` → R, `7777#` → S |
| 8 | TUV | `8#` → T, `88#` → U, `888#` → V |
| 9 | WXYZ | `9#` → W, `99#` → X, `999#` → Y, `9999#` → Z |

### Usage Examples

#### Basic Letter Selection
```csharp
// Single letters
string result1 = OldPhonePadSimulator.OldPhonePad("2#");    // "A"
string result2 = OldPhonePadSimulator.OldPhonePad("33#");   // "E"
string result3 = OldPhonePadSimulator.OldPhonePad("7777#"); // "S"
```

#### Multiple Letters from Different Keys
```csharp
// Consecutive different keys
string result = OldPhonePadSimulator.OldPhonePad("23456#"); // "ADGJ"
```

#### Using Separators for Same Key
```csharp
// Multiple characters from same key require separators
string result1 = OldPhonePadSimulator.OldPhonePad("2 22#");     // "AB"
string result2 = OldPhonePadSimulator.OldPhonePad("222 2 22#"); // "CAB"
```

#### Complex Words
```csharp
// HELLO: H(44) E(33) L(555) space-separator L(555) O(666)
string hello = OldPhonePadSimulator.OldPhonePad("4433555 555666#"); // "HELLO"

// WORLD: W(9999) O(666) R(777) L(555) D(3)
string world = OldPhonePadSimulator.OldPhonePad("9999666777555333#"); // "WORLD"
```

#### Using Backspace
```csharp
// Type AB, then backspace to remove B, then add C
string result1 = OldPhonePadSimulator.OldPhonePad("222*22#");   // "AB"
string result2 = OldPhonePadSimulator.OldPhonePad("227*#");     // "B" (22=B, 7=P, *=remove P)
```

#### Adding Spaces
```csharp
// Using 0 for literal spaces
string result1 = OldPhonePadSimulator.OldPhonePad("44033555666#"); // "H ELO"
string result2 = OldPhonePadSimulator.OldPhonePad("2002#");        // "A A"
```

#### Cycling Behavior
```csharp
// More presses than available letters cycle back
string result1 = OldPhonePadSimulator.OldPhonePad("2222#");     // "A" (4 presses cycle back)
string result2 = OldPhonePadSimulator.OldPhonePad("77777#");    // "P" (5 presses on 4-letter key)
```

### Testing the Implementation

#### Running Tests

To run the test suite, use the following command from the solution root:

```bash
cd T9KeypadSimulator.Tests
dotnet test
```

#### Test Categories

The test suite is organized into the following categories:

1. **Basic Functionality Tests**
   - Single key presses
   - Multiple key presses
   - Key cycling behavior

2. **Separator Tests**
   - Space as separator
   - Multiple characters from same key
   - Complex word formation

3. **Backspace Tests**
   - Single backspace
   - Multiple backspaces
   - Edge cases

4. **Space Character Tests**
   - Single spaces
   - Multiple spaces
   - Spaces with other characters

5. **Edge Cases**
   - Empty input
   - Only hash symbol
   - Invalid characters
   - No hash terminator

#### Example Test Cases

```csharp
// Basic functionality
Assert.Equal("A", OldPhonePadSimulator.OldPhonePad("2#"));
Assert.Equal("E", OldPhonePadSimulator.OldPhonePad("33#"));

// Using separators
Assert.Equal("CAB", OldPhonePadSimulator.OldPhonePad("222 2 22#"));
Assert.Equal("HELLO", OldPhonePadSimulator.OldPhonePad("4433555 555666#"));

// Using backspace
Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("227*#"));
Assert.Equal("TURING", OldPhonePadSimulator.OldPhonePad("8 88777444666*664#"));

// Edge cases
Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad(""));
Assert.Throws<ArgumentException>(() => OldPhonePadSimulator.OldPhonePad("22")); // Missing #
```

### Error Handling

#### Invalid Characters
```csharp
try 
{
    string result = OldPhonePadSimulator.OldPhonePad("12a#");
}
catch (ArgumentException ex)
{
    // Handle invalid character error
    Console.WriteLine(ex.Message); // "Invalid input character: '1'..."
}
```

#### Edge Cases
```csharp
// Empty or null input
string empty1 = OldPhonePadSimulator.OldPhonePad("");      // ""
string empty2 = OldPhonePadSimulator.OldPhonePad(null);    // ""
string empty3 = OldPhonePadSimulator.OldPhonePad("#");     // ""
```

### Performance Characteristics

- **Time Complexity**: O(n) where n is input length
- **Space Complexity**: O(m) where m is output length
- **Memory Usage**: Efficient StringBuilder-based string building
- **Thread Safety**: Thread-safe (static methods with no shared state)

### Advanced Scenarios

#### Long Text Processing
```csharp
// Efficiently handles long input sequences
string longInput = "44033555555666027777777733#";
string result = OldPhonePadSimulator.OldPhonePad(longInput); // "H ELLO WORLD"
```

#### Multiple Backspaces
```csharp
// Multiple consecutive backspaces
string result = OldPhonePadSimulator.OldPhonePad("44335555**2#"); // "HEA" (removes LL, adds A)
```

#### Complex Separations
```csharp
// Complex same-key sequences
string result = OldPhonePadSimulator.OldPhonePad("2 22 222 2222#"); // "ABCA"
```

### Best Practices

1. **Input Validation**: Always handle `ArgumentException` for invalid input
2. **Null Checking**: Handle null input gracefully (returns empty string)
3. **Performance**: Method is optimized for single-pass processing
4. **Memory**: Uses StringBuilder internally for efficient string building
5. **Thread Safety**: Safe to use in multi-threaded environments

### Common Patterns

#### Word Building Pattern
```csharp
// Build words letter by letter
var builder = new List<string>();
builder.Add(OldPhonePadSimulator.OldPhonePad("44#"));    // H
builder.Add(OldPhonePadSimulator.OldPhonePad("33#"));    // E
builder.Add(OldPhonePadSimulator.OldPhonePad("555#"));   // L
// Result: H-E-L
```

#### Sentence Building Pattern
```csharp
// Build sentences with spaces
string sentence = OldPhonePadSimulator.OldPhonePad("44033555555666027777777733#");
// Result: "H ELLO WORLD"
```

#### Error Recovery Pattern
```csharp
// Handle errors gracefully
public static string SafeConvert(string input)
{
    try
    {
        return OldPhonePadSimulator.OldPhonePad(input);
    }
    catch (ArgumentException)
    {
        return "[INVALID INPUT]";
    }
    catch (Exception)
    {
        return "[ERROR]";
    }
}
```