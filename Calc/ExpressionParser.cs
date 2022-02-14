using System;
using System.Linq.Expressions;
using Sprache;

namespace Calc
{
    public class ExpressionParser : IParser<long>
    {
        public Expression<Func<long>> Parse(string input)
        {
            var result = Syntax.ParseLambda(new Input(input));

            if (!result.WasSuccessful)
            {
                throw new Exception(result.Message);
            }

            return result.Value;
        }
    }
}