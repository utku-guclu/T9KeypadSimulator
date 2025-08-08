using System;
using System.Collections.Generic;
using System.Text;

namespace T9KeypadSimulator
{
    /// <summary>
    /// Represents the T9 keypad layout and provides methods to convert key sequences to text.
    /// </summary>
    public class T9Keypad
    {
        private static readonly IReadOnlyDictionary<char, string> KeyMappings = new Dictionary<char, string>
        {
            { '1', "&'(" },
            { '2', "ABC" },
            { '3', "DEF" },
            { '4', "GHI" },
            { '5', "JKL" },
            { '6', "MNO" },
            { '7', "PQRS" },
            { '8', "TUV" },
            { '9', "WXYZ" },
            { '0', " " }
        };

        private readonly StringBuilder _result;
        private char? _currentKey;
        private int _keyPressCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="T9Keypad"/> class.
        /// </summary>
        public T9Keypad()
        {
            _result = new StringBuilder();
        }

        /// <summary>
        /// Converts a sequence of key presses to text using T9 keypad rules.
        /// </summary>
        /// <param name="input">The input string containing key presses</param>
        /// <returns>The converted text</returns>
        /// <exception cref="ArgumentException">Thrown when input is invalid or doesn't end with '#'</exception>
        public string ConvertToText(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            if (!input.EndsWith('#'))
            {
                throw new ArgumentException("Input must end with '#'", nameof(input));
            }

            ResetState();

            for (int i = 0; i < input.Length - 1; i++) // Skip the last '#'
            {
                char c = input[i];
                
                // Handle space as a separator between key presses
                if (c == ' ')
                {
                    ProcessCurrentKey();
                    continue;
                }
                
                ProcessCharacter(c);
            }

            // Process any remaining key presses
            ProcessCurrentKey();

            return _result.ToString();
        }

        private void ProcessCharacter(char c)
        {
            switch (c)
            {
                case '*':
                    ProcessBackspace();
                    break;
                case '0':
                    ProcessCurrentKey();
                    _result.Append(' ');
                    break;
                default:
                    ProcessKeyPress(c);
                    break;
            }
        }

        private void ProcessKeyPress(char key)
        {
            if (!IsValidKey(key))
            {
                throw new ArgumentException($"Invalid key: {key}");
            }

            if (_currentKey == key)
            {
                _keyPressCount++;
            }
            else
            {
                // If it's a different key than the current one
                ProcessCurrentKey();  // Process the previous key sequence first
                _currentKey = key;    // Set the new current key
                _keyPressCount = 1;   // Reset press count for the new key
            }
        }

        /// <summary>
        /// Processes the current key sequence and appends the appropriate character to the result.
        /// </summary>
        private void ProcessCurrentKey()
        {
            // If there's no current key being processed, exit early
            if (!_currentKey.HasValue) return;

            // Try to get the letters mapped to the current key
            if (KeyMappings.TryGetValue(_currentKey.Value, out var letters))
            {
                // Calculate the index of the character to use (0-based)
                // Using modulo to cycle through available letters if press count exceeds letter count
                int index = (_keyPressCount - 1) % letters.Length;
                // Append the selected character to the result
                _result.Append(letters[index]);
            }

            // Reset the current key state after processing
            _currentKey = null;
            _keyPressCount = 0;
        }

        /// <summary>
        /// Processes a backspace operation, removing the last character from the result.
        /// </summary>
        private void ProcessBackspace()
        {
            // If there's a current key being processed, reset it
            if (_currentKey.HasValue)
            {
                _currentKey = null;
                _keyPressCount = 0;
            }
            // If there's no current key, remove the last character from the result
            else if (_result.Length > 0)
            {
                _result.Length--;
            }
        }

        /// <summary>
        /// Checks if the provided character is a valid key (0-9 or #).
        /// </summary>
        private static bool IsValidKey(char key)
        {
            return key == '0' || (key >= '2' && key <= '9');
        }

        /// <summary>
        /// Resets the state of the keypad, clearing the result and resetting key tracking.
        /// </summary>
        private void ResetState()
        {
            _result.Clear();
            _currentKey = null;
            _keyPressCount = 0;
        }
    }

    /// <summary>
    /// Provides a static interface to the T9 keypad functionality.
    /// </summary>
    public static class OldPhonePadSimulator
    {
        private static readonly T9Keypad _keypad = new T9Keypad();

        /// <summary>
        /// Converts a sequence of key presses to text using T9 keypad rules.
        /// </summary>
        /// <param name="input">The input string containing key presses</param>
        /// <returns>The converted text</returns>
        /// <exception cref="ArgumentException">Thrown when input is invalid or doesn't end with '#'</exception>
        public static string OldPhonePad(string? input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            return _keypad.ConvertToText(input);
        }
    }
}
