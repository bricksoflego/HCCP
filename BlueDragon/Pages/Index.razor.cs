using BlueDragon.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Pages;
public partial class Index
{
    #region Dependencies
    [Inject] IAuthService? AuthService { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        if (AuthService != null)
            AuthService.OnChange += StateHasChanged;
        costAnalysis = Markdown.ToHtml(costAnalysis);
        await base.OnInitializedAsync();
    }
}