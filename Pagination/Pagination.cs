using Luilliarcec.Pagination.Contracts;
using Luilliarcec.Pagination.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luilliarcec.Pagination
{
    /// <summary>
    /// Allows paging of data from an IOrderedQueryable object.
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// Paginate function
        /// </summary>
        /// <param name="queryable">Query prepared previously ordered</param>
        /// <param name="action">Paging action type</param>
        /// <param name="current_page">Current page</param>
        /// <param name="limit">Limits of records per page</param>
        /// <returns>Returns a dictionary with total records, limit per page, current page, last page and records</returns>
        public static IDictionary<string, object> Paginate(IOrderedQueryable<IPaginable> queryable, string action, int current_page = 1, int limit = 15)
        {
            IReadOnlyList<IPaginable> data;

            var total_records = GetTotalRecords(queryable);

            var last_page = GetLastPage(total_records, limit);

            switch (action)
            {
                case "next":
                    data = Next(queryable, limit, ref current_page, last_page);
                    break;
                case "previous":
                    data = Previous(queryable, limit, ref current_page);
                    break;
                case "last":
                    data = Last(queryable, limit, last_page);
                    current_page = last_page;
                    break;
                case "first":
                    data = First(queryable, limit);
                    current_page = 1;
                    break;
                case "current":
                    data = Current(queryable, limit, ref current_page, last_page);
                    break;
                default:
                    throw new PaginationException($"Invalid argument exception, expected between, [next, previous, last, first or current], received {action}");
            }

            return new Dictionary<string, object>()
            {
                {"total", total_records},
                {"per_page", limit},
                {"current_page", current_page},
                {"last_page", last_page},
                {"data", data},
            };
        }

        #region <<< PRIVATE FUNCTIONS >>>
        /// <summary>
        /// Get paginated records from current page
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <param name="limit">Limits of records per page</param>
        /// <param name="current_page">Current page</param>
        /// <param name="last_page">Last page</param>
        /// <returns>IPaginable object read-only list</returns>
        private static IReadOnlyList<IPaginable> Current(IOrderedQueryable<IPaginable> queryable, int limit, ref int current_page, int last_page)
        {
            if (current_page >= 1 && current_page <= last_page)
            {
                int skip_data = (current_page - 1) * limit;
                return queryable.Skip(skip_data).Take(limit).ToList().AsReadOnly();
            }

            current_page = 1;
            return First(queryable, limit);
        }

        /// <summary>
        /// Get paginated records from next page
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <param name="limit">Limits of records per page</param>
        /// <param name="current_page">Current page</param>
        /// <param name="last_page">Last page</param>
        /// <returns>IPaginable object read-only list</returns>
        private static IReadOnlyList<IPaginable> Next(IOrderedQueryable<IPaginable> queryable, int limit, ref int current_page, int last_page)
        {
            if (current_page < last_page) current_page += 1; else current_page = last_page;

            int skip_data = (current_page - 1) * limit;

            return queryable.Skip(skip_data).Take(limit).ToList().AsReadOnly();
        }

        /// <summary>
        /// Get paginated records from previous page
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <param name="limit">Limits of records per page</param>
        /// <param name="current_page">Current page</param>
        /// <returns>IPaginable object read-only list</returns>
        private static IReadOnlyList<IPaginable> Previous(IOrderedQueryable<IPaginable> queryable, int limit, ref int current_page)
        {
            if (current_page > 1) current_page -= 1; else current_page = 1;

            int skip_data = (current_page - 1) * limit;

            return queryable.Skip(skip_data).Take(limit).ToList().AsReadOnly();
        }

        /// <summary>
        /// Get the paginated records from the last page
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <param name="limit">Limits of records per page</param>
        /// <param name="last_page">Last page</param>
        /// <returns>IPaginable object read-only list</returns>
        private static IReadOnlyList<IPaginable> Last(IOrderedQueryable<IPaginable> queryable, int limit, int last_page)
        {
            int skip_data = (last_page - 1) * limit;

            return queryable.Skip(skip_data).Take(limit).ToList().AsReadOnly();
        }

        /// <summary>
        /// Get the paginated records from the first page
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <param name="limit">Limits of records per page</param>
        /// <returns>IPaginable object read-only list</returns>
        private static IReadOnlyList<IPaginable> First(IOrderedQueryable<IPaginable> queryable, int limit)
        {
            return queryable.Take(limit).ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the total number of records for a prepared query
        /// </summary>
        /// <param name="queryable">Prepared query</param>
        /// <returns>An integer representing the total number of records in a query</returns>
        private static int GetTotalRecords(IOrderedQueryable<IPaginable> queryable)
        {
            return queryable.Count();
        }

        /// <summary>
        /// Get the number of the last page or the total number of pages
        /// </summary>
        /// <param name="total_records">Total records</param>
        /// <param name="limit">Limits of records per page</param>
        /// <returns>Integer rounded up</returns>
        private static int GetLastPage(decimal total_records, int limit)
        {
            var last_page = total_records / limit;

            return (int)Math.Ceiling(last_page);
        }
        #endregion
    }
}
