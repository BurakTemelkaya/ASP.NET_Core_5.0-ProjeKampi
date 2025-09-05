﻿using System;
using System.Linq.Expressions;

namespace CoreLayer.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        if (left == null)
        {
            return right;
        }

        var invokedExpr = Expression.Invoke(right, left.Parameters);
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
    }

    public static Expression<Func<T, bool>> Or<T>(
    this Expression<Func<T, bool>> left,
    Expression<Func<T, bool>> right)
    {
        if (left == null)
        {
            return right;
        }

        var invokedExpr = Expression.Invoke(right, left.Parameters);
        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(left.Body, invokedExpr), left.Parameters);
    }

}
