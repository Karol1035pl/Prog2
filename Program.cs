using System;
using System.Collections.Generic;

namespace Prog2
{
    class Program
    {
        static void Main(string[] args)
        {
            OdwroconaNotacjaPolska test = new OdwroconaNotacjaPolska(args[0]);
            test.X = Convert.ToDouble(args[1]);
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
            System.Console.WriteLine();
            double wynik = test.ONP_postfix_oblicz(postfix);
            System.Console.WriteLine("{0} ", wynik);
            double[,] wyniki = test.ONP_postfix_przedzial(postfix, Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToInt32(args[4]));
            for (int i = 0; i < Convert.ToInt32(args[4]); i++)
            {
                System.Console.WriteLine("{0} => {1}", wyniki[0, i], wyniki[1, i]);
            }
        }
    }
}
