/////////////////////////////////////////////////////////////
// Program: "Hello World" API
// Author: Jason Tanner
// Date: 6/22/17
/////////////////////////////////////////////////////////////
/* Requirements:
 *  A. Print "Hello World" to screen using an API
 *  B. API should be in its own reusable class
 *  C. API should be extensible
 *      I. Use common design patterns
 *      II. Use common config files to store data
 *  D. Incorporate unit tests to support the API class
 */
/////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace HelloWorld
{
    /// <summary>
    /// A color scheme based on the type of message to be displayed; Grey, Blue, Green, Yellow, Red, White.
    /// </summary>
    public enum MessageStyle { General, Notice, Success, Warning, Error, None }

    /// <summary>
    /// The encoding type to be used when coverting strings, bytes, and arrays.
    /// </summary>
    public enum EncodingStyle { Default, ASCII, UTF7, UTF8, UTF32, Unicode, BigEndianUnicode }
    
    public static class XText
    {
        /// <summary>
        /// Converts a byte array into a readable string.
        /// </summary>
        /// <param name="input">The byte array to be converted.</param>
        /// <param name="style">The encoding to use when coverting.</param>
        /// <returns>A string formatted with the specified encoding style.</returns>
        public static string GetString(this byte[] input, EncodingStyle style = EncodingStyle.Default)
        {
            StringBuilder sb = new StringBuilder();
            switch (style)
            {
                case EncodingStyle.Default:
                    sb.Append(Encoding.Default.GetString(input));
                    break;
                case EncodingStyle.ASCII:
                    sb.Append(Encoding.ASCII.GetString(input));
                    break;
                case EncodingStyle.UTF7:
                    sb.Append(Encoding.UTF7.GetString(input));
                    break;
                case EncodingStyle.UTF8:
                    sb.Append(Encoding.UTF8.GetString(input));
                    break;
                case EncodingStyle.UTF32:
                    sb.Append(Encoding.UTF32.GetString(input));
                    break;
                case EncodingStyle.Unicode:
                    sb.Append(Encoding.Unicode.GetString(input));
                    break;
                case EncodingStyle.BigEndianUnicode:
                    sb.Append(Encoding.BigEndianUnicode.GetString(input));
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a string into a byte array.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <param name="style">The encoding to use when coverting.</param>
        /// <returns>A byte array formatted with the specified encoding style.</returns>
        public static byte[] GetBytes(this string input, EncodingStyle style = EncodingStyle.Default)
        {
            byte[] output = null;
            switch (style)
            {
                case EncodingStyle.Default:
                    output = Encoding.Default.GetBytes(input);
                    break;
                case EncodingStyle.ASCII:
                    output = Encoding.ASCII.GetBytes(input);
                    break;
                case EncodingStyle.UTF7:
                    output = Encoding.UTF7.GetBytes(input);
                    break;
                case EncodingStyle.UTF8:
                    output = Encoding.UTF8.GetBytes(input);
                    break;
                case EncodingStyle.UTF32:
                    output = Encoding.UTF32.GetBytes(input);
                    break;
                case EncodingStyle.Unicode:
                    output = Encoding.Unicode.GetBytes(input);
                    break;
                case EncodingStyle.BigEndianUnicode:
                    output = Encoding.BigEndianUnicode.GetBytes(input);
                    break;
            }
            return output;
        }

        /// <summary>
        /// Converts a char array into a byte array.
        /// </summary>
        /// <param name="input">The array to be converted.</param>
        /// <param name="style">The encoding to use when coverting.</param>
        /// <returns>A byte array formatted with the specified encoding style.</returns>
        public static byte[] GetBytes(this char[] input, EncodingStyle style = EncodingStyle.Default)
        {
            byte[] output = null;
            switch (style)
            {
                case EncodingStyle.Default:
                    output = Encoding.Default.GetBytes(input);
                    break;
                case EncodingStyle.ASCII:
                    output = Encoding.ASCII.GetBytes(input);
                    break;
                case EncodingStyle.UTF7:
                    output = Encoding.UTF7.GetBytes(input);
                    break;
                case EncodingStyle.UTF8:
                    output = Encoding.UTF8.GetBytes(input);
                    break;
                case EncodingStyle.UTF32:
                    output = Encoding.UTF32.GetBytes(input);
                    break;
                case EncodingStyle.Unicode:
                    output = Encoding.Unicode.GetBytes(input);
                    break;
                case EncodingStyle.BigEndianUnicode:
                    output = Encoding.BigEndianUnicode.GetBytes(input);
                    break;
            }
            return output;
        }

        /// <summary>
        /// Converts a string into a Base64 encoded string.
        /// </summary>
        /// <param name="input">The string to be converted.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string ToBase64(this string input)
        {
            return Convert.ToBase64String(GetBytes(input));
        }

        /// <summary>
        /// Converts a byte array into a Base64 encoded string.
        /// </summary>
        /// <param name="input">The array to be converted.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string ToBase64(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// Converts a char array into a Base64 encoded string.
        /// </summary>
        /// <param name="input">The char array to be converted.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string ToBase64(this char[] input)
        {
            return Convert.ToBase64String(GetBytes(input));
        }

        /// <summary>
        /// Converts a Base64 encoded string into a non-encoded string.
        /// </summary>
        /// <param name="input">The original Base64 string.</param>
        /// <returns>A non-encoded string.</returns>
        public static string FromBase64(this string input)
        {
            return GetString(Convert.FromBase64String(input));
        }
    }
    public static class XConsole
    {
        /// <summary>
        /// Prints a message to the console with a predefined style.
        /// </summary>
        /// <param name="message">The text to be stylized before printing.</param>
        /// <param name="accents">Flag determining if we should have special characters in front of our message.</param>
        /// <param name="timestamp">Flag determining whether a timestamp should be included with the message.</param>
        /// <param name="line">Flag determining if the entire message line should be stylized or not.</param>
        /// <param name="style">The style or color choice for the message we are printing.</param>
        public static void Print(string message, bool accents = true, bool timestamp = false, bool line = false, MessageStyle style = MessageStyle.None)
        {
            switch (style)
            {
                case MessageStyle.General:
                    if (line)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                            Console.WriteLine("[-]: " + message);
                        else
                            Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("-");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("]: ");
                        }
                        Console.WriteLine(message);
                    }
                    break;
                case MessageStyle.Notice:
                    if (line)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                            Console.WriteLine("[*]: " + message);
                        else
                            Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("*");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("]: ");
                        }
                        Console.WriteLine(message);
                    }
                    break;
                case MessageStyle.Success:
                    if (line)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                            Console.WriteLine("[+]: " + message);
                        else
                            Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("+");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("]: ");
                        }
                        Console.WriteLine(message);
                    }
                    break;
                case MessageStyle.Warning:
                    if (line)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                            Console.WriteLine("[!]: " + message);
                        else
                            Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("!");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("]: ");
                        }
                        Console.WriteLine(message);
                    }
                    break;
                case MessageStyle.Error:
                    if (line)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                            Console.WriteLine("[x]: " + message);
                        else
                            Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (timestamp)
                            Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                        if (accents)
                        {
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("x");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("]: ");
                        }
                        Console.WriteLine(message);
                    }
                    break;
                case MessageStyle.None:
                    Console.ForegroundColor = ConsoleColor.White;
                    if (timestamp)
                        Console.Write(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
