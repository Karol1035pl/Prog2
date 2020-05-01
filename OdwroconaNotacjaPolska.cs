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
        public double X { get; set; }
        public OdwroconaNotacjaPolska(string formula)
        {
            formula = formula.ToLower();
            Formula = formula;
        }

        public bool Walidacja(string[] tokeny)
        {
            int liczbaNawiasow = 0;
            if (Regex.IsMatch(tokeny[tokeny.Length - 1], @"[\-\+\/\*\^]")) return false;
            for (int i = 0; i < tokeny.Length; i++)
            {
                if (tokeny[i] == "(")
                {
                    liczbaNawiasow++;
                }
                else if (tokeny[i] == ")")
                {
                    if (liczbaNawiasow <= 0) return false;
                    liczbaNawiasow--;
                }
                if (!Regex.IsMatch(tokeny[i], @"[a-zA-Z\d\(\)]|[\-\+\/\*\^]") || (tokeny[i] != "x" && Regex.IsMatch(tokeny[i], @"^[a-zA-Z]") && !Regex.IsMatch(tokeny[i], @"(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)")))
                {
                    return false;
                }
                if (i != 0 && Regex.IsMatch(tokeny[i], @"[\-\+\/\*\^]") && Regex.IsMatch(tokeny[i - 1], @"[\-\+\/\*\^]"))
                {
                    return false;
                }
                if (Regex.IsMatch(tokeny[i], @"\d+"))
                {
                    if (i != 0)
                    {
                        if (!(Regex.IsMatch(tokeny[i - 1], @"([(]|[\-\+\/\*\^]|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan))"))) return false;
                        if (tokeny[i] == "0")
                            if (tokeny[i - 1] == "/") throw new Exception("Nie można dzielić przez 0");
                    }
                }
            }
            if (liczbaNawiasow != 0) return false;
            return true;
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
        public double ONP_postfix_oblicz(string[] postfix)
        {
            double wynik;
            Stack<string> s = new Stack<string>();
            for (int i = 0; i < postfix.Length; i++)
            {
                if (Regex.IsMatch(postfix[i], @"\d|[x]"))
                {
                    if (postfix[i] == "x") s.Push(X.ToString());
                    else s.Push(postfix[i]);
                }
                else if (Regex.IsMatch(postfix[i], @"(pi)|(e)"))
                {
                    switch (postfix[i])
                    {
                        case "pi":
                            s.Push(Math.PI.ToString());
                            break;
                        case "e":
                            s.Push(Math.E.ToString());
                            break;
                    }
                }
                else if (Regex.IsMatch(postfix[i], @"^[-]((abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan))|((abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan))"))
                {
                    double x;
                    x = Convert.ToDouble(s.Pop(), System.Globalization.CultureInfo.InvariantCulture);
                    switch (postfix[i])
                    {
                        case "abs":
                            s.Push(Math.Abs(x).ToString());
                            break;
                        case "cos":
                            s.Push(Math.Cos(x).ToString());
                            break;
                        case "exp":
                            s.Push(Math.Exp(x).ToString());
                            break;
                        case "log":
                            s.Push(Math.Log(x).ToString());
                            break;
                        case "sin":
                            s.Push(Math.Sin(x).ToString());
                            break;
                        case "sqrt":
                            s.Push(Math.Sqrt(x).ToString());
                            break;
                        case "tan":
                            s.Push(Math.Tan(x).ToString());
                            break;
                        case "cosh":
                            s.Push(Math.Cosh(x).ToString());
                            break;
                        case "sinh":
                            s.Push(Math.Sinh(x).ToString());
                            break;
                        case "tanh":
                            s.Push(Math.Tanh(x).ToString());
                            break;
                        case "acos":
                            s.Push(Math.Acos(x).ToString());
                            break;
                        case "asin":
                            s.Push(Math.Asin(x).ToString());
                            break;
                        case "atan":
                            s.Push(Math.Atan(x).ToString());
                            break;
                    }
                }
                else if (Regex.IsMatch(postfix[i], @"[\-\+\/\*\^]"))
                {
                    double a, b;
                    a = Convert.ToDouble(s.Pop(), System.Globalization.CultureInfo.InvariantCulture);
                    b = Convert.ToDouble(s.Pop(), System.Globalization.CultureInfo.InvariantCulture);
                    switch (postfix[i])
                    {
                        case "+":
                            s.Push((a + b).ToString());
                            break;
                        case "-":
                            s.Push((b - a).ToString());
                            break;
                        case "*":
                            s.Push((a * b).ToString());
                            break;
                        case "/":
                            s.Push((b / a).ToString());
                            break;
                        case "^":
                            s.Push((Math.Pow(b, a)).ToString());
                            break;
                    }
                }
            }
            wynik = Convert.ToDouble(s.Pop());
            if (double.IsNaN(wynik)) throw new NullReferenceException("Błąd przedziału");
            return wynik;
        }
        public List<Punkty> ONP_postfix_przedzial(string[] postfix, double from, double to, int ilosc)
        {
            List<Punkty> wyniki = new List<Punkty>();
            double wynik;
            double dod = (to - from) / (ilosc - 1);
            X = from;
            for (int i = 0; i < ilosc; i++)
            {
                wynik = ONP_postfix_oblicz(postfix);
                wyniki.Add(new Punkty(X, wynik));
                X += dod;
            }
            return wyniki;
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
    public class Punkty
    {
        public double x { get; set; }
        public double y { get; set; }

        public Punkty(double X, double Y)
        {
            x = X;
            y = Y;
        }
    }
}
