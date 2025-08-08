# Old Phone Keypad Simulator
## C# Coding Challenge / IRON

A C# implementation that simulates the text input method used on old mobile phones before smartphones existed. This simulator converts numeric keypress sequences into their corresponding letters, just like the old T9 (Text on 9 keys) system.

## 🚀 Features

- **Multi-tap Input**: Press keys multiple times to cycle through letters
- **Smart Separation**: Use spaces to type multiple characters from the same key
- **Backspace Support**: Use `*` to delete the last entered character
- **Space Characters**: Use `0` to add literal spaces to your text
- **Input Termination**: Use `#` to indicate end of input
- **Error Handling**: Validates input and provides clear error messages

## 📱 How It Works

### Key Mapping
Each numeric key corresponds to letters, just like old phone keypads:

| Key | Letters |
|-----|---------|
| 2   | ABC     |
| 3   | DEF     |
| 4   | GHI     |
| 5   | JKL     |
| 6   | MNO     |
| 7   | PQRS    |
| 8   | TUV     |
| 9   | WXYZ    |
| 0   | (space) |

### Special Characters

- **`#`** - End of input/Send button
- **`*`** - Backspace (removes last character)
- **` ` (space)** - Separator (allows typing multiple characters from same key)
- **`0`** - Adds a literal space character to the output

## 💡 Usage Examples

### Basic Examples
```csharp
OldPhonePad("33#")           // => "E"     (key 3 pressed twice)
OldPhonePad("2#")            // => "A"     (key 2 pressed once)
OldPhonePad("7777#")         // => "S"     (key 7 pressed four times)
```

### Using Separators
```csharp
OldPhonePad("222 2 22#")     // => "CAB"   (C, then A, then B)
OldPhonePad("4433555 555666#") // => "HELLO" (H-E-L, separator, L-O)
```

### Using Backspace
```csharp
OldPhonePad("227*#")         // => "B"     (22=B, 7=P, *=remove P)
OldPhonePad("2233*2#")       // => "BA"    (A-F, remove F, add A)
```

### Adding Spaces
```csharp
OldPhonePad("44040555666#")  // => "H LO"  (H, space, L, O)
```

## 🏗️ Architecture

### Core Components

1. **`T9KeypadSimulator` Class**
   - Main logic for processing keypad input
   - Static keypad mapping dictionary
   - Input validation and error handling

2. **`OldPhonePad` Method**
   - Primary entry point for text conversion
   - Handles character sequencing and state management
   - Processes special characters (space, backspace, end)

3. **`AppendCharacter` Helper Method**
   - Maps key presses to specific letters
   - Handles cycling through available letters
   - Uses modulo operation for wraparound behavior

### Algorithm Flow

1. **Initialize**: Set up result builder and tracking variables
2. **Process Each Character**:
   - Handle special characters (`#`, `*`, ` `, `0`)
   - Track consecutive presses of same key
   - Process letter sequences when key changes
3. **Finalize**: Process any remaining sequence and return result

## 🧪 Testing

The project includes comprehensive unit tests covering all functionality. The test suite uses xUnit and can be run using the .NET CLI.

### Running Tests

To run the test suite, navigate to the test project directory and run:

```bash
cd T9KeypadSimulator.Tests
dotnet test
```

### Test Coverage

The test suite covers:

- ✅ Basic letter mapping for all keys
- ✅ Multi-press cycling behavior
- ✅ Separator functionality
- ✅ Backspace functionality
- ✅ Space character handling
- ✅ Edge cases and error conditions
- ✅ Complex word formation
- ✅ User-provided example cases

### Example Test Cases

```csharp
// Basic letter mapping
Assert.Equal("A", OldPhonePadSimulator.OldPhonePad("2#"));
Assert.Equal("E", OldPhonePadSimulator.OldPhonePad("33#"));

// Using separators and backspace
Assert.Equal("CAB", OldPhonePadSimulator.OldPhonePad("222 2 22#"));
Assert.Equal("B", OldPhonePadSimulator.OldPhonePad("227*#"));

// Complex words
Assert.Equal("HELLO", OldPhonePadSimulator.OldPhonePad("4433555 555666#"));
Assert.Equal("TURING", OldPhonePadSimulator.OldPhonePad("8 88777444666*664#"));
```
- ✅ Backspace operations
- ✅ Space character insertion
- ✅ Complex integration scenarios
- ✅ Edge cases and error conditions
- ✅ Performance with long inputs

### Running Tests
```bash
dotnet test
```

### Test Coverage
- **Basic Functionality**: Single and multiple key presses
- **Separators**: Space-based sequence separation
- **Backspace**: Character deletion scenarios
- **Integration**: Complex word examples
- **Edge Cases**: Empty input, invalid characters, boundary conditions

## 🔧 Installation & Setup

### Prerequisites
- .NET 6.0 or higher
- Visual Studio 2022 or VS Code (optional)

### Building the Project
```bash
git clone [repository-url]
cd T9KeypadSimulator
dotnet build
```

### Running the Application
```bash
cd T9KeypadSimulator
dotnet run
```

### Running Tests
```bash
cd T9KeypadSimulator.Tests
dotnet test
```

## 📋 API Reference

### `OldPhonePad(string input)`

Converts numeric keypad input to text output.

**Parameters:**
- `input` (string): The keypad input sequence

**Returns:**
- `string`: The converted text

**Throws:**
- `ArgumentException`: When input contains invalid characters

**Valid Input Characters:**
- `2-9`: Letter keys
- `0`: Space character
- `*`: Backspace
- `#`: End input
- ` ` (space): Separator

## 🎯 Performance Characteristics

- **Time Complexity**: O(n) where n is the length of input
- **Space Complexity**: O(m) where m is the length of output
- **Memory Efficient**: Uses StringBuilder for optimal string building
- **Single Pass**: Processes input in one forward pass

## 🐛 Error Handling

The simulator validates input and provides clear error messages for:

- Invalid characters (anything other than 2-9, 0, *, #, space)
- Detailed error messages indicating the problematic character
- Graceful handling of edge cases (empty input, null input)

## 🔍 Example Walkthrough

Let's trace through the complex example: `"4433555 555666#"` → `"HELLO"`

1. **`44`** → Key 4 pressed twice → `H` (2nd letter of "GHI")
2. **`33`** → Key 3 pressed twice → `E` (2nd letter of "DEF")  
3. **`555`** → Key 5 pressed three times → `L` (3rd letter of "JKL")
4. **` `** → Space separator (allows next sequence from same key)
5. **`555`** → Key 5 pressed three times → `L` (3rd letter of "JKL")
6. **`666`** → Key 6 pressed three times → `O` (3rd letter of "MNO")
7. **`#`** → End of input

Result: **"HELLO"**

