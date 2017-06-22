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
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorld.Tests
{
    [TestClass]
    public class HelloWorldAPITests
    {
        [TestMethod]
        public void PrintGreeting_Default()
        {
            // Listen for the contents of the console.
            using (var co = new ConsoleOutput())
            {
                // Construct our API object and print a greeting.
                HelloWorldAPI api = new HelloWorldAPI();
                api.PrintGreeting();

                // Split up our output since we have a bunch of bells and whistles.
                string[] split = co.GetOuput().Split(':');

                // Check if we have our standard "Hello World" output.
                Assert.AreEqual("Hello World!", split[split.Length - 1].Trim(null));
            }
        }

        [TestMethod]
        public void PrintGreeting_CustomNoEncryption()
        {
            // Create a new greeting string.
            string greeting = "This is a custom greeting!";

            // Listen for the contents of the console.
            using (var co = new ConsoleOutput())
            {
                // Construct our API object and print a greeting.
                HelloWorldAPI api = new HelloWorldAPI();
                api.PrintGreeting(greeting);

                // Split up our output because of formatting.
                string[] split = co.GetOuput().Split(':');

                // Check our assertion.
                Assert.AreEqual(greeting, split[split.Length - 1].Trim(null));
            }
        }

        [TestMethod]
        public void PrintGreeting_CustomWithEncryption()
        {
            /* Encrypted output is indeterministic so we'll just run the method, but since we 
             * have included error handling in our code our test will work regardless; otherwise
             * if there was any kind of error the test would fail. */
            HelloWorldAPI api = new HelloWorldAPI();
            api.PrintGreeting("Hoy es miercoles.", true);
        }

        [TestMethod]
        public void PrintGreeting_CustomWithEncryption_BadLength()
        {
            // Create an abnormally long string.
            string longString = null;
            for (int i = 0; i < 1024; i++) { longString += "x"; }
            
            // Create our API object.
            HelloWorldAPI api = new HelloWorldAPI();

            // Run our test.
            api.PrintGreeting(longString, true);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(CryptographicException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_File()
        {
            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI();

            // Store our data into a file.
            api.StoreData("Some test data...", HelloWorldAPI.StorageType.File);

            // Check if the file exists.
            Assert.IsTrue(File.Exists(api.FilePath));
        }

        [TestMethod]
        public void StoreData_File_PathTooLong()
        {
            // Create an abnormally long path.
            string path = null;
            string longString = null;
            for (int i = 0; i < 1024; i++) { longString += "x"; }
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + longString;

            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI(path);

            // Try storing our data.
            api.StoreData("Test data...", HelloWorldAPI.StorageType.File);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(PathTooLongException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_File_DirectoryNotFound()
        {
            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI("This is not a real directory");

            // Try storing our data into a non-existant file.
            api.StoreData("Invalid path data...", HelloWorldAPI.StorageType.File);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(DirectoryNotFoundException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_File_UnauthorizedAccess()
        {
            /* Note - This unauthorized access test method will fail if run under
             * an adminstrator or higher elevated user. So, run them unprivileged. */

            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI(@"C:\Windows\System32");

            // Try storing our data into a non-existant file.
            api.StoreData("Data being written...", HelloWorldAPI.StorageType.File);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(UnauthorizedAccessException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_Container()
        {
            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI();

            // Store our data into a container.
            api.StoreData("More test data...", HelloWorldAPI.StorageType.Container);

            // Check if the file exists.
            Assert.IsTrue(File.Exists(api.ContainerPath));
        }

        [TestMethod]
        public void StoreData_Container_PathTooLong()
        {
            // Create an abnormally long path.
            string path = null;
            string longString = null;
            for (int i = 0; i < 1024; i++) { longString += "x"; }
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + longString;

            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI(path);

            // Try storing our data.
            api.StoreData("Test data...", HelloWorldAPI.StorageType.Container);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(PathTooLongException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_Container_DirectoryNotFound()
        {
            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI("This is not a real directory");

            // Try storing our data into a non-existant file.
            api.StoreData("Invalid path data...", HelloWorldAPI.StorageType.Container);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(DirectoryNotFoundException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_Container_UnauthorizedAccess()
        {
            /* Note - This unauthorized access test method will fail if run under
             * an adminstrator or higher elevated user. So, run them unprivileged. */

            // Create a new API object.
            HelloWorldAPI api = new HelloWorldAPI(@"C:\Windows\System32");

            // Try storing our data into a non-existant file.
            api.StoreData("Data being written...", HelloWorldAPI.StorageType.Container);

            // Check our exceptions and assert.
            if (api.Errors.Count > 0)
                Assert.IsInstanceOfType(api.Errors[0], typeof(UnauthorizedAccessException));
            else
                throw new ApplicationException("No errors were found.");
        }

        [TestMethod]
        public void StoreData_Database()
        {
            // Since this method hasn't been fully implemented, it will pass.
            HelloWorldAPI api = new HelloWorldAPI();
            api.StoreData("More test data...", HelloWorldAPI.StorageType.Database);
        }
    }

    internal class ConsoleOutput : IDisposable
    {
        private StringWriter sw;
        private TextWriter tw;

        public ConsoleOutput()
        {
            sw = new StringWriter();
            tw = Console.Out;
            Console.SetOut(sw);
        }

        public string GetOuput()
        {
            return sw.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(tw);
            sw.Dispose();
        }
    }
}