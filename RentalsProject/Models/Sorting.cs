using System.Linq.Expressions;

namespace RentalsProject.Models
{
    public static class Sorting
    {
        private const string SORT_BY_DESC_SUFFIX = "_desc";
        private const string SORT_BY_DELIMITER = ",";

        public static IQueryable<RentalListing> Sort(this IQueryable<RentalListing> query, string? sortByColumn)
        {
            // incase there is sort
            if (string.IsNullOrEmpty(sortByColumn))
            {
                return query;
            }

            var sortList = sortByColumn.Split(SORT_BY_DELIMITER);

            foreach (var currentColumn in sortList)
            {
                var sortByDesc = IsSortDescenting(currentColumn);

                // nameof gets the string value of the course name.
                if (getColumnName(currentColumn) == nameof(RentalListing.ListingSummary))
                {
                    query = SortBy(query, c => c.ListingSummary, sortByDesc);
                }
                else if (getColumnName(currentColumn) == nameof(RentalListing.City))
                {
                    query = SortBy(query, c => c.City, sortByDesc);
                }
                else if (getColumnName(currentColumn) == nameof(RentalListing.Province))
                {
                    query = SortBy(query, c => c.Province, sortByDesc);
                }
                else if (getColumnName(currentColumn) == nameof(RentalListing.NumberOfBedrooms))
                {
                    query = SortBy(query, c => c.NumberOfBedrooms, sortByDesc);
                }
            }

            return query;
        }

        private static IQueryable<RentalListing> SortBy<TKey>(IQueryable<RentalListing> query, Expression<Func<RentalListing, TKey>> property, bool descending = false)
        {
            // if an order by has previously been applied, add a "ThenBy". Otherwise just do a regular order by.
            // Then by is not on IQueryable, but it is on IOrderedQueryable
            if (query is IOrderedQueryable<RentalListing> orderable)
            {
                if (descending)
                {
                    query = orderable.ThenByDescending(property);
                }
                else
                {
                    query = orderable.ThenBy(property);
                }
            }
            else
            {
                if (descending)
                {
                    query = query.OrderByDescending(property);
                }
                else
                {
                    query = query.OrderBy(property);
                }
            }

            return query;
        }

        public static IQueryable<Reservation> Sort(this IQueryable<Reservation> query, string? sortByColumn)
        {
            // incase there is sort
            if (string.IsNullOrEmpty(sortByColumn))
            {
                return query;
            }

            var sortList = sortByColumn.Split(SORT_BY_DELIMITER);

            foreach (var currentColumn in sortList)
            {
                var sortByDesc = IsSortDescenting(currentColumn);

                // nameof gets the string value of the course name.
                if (getColumnName(currentColumn) == nameof(Reservation.RentalListing.City))
                {
                    query = SortBy(query, c => c.RentalListing.City, sortByDesc);
                }
                else if (getColumnName(currentColumn) == nameof(Reservation.CheckIn))
                {
                    query = SortBy(query, c => c.CheckIn, sortByDesc);
                }
                else if (getColumnName(currentColumn) == nameof(Reservation.CheckOut))
                {
                    query = SortBy(query, c => c.CheckOut, sortByDesc);
                }
            }

            return query;
        }

        private static IQueryable<Reservation> SortBy<TKey>(IQueryable<Reservation> query, Expression<Func<Reservation, TKey>> reservation, bool descending = false)
        {
            // if an order by has previously been applied, add a "ThenBy". Otherwise just do a regular order by.
            // Then by is not on IQueryable, but it is on IOrderedQueryable
            if (query is IOrderedQueryable<Reservation> orderable)
            {
                if (descending)
                {
                    query = orderable.ThenByDescending(reservation);
                }
                else
                {
                    query = orderable.ThenBy(reservation);
                }
            }
            else
            {
                if (descending)
                {
                    query = query.OrderByDescending(reservation);
                }
                else
                {
                    query = query.OrderBy(reservation);
                }
            }

            return query;
        }

        public static string? GetNewSortOrder(string existingSortOrder, string newSortByColumn)
        {
            if (string.IsNullOrEmpty(newSortByColumn))
                return existingSortOrder;

            if (string.IsNullOrEmpty(existingSortOrder))
                return newSortByColumn;

            var newSortOrder        = new List<string>();
            var previousSortOrder   = existingSortOrder.Split(SORT_BY_DELIMITER).ToList();

            // if I have a new colum to sort by
            if (!string.IsNullOrEmpty(newSortByColumn))
            {
                // either add the new column as is or flip it by inverting it
                if (!existingSortOrder.Contains(newSortByColumn) || existingSortOrder.Contains(InvertSort(newSortByColumn)))
                {
                    newSortOrder.Add(newSortByColumn);
                }
                else
                {
                    newSortOrder.Add(InvertSort(newSortByColumn));
                }

                // once the new one is added, we add old ones if they are there.
                // but first we remove the new column because it has already been added.
                if (previousSortOrder != null)
                {
                    if (newSortByColumn != null)
                    {
                        // already move it to thwe new one, and lets coverthe descending version
                        previousSortOrder.Remove(newSortByColumn);
                        previousSortOrder.Remove(InvertSort(newSortByColumn));
                    }

                    newSortOrder.AddRange(previousSortOrder);
                }
            }

            return string.Join(SORT_BY_DELIMITER, newSortOrder);
        }

        /// <summary>
        /// Check if its Descending, Ascending is default
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static bool IsSortDescenting(string sortOrder)
        {
            if (sortOrder != null && sortOrder.Contains(SORT_BY_DESC_SUFFIX))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string getColumnName(string sortColumn)
        {
            return sortColumn.Replace(SORT_BY_DESC_SUFFIX, string.Empty);
        }

        private static string InvertSort(string sortColumn)
        {
            if (IsSortDescenting(sortColumn))
            {
                return sortColumn.Replace(SORT_BY_DESC_SUFFIX, "");
            }

            return sortColumn + SORT_BY_DESC_SUFFIX;
        }
    }
}
