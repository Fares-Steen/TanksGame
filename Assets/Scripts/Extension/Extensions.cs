using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Extension
{
    public static class Extensions
    {
        //Extension to sort a list
        public static List<T> SortBy<T>(this List<T> list, Func<T, Object> orderBy)
        {
            list = list.AsQueryable().OrderByDescending(orderBy).ToList();

            return list;
        }

    }
}
