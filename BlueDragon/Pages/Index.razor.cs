using BlueDragon.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Pages;
public partial class Index
{
    #region Dependencies
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] AppConfig? AppConfig { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AppConfig!.CurrentPageTitle = "Home";
        if (AuthService != null)
            AuthService.OnChange += StateHasChanged;
        costAnalysis = Markdown.ToHtml(costAnalysis);
        await base.OnInitializedAsync();
    }
}