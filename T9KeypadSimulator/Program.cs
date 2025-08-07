using System;

namespace T9KeypadSimulator
{
    /// <summary>
    /// Console application for testing the Old Phone Keypad Simulator.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Old Phone Keypad Simulator");
            Console.WriteLine("=========================");
            Console.WriteLine();
            Console.WriteLine("Instructions:");
            Console.WriteLine("- Keys 2-9: Press multiple times to cycle through letters");
            Console.WriteLine("- Space: Separator (allows same key sequences)");
            Console.WriteLine("- *: Backspace (removes last character)");
            Console.WriteLine("- 0: Adds a space to output");
            Console.WriteLine("-#: End of input");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  33# → E");
            Console.WriteLine("  227*# → B");
            Console.WriteLine("  4433555 555666# → HELLO");
            Console.WriteLine("  222 2 22# → CAB");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.Write("Enter input (or 'quit' to exit): ");
                    string? input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    if (input.ToLower() == "quit")
                        break;

                    string output = OldPhonePadSimulator.OldPhonePad(input);
                    Console.WriteLine($"Output: '{output}'");
                    Console.WriteLine();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Thanks for using the Old Phone Keypad Simulator!");
        }
    }
}
