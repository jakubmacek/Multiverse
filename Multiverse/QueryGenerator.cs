using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Multiverse
{
    public static class QueryGenerator
    {
        public static IQueryable<Unit> UnitIsInPlace(this IQueryable<Unit> query, IEnumerable<Place> places)
        {
            Expression? filterExpression = null;
            var parameterExp = Expression.Parameter(typeof(Unit), "unit");
            var placeExp = Expression.Property(parameterExp, "Place");

            foreach (var place in places)
            {
                var e = Expression.Equal(placeExp, Expression.Constant(place));

                if (filterExpression == null)
                    filterExpression = e;
                else
                    filterExpression = Expression.OrElse(filterExpression, e);
            }

            if (filterExpression == null)
                return query;

            return query.Where(Expression.Lambda<Func<Unit, bool>>(filterExpression, parameterExp));
        }
    }
}
