using Microsoft.AspNetCore.Http;

namespace SnusPunch.Shared.Models.Entry
{
    public class AddEntryWithImageDto
    {
        public string? Description { get; set; }
        public int SnusId { get; set; }
        public IFormFile? FormFile { get; set; } = null;
    }
}
