using System;
using System.Linq;
using System.Linq.Expressions;
using Sprache;

namespace Calc
{
    internal class Syntax
    {
        public static readonly Parser<Expression<Func<long>>> ParseLambda =
            Parse.Ref(() => Expr).End().Select(body => Expression.Lambda<Func<long>>(body));

        static readonly Parser<ExpressionType> Operator =
            Parse.Chars('*', '×', '⋅').Return(ExpressionType.Multiply)
                .Or(Parse.Chars('/', '÷').Return(ExpressionType.Divide))
                //.Or(Parse.Char('%').Return(ExpressionType.Modulo))
                //.Or(Parse.Char('^').Return(ExpressionType.Power))
                .Or(Parse.Char('+').Return(ExpressionType.Add))
                .Or(Parse.Char('-').Return(ExpressionType.Subtract))
                .Token();

        static readonly Parser<Expression> Operand =
            Parse.Ref(() => Number)
                .Or(Parse.Ref(() => Constant))
                .Or(Parse.Ref(() => Parenthesized))
                .Or(Parse.Ref(() => Function))
                .Or(Parse.Ref(() => Negate))
                .Token();

        static readonly Parser<Expression> Expr =
            Parse.ChainOperator(Operator, Operand, Expression.MakeBinary);

        static readonly Parser<Expression> Constant =
            Parse.String("pi").Return(Expression.Constant(Math.PI))
                .Or(Parse.Char('e').Return(Expression.Constant(Math.E)));

        static readonly Parser<Expression> Number =
            Parse.Decimal.Select(number => Expression.Constant(Convert.ToInt64(number)));

        static readonly Parser<Expression> Parenthesized = Parse.Char('(')
            .SelectMany(openParen => Parse.Ref(() => Expr), (openParen, operand) => new { openParen, operand })
            .SelectMany(@t => Parse.Char(')'), (@t, closeParen) => @t.operand);

        static readonly Parser<Expression> Negate = Parse.Char('-')
            .SelectMany(negative => Parse.Ref(() => Operand), (negative, expression) => Expression.Negate(expression));

        static readonly Parser<Expression> Function = Parse.Letter.AtLeastOnce()
            .Text()
            .SelectMany(name => Parse.Char('('), (name, openParen) => new { name, openParen })
            .SelectMany(@t => Parse.Ref(() => Expr).DelimitedBy(Parse.Char(',')), (@t, args) => new { @t, args })
            .SelectMany(@t => Parse.Char(')'),
                (@t, closeParen) => Expression.Call(typeof(Math), @t.@t.name, new Type[0], @t.args.ToArray()));
    }
}