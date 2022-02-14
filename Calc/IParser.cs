using System;
using System.Linq.Expressions;

namespace Calc
{
    public interface IParser<TResult>
    {
        Expression<Func<TResult>> Parse(string input);
    }
}