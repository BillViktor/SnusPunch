using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Profile
{
    public partial class ProfilePage
    {
        [Parameter] public string UserName { get; set; }
    }
}
