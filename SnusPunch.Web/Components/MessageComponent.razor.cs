using Microsoft.AspNetCore.Components;

namespace SnusPunch.Web.Components
{
    public partial class MessageComponent
    {
        [Parameter]
        public bool IsBusy { get; set; }

        [Parameter]
        public List<string> SuccessMessages { get; set; } = new List<string>();
    }

}
