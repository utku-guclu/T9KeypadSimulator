using System;
using System.Collections.Generic;
using System.Text;

namespace T9KeypadSimulator
{
    /// <summary>
    /// Simulates the old mobile phone keypad text input method.
    /// Converts numeric keypress sequences into corresponding letters, supporting
    /// multi-tap input, separators, backspace, and space characters.
    /// </summary>
    public static class OldPhonePadSimulator
    {
        /// <summary>
        /// Maps each numeric key to its corresponding letters following the standard
        /// old mobile phone keypad layout.
        /// </summary>
        private static readonly IReadOnlyDictionary<char, string> KeypadLayout = 
            new Dictionary<char, string>
            {
                { '2', "ABC" },
                { '3', "DEF" },
                { '4', "GHI" },
                { '5', "JKL" },
                { '6', "MNO" },
                { '7', "PQRS" },
                { '8', "TUV" },
                { '9', "WXYZ" }
            };

        /// <summary>
        /// Converts a sequence of old phone keypad input into the corresponding text.
        /// </summary>
        /// <param name="input">The keypad input sequence</param>
        /// <returns>The converted text string</returns>
        /// <exception cref="ArgumentException">Thrown when input contains invalid characters</exception>
        public static string OldPhonePad(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
                
            // Check if input ends with '#'
            if (!input.EndsWith('#'))
            {
                throw new ArgumentException("Input must end with '#'");
            }

            var result = new StringBuilder();
            char currentKey = '\0';
            int pressCount = 0;

            foreach (char ch in input)
            {
                switch (ch)
                {
                    case '#':
                        // End of input - process any pending sequence
                        if (currentKey != '\0')
                        {
                            AppendLetter(result, currentKey, pressCount);
                        }
                        return result.ToString();

                    case '*':
                        // Process current sequence before handling backspace
                        if (currentKey != '\0')
                        {
                            AppendLetter(result, currentKey, pressCount);
                            currentKey = '\0';
                            pressCount = 0;
                        }
                        // Then remove last character if there is one
                        if (result.Length > 0)
                        {
                            result.Length--;
                        }
                        break;

                    case ' ':
                        // Separator - process current sequence
                        if (currentKey != '\0')
                        {
                            AppendLetter(result, currentKey, pressCount);
                            currentKey = '\0';
                            pressCount = 0;
                        }
                        break;

                    case '0':
                        // Space character - process current sequence then add space
                        if (currentKey != '\0')
                        {
                            AppendLetter(result, currentKey, pressCount);
                            currentKey = '\0';
                            pressCount = 0;
                        }
                        result.Append(" ");
                        break;

                    default:
                        // Letter keys (2-9)
                        if (ch < '2' || ch > '9')
                        {
                            throw new ArgumentException($"Invalid input character: '{ch}'. Valid characters are: 2-9 (letters), 0 (space), * (backspace), # (end), and space (separator).");
                        }

                        if (ch == currentKey)
                        {
                            // Same key pressed again
                            pressCount++;
                        }
                        else
                        {
                            // Different key - process previous sequence
                            if (currentKey != '\0')
                            {
                                AppendLetter(result, currentKey, pressCount);
                            }
                            currentKey = ch;
                            pressCount = 1;
                        }
                        break;
                }
            }

            // Process any remaining sequence if no # at end
            if (currentKey != '\0')
            {
                AppendLetter(result, currentKey, pressCount);
            }

            return result.ToString();
        }

        /// <summary>
        /// Appends the appropriate letter based on key and press count.
        /// </summary>
        /// <param name="result">StringBuilder to append to</param>
        /// <param name="key">The key that was pressed</param>
        /// <param name="pressCount">Number of times the key was pressed</param>
        private static void AppendLetter(StringBuilder result, char key, int pressCount)
        {
            if (KeypadLayout.TryGetValue(key, out string? letters))
            {
                int index = (pressCount - 1) % letters.Length;
                result.Append(letters[index]);
            }
        }
    }
}
