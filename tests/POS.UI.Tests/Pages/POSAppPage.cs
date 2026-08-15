using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Playwright;
using Xunit;

namespace POS.UI.Tests.Pages;

/// <summary>
/// Page Object Model for the POS Dashboard / Main App.
/// Follows the POM pattern for clean separation of test logic and selectors.
/// </summary>
public class POSAppPage
{
    private readonly IPage _page;

    public POSAppPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync(string baseUrl = "http://localhost:5000")
    {
        await _page.GotoAsync(baseUrl);
    }

    public async Task ClickNavItemAsync(string navId)
    {
        await _page.ClickAsync($"#{navId}");
    }

    public async Task<bool> IsViewVisibleAsync(string viewId)
    {
        var section = _page.Locator($"#{viewId}");
        return await section.IsVisibleAsync();
    }

    public async Task<string> GetPageTitleAsync()
    {
        return await _page.Locator("#page-title").InnerTextAsync();
    }
}
