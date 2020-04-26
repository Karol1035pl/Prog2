using System;
using System.Collections.Generic;

namespace Prog2
{
    class Program
    {
        static void Main(string[] args)
        {
            OdwroconaNotacjaPolska test = new OdwroconaNotacjaPolska("sin(2*x)");
            string[] tokeny = test.Tokeny();
            for (int i = 0; i < tokeny.Length; i++)
            {
                System.Console.Write("{0} ", tokeny[i]);
            }
            string[] postfix = test.ONP_postfix(tokeny);
            System.Console.WriteLine();
            for (int i = 0; i < postfix.Length; i++)
            {
                System.Console.Write("{0} ", postfix[i]);
            }
        }
    }
}
