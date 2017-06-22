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

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print some application details.
            XConsole.Print("================================", false, false, true, MessageStyle.General);
            XConsole.Print("Welcome to HelloWorldAPI v0.0.1!", false, false, true, MessageStyle.Notice);
            XConsole.Print("Written By: Jason Tanner", false, false, true, MessageStyle.Notice);
            XConsole.Print("================================", false, false, true, MessageStyle.General);

            // Spawn a new API object so we can print, store, and do whatever else we need.
            HelloWorldAPI api = new HelloWorldAPI();

            // Print "Hello World".
            api.PrintGreeting();

            // Print a custom message without encryption.
            api.PrintGreeting("Hola! Mi nombre es Jason.");

            // Print a custom message with encryption.
            api.PrintGreeting("Este es muy importante!", true);

            // Store our message using a specified method.
            api.StoreData("Test data to store...", HelloWorldAPI.StorageType.File);
            api.StoreData("Some more test data...", HelloWorldAPI.StorageType.Container);
            api.StoreData("This is even more data to be stored...", HelloWorldAPI.StorageType.Database);

            // Pause the console to read output.
            XConsole.Print("\nPress any key to continue...", false, false, true, MessageStyle.General);
            Console.Read();
        }
    }
}
