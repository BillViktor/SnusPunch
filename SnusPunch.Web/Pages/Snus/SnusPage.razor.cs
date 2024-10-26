using Microsoft.AspNetCore.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Snus
{
    public partial class SnusPage
    {
        [Inject]
        SnusViewModel SnusViewModel { get; set; }


    }
}
