
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordGenerator
{
    public class Program
    {
        // Create objects
        StringBuilder structure;
        Random asciiRange;
        HashSet<int> numbers;
        HashSet<char> symbols;
        Regex _initials;

        public string ReturnPassword(string str)
        {
            ElementPrep();

            // Example name
            string ExampleName = str;

            // Instantiation
            structure = new StringBuilder();

            var initials = RegexGetGivenNameInitials(ExampleName);

            if (initials != null)
            {
                var prefix = AsciiRangeGen(initials);
                var root = NumericRangeGen();
                var suffix = SymbolPoolGenerator();

                structure.Append(prefix);
                structure.Append(root.ToString().Substring(0, 5));
                structure.Append(suffix);
            }
            return structure.ToString();
        }

        public void ElementPrep()
        {
            _initials = new Regex(@"(\b[a - zA - Z])[a - zA - Z] * ? ");
            asciiRange = new Random();
            numbers = new HashSet<int>();
            symbols = new HashSet<char>();
        }

        public string AsciiRangeGen(string initials) // Recursion, prefix must not be same as Given Name Initials
        {
            string trivial = ((char)asciiRange.Next(65, 90)).ToString() + ((char)asciiRange.Next(97, 122)).ToString();

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
            catch { }
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
            }
            catch { }
            int root = Int32.Parse(String.Join("", numbers));
            numbers.Clear();

            return root;
        }

        public string RegexGetGivenNameInitials(string givenName)
        {
            try
            {
                if (givenName != null)
                    givenName = givenName.Split(',')[1].Trim() + givenName.Split(',')[0].Trim();
            }
            catch { }
            return _initials.Replace(givenName, "$1");
        }
    }
}
