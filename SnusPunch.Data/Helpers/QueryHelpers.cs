﻿using SnusPunch.Data.Models.Entry;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using System.Linq.Expressions;

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
            Expression sProperty = sParameter;
            foreach (var sPropertyName in aSortProperty.Split('.'))
            {
                sProperty = Expression.PropertyOrField(sProperty, sPropertyName);
            }

            var sOrderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(sProperty, typeof(object)), sParameter);

            IOrderedQueryable<T> sOrderedQuery = 
                aSortOrder == SortOrderEnum.Descending ?
                    Queryable.OrderByDescending(aQuery, sOrderByExpression) :
                    Queryable.OrderBy(aQuery, sOrderByExpression);

            return sOrderedQuery;
        }

        public static IQueryable<T> SearchByProperty<T>(this IQueryable<T> aQuery, List<string> aSearchPropertyList, string? aSearchString)
        {
            if (aSearchPropertyList.Count == 0 || string.IsNullOrEmpty(aSearchString))
            {
                return aQuery;
            }

            var sSearchValue = Expression.Constant(aSearchString);
            var sContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var sParameter = Expression.Parameter(typeof(T), "x");
            Expression? sCombinedExpression = null;

            foreach(var sSearchProperty in aSearchPropertyList)
            {
                Expression sProperty = sParameter;

                foreach (var sPropertyName in sSearchProperty.Split('.'))
                {
                    sProperty = Expression.PropertyOrField(sProperty, sPropertyName);
                }

                if (sProperty.Type != typeof(string))
                {
                    throw new ArgumentException("Sökegenskapen måste vara en sträng!");
                }

                var sContainsExpression = Expression.Call(sProperty, sContains, sSearchValue);

                // Combine the expressions using OrElse
                sCombinedExpression = sCombinedExpression == null
                    ? sContainsExpression
                    : Expression.OrElse(sCombinedExpression, sContainsExpression);
            }
            

            if(sCombinedExpression == null)
            {
                throw new Exception("Nu gick något fel i sökningen!");
            }

            var sLambda = Expression.Lambda<Func<T, bool>>(sCombinedExpression, sParameter);

            return aQuery.Where(sLambda);
        }

        public static IQueryable<EntryModel> FilterEntryHelper(this IQueryable<EntryModel> aQuery, string aSnusPunchUserModelId, EntryFilterEnum aEntryFilterEnum, bool aFetchEmptyPunches)
        {
            IQueryable<EntryModel> sQuery = aQuery;

            //Vilka inlägg ska vi se?
            switch (aEntryFilterEnum)
            {
                default:
                case EntryFilterEnum.All:
                    break;
                case EntryFilterEnum.Self:
                    sQuery = sQuery.Where(x => x.SnusPunchUserModelId == aSnusPunchUserModelId);
                    break;
                case EntryFilterEnum.Friends:
                    sQuery = sQuery.Where(x => x.SnusPunchUserModel.FriendsAddedByOthers.Any(x => x.SnusPunchUserModelOneId == aSnusPunchUserModelId) || x.SnusPunchUserModel.FriendsAddedByUser.Any(x => x.SnusPunchUserModelTwoId == aSnusPunchUserModelId));
                    break;
            }

            //Ska tomma inlägg visas?
            if(!aFetchEmptyPunches)
            {
                sQuery = sQuery.Where(x => x.PhotoUrl != null || x.Description != null);
            }

            //Ta hänsyn till privacy settings
            sQuery = sQuery.Where(x => x.SnusPunchUserModelId == aSnusPunchUserModelId || x.SnusPunchUserModel.EntryPrivacySetting == PrivacySettingEnum.All ||
            (x.SnusPunchUserModel.EntryPrivacySetting == PrivacySettingEnum.Friends && (x.SnusPunchUserModel.FriendsAddedByUser.Any(x => x.SnusPunchUserModelTwoId == aSnusPunchUserModelId) || x.SnusPunchUserModel.FriendsAddedByOthers.Any(x => x.SnusPunchUserModelOneId == aSnusPunchUserModelId))));

            return sQuery;
        }
    }
}
