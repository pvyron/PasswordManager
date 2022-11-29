using Microsoft.JSInterop;

namespace PasswordManager.Portal.Services;

public sealed class ClipboardService
{
    private readonly IJSRuntime _jsInterop;

    public ClipboardService(IJSRuntime jsInterop)
    {
        _jsInterop = jsInterop;
    }

    public async Task CopyToClipboard(string? text)
    {
        if (text is null) return;

        await _jsInterop.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}