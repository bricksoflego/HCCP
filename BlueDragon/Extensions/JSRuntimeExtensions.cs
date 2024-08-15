using Microsoft.JSInterop;

namespace BlueDragon.Extensions
{
    public static class JSRuntimeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <returns></returns>
        public static bool IsJSRuntimeAvailable(this IJSRuntime jsRuntime)
        {
            // In Blazor Server, IJSUnmarshalledRuntime is only available after the first render
            return jsRuntime is IJSUnmarshalledRuntime;
        }
    }
}
