using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NCalc;

namespace Calc
{
    public static class Calculator
    {
        public static string ArbitraryToArbitrarySystemExpression(string expression, int from, int to)
        {
            MatchEvaluator evaluator = match => ArbitraryToArbitrarySystem(match.Value, from, to);
            return Regex.Replace(expression, @"[\dABCDEF]+", evaluator);
        }
        public static string Get(string expression, int notation)
        {
            MatchEvaluator evaluator = match => ArbitraryToArbitrarySystem(match.Value, notation, 10);
            var expression10 = Regex.Replace(expression, @"[\dABCDEF]+", evaluator);
            var exp = new Expression(expression10);
            exp.Options = EvaluateOptions.RoundAwayFromZero;
            var res = exp.Evaluate();
            return DecimalToArbitrarySystem((int)(double)res, notation);
        }

        public static string ArbitraryToArbitrarySystem(string number, int from, int to)
        {
            return DecimalToArbitrarySystem(ArbitraryToDecimalSystem(number, from), to);
        }

        public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456789ABCDEF";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                                            Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber /= radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        public static long ArbitraryToDecimalSystem(string number, int radix)
        {
            const string Digits = "0123456789ABCDEF";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                                            Digits.Length.ToString());

            if (String.IsNullOrEmpty(number))
                return 0;

           
            number = number.ToUpperInvariant();

            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                 
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }
    }
}