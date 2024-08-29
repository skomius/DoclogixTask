using DoclogixTask.Interface;
using DoclogixTask.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoclogixTask
{
    public class ExpressionBuilder: IExpressionBuilder
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

        public ExpressionBuilder() { }

        public Func<T, bool> GetExpression<T>(IList<ParseResult> filters)
        {
            
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            Expression expression = null;

            if (filters == null || !filters.Any())
                expression = Expression.Equal(Expression.Default(typeof(T)), Expression.Default(typeof(T)));
            else
            {
                foreach (var filter in filters)
                {
                    if (expression == null)
                        expression = GetExpression<T>(parameter, filter);
                    else
                        expression = Expression.AndAlso(expression, GetExpression<T>(parameter, filter));
                }
            }

            return Expression.Lambda<Func<T, bool>>(expression, parameter).Compile();
        }

        private static Expression GetExpression<T>(ParameterExpression parameter, ParseResult filter)
        {
            MemberExpression member = Expression.PropertyOrField(parameter, filter.Property);
            UnaryExpression constant = GetUnary(member, filter);

            switch (filter.Operator)
            {
                case Operator.Equals:
                        return Expression.Equal(member, constant);

                case Operator.NotEquals:
                        return Expression.NotEqual(member, constant);

                case Operator.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Operator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operator.LessThan:
                    return Expression.LessThan(member, constant);

                case Operator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operator.Contains:
                        return Expression.Call(member, containsMethod, constant);

                case Operator.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);

                case Operator.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);
            }

            return null;
        }

        private static UnaryExpression GetUnary(MemberExpression member, ParseResult filter)
        {
            if (filter.Value is string)
                return Expression.Convert(Expression.Constant(filter.Value), member.Type);

            if (filter.Value == null)
                return Expression.Convert(Expression.Constant(filter.Value), member.Type);

            if (member.Type.Equals(typeof(DateTime)) || member.Type.Equals(typeof(DateTime?)))
                return Expression.Convert(Expression.Constant(DateTime.Parse(filter.Value.ToString())), member.Type);
            else
                return Expression.Convert(Expression.Constant(filter.Value), member.Type);
        }
    }

    public enum Operator
    {
        Contains,
        Equals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        NotEquals,
        StartsWith,
        EndsWith
    }
}
