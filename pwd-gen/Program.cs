using System;
using System.Text;
using System.Collections.Generic;

namespace pwd_gen
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder allowed = new StringBuilder();
            for (char c = '0'; c <= '9'; ++c)
                allowed.Append(c);
            for (char c = 'a'; c <= 'z'; ++c)
                allowed.Append(c);
            for (char c = 'A'; c <= 'Z'; ++c)
                allowed.Append(c);
            
            Console.Write("How many passwords do you need? ");
            Random r = new Random();
            int count = int.Parse(Console.ReadLine());
            for (int i = 0; i < count; ++i)
            {
                StringBuilder pwd = new StringBuilder();
                for (int j = 0; j < 8; ++j)
                    pwd.Append(allowed[r.Next(0, allowed.Length-1)]);
                Console.WriteLine(pwd.ToString());
            }
        }
    }
}
