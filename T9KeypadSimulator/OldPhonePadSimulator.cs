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
                ProcessCurrentKey();
                _currentKey = key;
                _keyPressCount = 1;
            }
        }

        private void ProcessCurrentKey()
        {
            if (!_currentKey.HasValue) return;

            if (KeyMappings.TryGetValue(_currentKey.Value, out var letters))
            {
                int index = (_keyPressCount - 1) % letters.Length;
                _result.Append(letters[index]);
            }

            _currentKey = null;
            _keyPressCount = 0;
        }

        private void ProcessBackspace()
        {
            if (_currentKey.HasValue)
            {
                _currentKey = null;
                _keyPressCount = 0;
            }
            else if (_result.Length > 0)
            {
                _result.Length--;
            }
        }

        private static bool IsValidKey(char key)
        {
            return key == '0' || (key >= '2' && key <= '9');
        }

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
