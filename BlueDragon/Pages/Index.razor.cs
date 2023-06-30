using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
    public partial class Index
    {
        [Inject] AuthService? AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;
            costAnalysis = Markdown.ToHtml(costAnalysis);
            await base.OnInitializedAsync();
        }
    }
}
