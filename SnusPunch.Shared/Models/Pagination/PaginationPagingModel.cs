namespace SnusPunch.Shared.Models.Pagination
{
    public class PaginationPagingModel
    {
        public PaginationPagingModel(int aPage, bool aEnabled, string aText)
        {
            Page = aPage;
            Enabled = aEnabled;
            Text = aText;
        }

        public string Text { get; set; }
        public int Page { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }
    }
}
