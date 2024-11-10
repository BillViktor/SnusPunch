namespace SnusPunch.Shared.Models.Pagination
{
    public class PaginationMetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int Skip { get; set; }

        public bool HasPrevious
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }
    }
}
