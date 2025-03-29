using Microsoft.EntityFrameworkCore;

namespace RentalsProject.Models
{
    public static class PaginatedListExtension
    {
        public static async Task<SortedPaginatedList<T>> ToSortedPaginatedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;
            var count = await query.CountAsync();
            var items = await query.Skip(itemsToSkip).Take(pageSize).ToListAsync();

            return new SortedPaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var itemsToSkip = (pageNumber - 1) * pageSize;
            var count = query.Count();
            var items = query.Skip(itemsToSkip).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
