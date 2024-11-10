using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SnusPunch.Shared.Models.Pagination;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SnusPunch.Data.Helpers
{
    public static class QueryHelpers
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> aQuery, string? aSortProperty, SortOrderEnum aSortOrder)
        {
            if (string.IsNullOrEmpty(aSortProperty))
            {
                return aQuery;
            }

            var sParameter = Expression.Parameter(typeof(T), "x");
            var sProperty = Expression.Property(sParameter, aSortProperty);

            var sOrderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(sProperty, typeof(object)), sParameter);

            IOrderedQueryable<T> sOrderedQuery = 
                aSortOrder == SortOrderEnum.Descending ?
                    Queryable.OrderByDescending(aQuery, sOrderByExpression) :
                    Queryable.OrderBy(aQuery, sOrderByExpression);

            return sOrderedQuery;
        }

        public static IQueryable<T> SearchByProperty<T>(this IQueryable<T> aQuery, string? aSearchProperty, string? aSearchString)
        {
            if (string.IsNullOrEmpty(aSearchProperty) || string.IsNullOrEmpty(aSearchString))
            {
                return aQuery;
            }

            var sParameter = Expression.Parameter(typeof(T), "x");
            var sProperty = Expression.Property(sParameter, aSearchProperty);

            if (sProperty.Type != typeof(string))
            {
                throw new ArgumentException("Sökegenskapen måste vara en sträng!");
            }

            var sSearchValue = Expression.Constant(aSearchString);
            var sContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var sContainsExpression = Expression.Call(sProperty, sContains, sSearchValue);

            var sLambda = Expression.Lambda<Func<T, bool>>(sContainsExpression, sParameter);

            return aQuery.Where(sLambda);
        }
    }
}
