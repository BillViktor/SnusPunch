namespace SnusPunch.Shared.Models.Pagination
{
    public class PaginationParameters
    {
        public int PageNumber { get; set; } = 1;
        public int Skip { get; set; } = 0;

        private int mPageSize = 25;
        public int PageSize
        {
            get
            {
                return mPageSize;
            }
            set
            {
                if(value < 100)
                {
                    mPageSize = value;
                    Skip = (PageNumber - 1) * PageSize;
                } 
            }
        }

        //Sorting
        public string SortPropertyName { get; set; } = "";
        public SortOrderEnum SortOrder { get; set; } = SortOrderEnum.Ascending;

        //Search
        public List<string> SearchPropertyNames { get; set; } = new List<string>();
        public string SearchString { get; set; } = "";
    }
}
