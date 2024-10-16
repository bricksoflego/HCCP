using Microsoft.JSInterop;

namespace BlueDragon.Extensions;
public static class JSRuntimeExtensions
{
    /// <summary>
    /// Checks if the current JavaScript runtime is available as an unmarshalled runtime (IJSUnmarshalledRuntime),
    /// which allows for more efficient JavaScript interop calls. This is particularly relevant in Blazor Server,
    /// where IJSUnmarshalledRuntime is only available after the first render.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime to check.</param>
    /// <returns>True if the IJSUnmarshalledRuntime is available; otherwise, false.</returns>
    public static bool IsJSRuntimeAvailable(this IJSRuntime jsRuntime)
    {
        // IN BLAZOR SERVER, IJSUnmarshalledRuntime IS ONLY AVAILABLE AFTER THE FIRST RENDER
        return jsRuntime is IJSUnmarshalledRuntime;
    }
}