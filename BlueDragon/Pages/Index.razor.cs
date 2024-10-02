using BlueDragon.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Pages
{
    public partial class Index
    {
        [Inject] IAuthService? AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;
            costAnalysis = Markdown.ToHtml(costAnalysis);
            await base.OnInitializedAsync();
        }
    }
}
