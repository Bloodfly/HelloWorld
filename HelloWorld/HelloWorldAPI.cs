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
using System.Xml.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HelloWorld
{
    public class HelloWorldAPI
    {
        /// <summary>
        /// The type of storage function to use whether it's a plaintext file, encrypted file, or a database.
        /// </summary>
        public enum StorageType { File, Container, Database }
        /// <summary>
        /// A collection of all exceptions that have occured during method usage.
        /// </summary>
        public List<Exception> Errors = new List<Exception>();
        /// <summary>
        /// The path that our plaintext configuration file will be written to.
        /// </summary>
        public string FilePath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\config.xml";
        /// <summary>
        /// The path that our encrypted configuration file will be written to.
        /// </summary>
        public string ContainerPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\config.enc";

        // Our default constructor which does not accept a config path.
        public HelloWorldAPI() { }

        // Constructor which accepts and overwrites our current config paths.
        public HelloWorldAPI(string directory)
        {
            FilePath = directory + @"\config.xml";
            ContainerPath = directory + @"\config.enc";
        }

        /// <summary>
        /// Prints a simple "Hello World" unless the function is provided with a message to display; also supports encryption.
        /// </summary>
        /// <param name="message">The text to display in the console.</param>
        /// <param name="encrypt">Flag determining whether the message should be encrypted or not.</param>
        public void PrintGreeting(string message = "Hello World!", bool encrypt = false)
        {
            // Print our message based on if we have a custom greeting and if we should encrypt it or not.
            if (encrypt)
            {
                try { XConsole.Print(EncryptMessage(message, 1024), true, true, false, MessageStyle.Success); }
                catch (CryptographicException ex)
                {
                    Errors.Add(ex);
                    XConsole.Print("Your message was unable to be encrypted.", true, true, true, MessageStyle.Error);
                }
                catch (IOException ex)
                {
                    Errors.Add(ex);
                    XConsole.Print("The console is unable to print your message.", true, true, true, MessageStyle.Error);
                }
                catch (Exception ex)
                {
                    Errors.Add(ex);
                    XConsole.Print("There was an error printing your message.", true, true, true, MessageStyle.Error);
                }
            }
            else
            {
                try { XConsole.Print(message, true, true, false, MessageStyle.Success); }
                catch (IOException ex)
                {
                    Errors.Add(ex);
                    XConsole.Print("The console is unable to print your message.", true, true, true, MessageStyle.Error);
                }
            }
        }

        /// <summary>
        /// Stores the provided data using a specified method of storage; data can be stored in a plaintext file,
        /// an encrypted container file, or in a local or remote database.
        /// </summary>
        /// <param name="data">The data to be stored.</param>
        /// <param name="type">The method of storage to be used.</param>
        public void StoreData(string data, StorageType type)
        {
            //Store our provided data into our selected storage type(file, database, etc.)
            try
            {
                switch (type)
                {
                    case StorageType.File:
                        // Store our data into an XML file - can be updated to be an .ini or whatever.
                        XDocument file = Configuration(data);
                        file.Save(FilePath);
                        XConsole.Print("Stored data into a file!", true, true, false, MessageStyle.Success);
                        break;
                    case StorageType.Container:
                        // Store our data into an encrypted XML file.
                        XDocument container = Configuration(data);
                        MemoryStream ms = new MemoryStream();
                        container.Save(ms);
                        byte[] plainbytes = ms.ToArray();
                        byte[] cipherbytes = EncryptBytes(plainbytes, "ThisIsOurSuperSecretPassword");
                        File.WriteAllBytes(ContainerPath, cipherbytes);
                        XConsole.Print("Stored data into an encrypted container!", true, true, false, MessageStyle.Success);
                        break;
                    case StorageType.Database:
                        // Create a query to our database for storage.
                        /* This case is undefined at the moment because of the requirement of making the class
                         * extensible. So, we'll leave it blank until we've decided what type of database to use
                         * or until we get around to extending this method in general. */
                        XConsole.Print("No database could be found to store data!", true, true, false, MessageStyle.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                XConsole.Print(("The provided data could not be stored into a " + type.ToString().ToLower() + "."), true, true, true, MessageStyle.Error);
            }
        }

        /// <summary>
        /// Encrypts the provided string using the asymmetric RSA encryption scheme.
        /// </summary>
        /// <param name="message">The string to be encrypted.</param>
        /// <param name="length">The length of the generated RSA keys.</param>
        /// <returns>An encrypted version of the provided string.</returns>
        private static string EncryptMessage(string message, int length)
        {
            // Encrypt our message with RSA using a demo 1024 bit session key.
            // Note - this is setup to be encryption only since we're not exporting our keys.
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(length);
            byte[] data = message.GetBytes(EncodingStyle.ASCII);
            byte[] encrypted = csp.Encrypt(data, false);
            return encrypted.ToBase64();
        }

        /// <summary>
        /// Encrypts the provided bytes using the symmetric AES256-CBC algorithm.
        /// </summary>
        /// <param name="plainbytes">The bytes to be transformed into encrypted data.</param>
        /// <param name="password">The password to be used during the transformation.</param>
        /// <returns>An encrypted version of the provided plain bytes.</returns>
        private byte[] EncryptBytes(byte[] plainbytes, string password)
        {
            // Encrypt our bytes with AES256 in CBC mode.
            byte[] cipherbytes = null;

            // Set the salt here; must be at least 8 bytes.
            byte[] salt = ("This is my super secret salt!").GetBytes();
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    // Set our inital key and block size.
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    // Derive a new key from our provided password and salt.
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, salt, 1024);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    // Transform our plain bytes into encrypted bytes.
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainbytes, 0, plainbytes.Length);
                        cs.Close();
                    }
                    cipherbytes = ms.ToArray();
                }
            }
            return cipherbytes;
        }

        /// <summary>
        /// Creates a new XML configuration file to be used as settings or however the user wishes.
        /// </summary>
        /// <param name="data">The information to be placed within the configuration file.</param>
        /// <returns>An XML document containing the supplied data and example data.</returns>
        private static XDocument Configuration(string data)
        {
            // Generate a configuration file based on the provided data.
            XElement root = new XElement("HelloWorldAPI");
            XElement row = new XElement("Configuration");
            XElement column = new XElement("Example1", "Hello World!");
            row.Add(column);
            column = new XElement("Example2", "Second example config entry!");
            row.Add(column);
            column = new XElement("Data", data);
            row.Add(column);
            root.Add(row);
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                 new XComment("HelloWorldAPI Configuration File"),
                                 root);
        }
    }
}
