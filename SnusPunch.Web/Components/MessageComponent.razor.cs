using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Errors;

namespace SnusPunch.Web.Components
{
    public partial class MessageComponent
    {
        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();

        [Parameter]
        public List<string> SuccessMessages { get; set; } = new List<string>();
    }

}
