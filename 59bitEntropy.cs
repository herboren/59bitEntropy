using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordGenerator
{
    internal class Program
    {
        // Example name
        const string ExampleName = "Doe, John";
        
        // Create objects
        static StructuredElement buildElement;
        static StringBuilder structure;        

        static void Main(string[] args)
        {
            // Instantiation
            buildElement = new StructuredElement();
            structure = new StringBuilder();

            var initials = buildElement.RegexGetGivenNameInitials(ExampleName);

            if (initials != null)
            {
                for (int i = 0; i < 200; i++)
                {
                    var prefix = buildElement.AsciiRangeGen(initials);
                    var root = buildElement.NumericRangeGen();
                    var suffix = buildElement.SymbolPoolGenerator();

                    structure.Append(prefix);
                    structure.Append(root.ToString().Substring(0, 5));
                    structure.Append(suffix);

                    Console.WriteLine($"{i + 1:00#}. Password: {structure}");
                    structure.Clear();
                }
            }
            else
            {
                Console.WriteLine("nothing here or name does not exist.\n");                
            }

            Console.ReadLine();
        }        
    }

    class StructuredElement
    {
        Random asciiRange;
        HashSet<int> numbers;
        HashSet<char> symbols;
        Regex _initials;

        public StructuredElement()
        {
            _initials = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
            asciiRange = new Random();            
            numbers = new HashSet<int>();
            symbols = new HashSet<char>();
        }

        public string AsciiRangeGen(string initials) // Recursion, prefix must not be same as Given Name Initials
        {
            string trivial = $"{(char)asciiRange.Next(65, 90)}" +
                $"{(char)asciiRange.Next(97, 122)}";

            return initials != trivial ?
                trivial : AsciiRangeGen(initials);
        }

        public string SymbolPoolGenerator()
        {
            char symb = '\n';
            try
            {
                do
                {
                    symb = (char)asciiRange.Next(33, 47);
                    symbols.Add(symb);

                } while (symbols.Count > 0 && symbols.Count < 2);
            }
            catch { Exception e = null; }
            string suffix = String.Join("", symbols);
            symbols.Clear();

            return suffix;
        }

        public int NumericRangeGen()
        {
            int tmp = -1;
            try
            {
                do
                {
                    tmp = asciiRange.Next(0, 10);
                    numbers.Add(tmp);

                } while (numbers.Count > 0 && numbers.Count < 6);
               
            } catch { Exception e = null; }
            int root = Int32.Parse(String.Join("", numbers));
            numbers.Clear();

            return root;
        }

        public string RegexGetGivenNameInitials(string givenName)
        {
            try
            {
                if (givenName != null)
                    givenName = $"{givenName.Split(',')[1].Trim()}" +
                        $"{givenName.Split(',')[0].Trim()}";
            }
            catch { Exception e = null; }

            return _initials.Replace(givenName, "$1");
        }
    }
}
