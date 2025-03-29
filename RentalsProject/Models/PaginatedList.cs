namespace RentalsProject.Models
{
    public class PaginatedList<T> : List<T>
    {
        public string? SearchCriteria { get; set; }


        public int PageNumber { get; set; } // public getter, private setter


        public int TotalPages { get; set; } // Total from the database


        /// <summary>
        /// Constructor for Paginatedlist
        /// </summary>
        /// <param name="items">Items for page</param>
        /// <param name="count">Total items in the database</param>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Size of the page</param>
        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize) 
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }


        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }


        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }


        public int PreviousPageNumber
        { 
            get
            {
                return PageNumber - 1;
            }
        }


        public int NextPageNumber
        {
            get
            {
                return PageNumber + 1;
            }
        }
    }
}
