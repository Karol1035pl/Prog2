using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Prog2
{
    class OdwroconaNotacjaPolska
    {
        public string Formula { get; set; }
        public OdwroconaNotacjaPolska(string formula)
        {
            formula = formula.ToLower();
            Formula = formula;
        }
        public string[] Tokeny()
        {
            Formula = Regex.Replace(Formula, @"(?<num>\d+(\.\d+)?)", " ${num} ");
            Formula = Regex.Replace(Formula, @"(?<op>[+\-*/^()])", " ${op} ");
            Formula = Regex.Replace(Formula, @"(?<fun>(pi|e|sqrt|abs|exp|sqrt|log|asin|sinh|sin|cosh|acos|cos|atan|tanh|tan))", " ${fun} ");
            Formula = Regex.Replace(Formula, @"\s+", " ").Trim();
            Formula = Regex.Replace(Formula, "-", "MINUS");
            Formula = Regex.Replace(Formula, @"(?<sym>((pi|e|x|[)]|\d+(\.\d+)?)))\s+MINUS", "${sym} -");
            Formula = Regex.Replace(Formula, @"MINUS\s+(?<sym>((sqrt|abs|exp|sqrt|log|asin|sinh|sin|cosh|acos|cos|atan|tanh|tan|[)]|\d+(\.\d+)?)))", "-${sym}");
            string[] tokeny = Formula.Split(" ".ToCharArray());
            return tokeny;
        }
        public string[] ONP_postfix(string[] tokeny)
        {
            Stack<string> s = new Stack<string>();
            Queue<string> k = new Queue<string>();
            for (int i = 0; i < tokeny.Length; i++)
            {
                if (tokeny[i] == "(")
                {
                    s.Push(tokeny[i]);
                }
                else if (tokeny[i] == ")")
                {
                    while (s.Peek() != "(")
                    {
                        k.Enqueue(s.Pop());
                    }
                    s.Pop();
                }
                else if (Regex.IsMatch(tokeny[i], @"[\-\+\/\*\^]") || Regex.IsMatch(tokeny[i], @"((abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan))"))
                {
                    while (s.Count > 0 && Priorytet(tokeny[i]) <= Priorytet(s.Peek()))
                    {
                        k.Enqueue(s.Pop());
                    }
                    s.Push(tokeny[i]);
                }
                if (Regex.IsMatch(tokeny[i], @"\d|[x]|(pi)|(e)"))
                {
                    k.Enqueue(tokeny[i]);
                }
            }
            while (s.Count > 0)
            {
                k.Enqueue(s.Pop());
            }
            string[] l = k.ToArray();
            return l;
        }
        public int Priorytet(string token)
        {
            if (Regex.IsMatch(token, @"((sqrt)|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan))")) return 4;
            else if (token == "^") return 3;
            else if (token == "*" || token == "/") return 2;
            else if (token == "+" || token == "-") return 1;
            else return 0;
        }
    }
}
