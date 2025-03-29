namespace RentalsProject.Models
{
    public class SortedPaginatedList<T> : PaginatedList<T>
    {
        public string? PreviousSort { get; set; }

        public SortedPaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize) : 
            base(items, count, pageNumber, pageSize) { }
    }
}
