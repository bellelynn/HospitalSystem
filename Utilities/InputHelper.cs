using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManageSystem.Utilities
{
    public static class InputHelper
    {
        public delegate bool TryParseDelegate<T>(string input, out T value);

        // Writes a message at a specific position in the console without altering the current cursor position.
        public static void WriteAtPosition(int left, int top, string message)
        {
            // Saves the current cursor position.
            var originalLeft = Console.CursorLeft;
            var originalTop = Console.CursorTop;
            // Sets the cursor to the specified position and writes the message.
            Console.SetCursorPosition(left, top);
            Console.Write(message);
            // Restores the original cursor position.
            Console.SetCursorPosition(originalLeft, originalTop);
        }

        public static void SetCursorPositionSafe(int left, int top, string text)
        {
            // Sets the cursor position safely within the console bounds.
            left = Math.Max(0, Math.Min(left, Console.BufferWidth - 1));
            top = Math.Max(0, Math.Min(top, Console.BufferHeight - 1));
            Console.SetCursorPosition(left, top);
        }

        //Every time clear console and display title when enter  menu.
        public static void ClearAndTitle(string title)
        {
            // Clears the console and displays a formatted title.
            Console.Clear();
            Console.WriteLine("=============================");
            Console.WriteLine(title);
            Console.WriteLine("=============================");
        }

        public static void Pause(string message = "Press Enter to back last menu...")
        {
            // Pauses execution and waits for the user to press Enter.
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadKey(true);
        }

        public static string ReadNonEmptyString(string prompt)
        {

            // Prompts the user for a non-empty string input.
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    return s.Trim();
                }
                Console.WriteLine("Input cannot be empty. Please try again.");

            }

        }
        public static int ReadInt(string prompt, Func<int, bool> validator, string error)
        {
            // Prompts the user for an integer input with validation.
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (int.TryParse(s, out var value) && validator(value))
                {
                    return value;
                }
                Console.WriteLine(error);
            }
        }

        public static char ReadOptionLetter(string prompt, IList<char> options)
        {
            // Prompts the user to select a valid option from a list of characters.
            while (true)
            {

                var key = Console.ReadKey(true).KeyChar;
                key = char.ToLowerInvariant(key);
                if (options.Contains(key))
                {
                    Console.WriteLine(key); // Echo the valid input back to the console.
                    return key;
                }
            }
        }

        public static string ReadPasswordMasked(string prompt)
        {
            // Prompts the user for a password input, masking the input with asterisks.
            Console.Write(prompt);
            var sb = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b"); // Remove the last asterisk from the console.
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*"); // Display an asterisk for each character typed.
                }
            }
            return sb.ToString();
        }
    }
}