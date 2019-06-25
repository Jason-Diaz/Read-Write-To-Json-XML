//******************************************************
// File: Program.cs
//
// Purpose: This program will present a menu to the
//          user and then perform an action depending
//          on what the user chooses to do. You should
//          create one instance of a list of country at
//          the top of the main method. When the program
//          runs it should display the menu to the user
//          and give them a chance to input a choice. An
//          action should be taken depending on what
//          choice the user makes. The menu actions
//          should manipulate and use the list of country
//          instance that you declared at the top of main.
//
// Written By: Jason Diaz 
//
// Compiler: Visual Studio 2017
//
//******************************************************

using System;
using System.Collections.Generic;
using System.Text;
using CountryDataLibrary;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace jasonDiaz_Assign3
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Country> countries = new List<Country>();
            string userInput; // Menu user input
            int intVal; // Converted user input

            do
            {
                #region Main Menu
                // Main Menu
                Console.WriteLine("Country Menu");
                Console.WriteLine("------------");
                Console.WriteLine("1 – Read List of Country from JSON file");
                Console.WriteLine("2 – Read List of Country from XML file");
                Console.WriteLine("3 – Write List of Country to JSON file");
                Console.WriteLine("4 – Write List of Country to XML file");
                Console.WriteLine("5 – Display All Country List Items on Screen");
                Console.WriteLine("6 – Find and display country by name");
                Console.WriteLine("7 - Find and display countries that use a given currency code");
                Console.WriteLine("8 – Exit");
                Console.Write("Enter Choice: ");
                userInput = Console.ReadLine();
                Console.WriteLine();

                // Convert string to integer type
                intVal = Convert.ToInt32(userInput);
                #endregion

                #region Menu choices switch statments
                switch (intVal)
                {
                    case 1:
                        countries = deserialize(countries, true);
                        break;
                    case 2:
                        countries = deserialize(countries, false);
                        break;
                    case 3:
                        serialize(countries, true);
                        break;
                    case 4:
                        serialize(countries, false);
                        break;
                    case 5:
                        foreach(Country country in countries)
                        {
                            Console.WriteLine(country.ToString());
                        }
                        break;
                    case 6:
                        Console.Write("Enter country name: ");
                        string countryname = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine((countries.Find(x => x.Name == countryname)).ToString());
                        break;
                    case 7:
                        Console.Write("Enter country code: ");
                        string currencycode = Console.ReadLine();
                        Console.WriteLine();
                        List<Currency> currencyList = new List<Currency>();
                        List<Country> countryList = new List<Country>();

                        foreach (Country country in countries)
                        {
                            if (country.Currencies.Find(x => x.Code == currencycode) != null)
                            {
                                countryList.Add(country);
                            }
                        }
                      
                        foreach (Country country in countryList)
                        {
                            Console.WriteLine(country.Name);
                        }

                        Console.WriteLine();
                        break;

                    default:
                        Console.WriteLine("\nPlease enter valid input!\n");
                        break;
                }
                #endregion
            } while (intVal != 8);
        }

        #region deserialize Method
        static List<Country> deserialize(List<Country> obj, bool isJson)
        {
            
            Console.Write("Enter filename: ");
            string filename = Console.ReadLine();
            Console.WriteLine();

            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(reader, Encoding.UTF8);
            string streamString = streamReader.ReadToEnd();

            byte[] byteArray = Encoding.UTF8.GetBytes(streamString);
            MemoryStream stream = new MemoryStream(byteArray);

            if (isJson)
            {
                DataContractJsonSerializer inputSerializer;
                inputSerializer = new DataContractJsonSerializer(typeof(List<Country>));
                obj = (List<Country>)inputSerializer.ReadObject(stream);
            }
            else
            {
                DataContractSerializer inputSerializer;
                inputSerializer = new DataContractSerializer(typeof(List<Country>));
                obj = (List<Country>)inputSerializer.ReadObject(stream);
            }
            stream.Close();

            return obj;
        }
        #endregion

        #region serialize Method
        static void serialize(List<Country> obj, bool isJson)
        {
            Console.Write("Enter filename: ");
            string filename = Console.ReadLine();
            Console.WriteLine();

            if (isJson)
            {
                DataContractJsonSerializer ser;
                ser = new DataContractJsonSerializer(typeof(List<Country>));

                MemoryStream memoryStream = new MemoryStream();
                ser.WriteObject(memoryStream, obj);

                byte[] data = memoryStream.ToArray();
                string utf8String = Encoding.UTF8.GetString(data, 0, data.Length);

                StreamWriter streamWriter = new StreamWriter(filename, false, Encoding.UTF8);
                streamWriter.Write(utf8String);
                streamWriter.Close();
            }
            else
            {
                DataContractSerializer ser;
                ser = new DataContractSerializer(typeof(List<Country>));

                MemoryStream memoryStream = new MemoryStream();
                ser.WriteObject(memoryStream, obj);

                byte[] data = memoryStream.ToArray();
                string utf8String = Encoding.UTF8.GetString(data, 0, data.Length);

                StreamWriter streamWriter = new StreamWriter(filename, false, Encoding.UTF8);
                streamWriter.Write(utf8String);
                streamWriter.Close();
            }

        }
        #endregion
    }
}
