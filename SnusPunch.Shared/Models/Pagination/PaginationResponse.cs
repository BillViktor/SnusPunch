namespace SnusPunch.Shared.Models.Pagination
{
    public class PaginationResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public PaginationMetaData PaginationMetaData { get; set; } = new PaginationMetaData();

        public PaginationResponse()
        {
            Items = new List<T>();
            PaginationMetaData.TotalCount = 0;
            PaginationMetaData.PageSize = 25;
            PaginationMetaData.CurrentPage = 1;
        }

        public PaginationResponse(List<T> aItems, int aCount, int aPageNumber, int aPageSize)
        {
            Items = aItems;
            PaginationMetaData.TotalCount = aCount;
            PaginationMetaData.PageSize = aPageSize;
            PaginationMetaData.CurrentPage = aPageNumber;
        }
    }
}
