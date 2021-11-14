using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiverse.WebApp.Services
{
    public static class JsRuntimeExtensions
    {
        public static ValueTask Alert(this IJSRuntime runtime, string message)
        {
            return runtime.InvokeVoidAsync("alert", message);
        }

        public static ValueTask ConsoleLog(this IJSRuntime runtime, object? obj)
        {
            return runtime.InvokeVoidAsync("console.log", obj);
        }
    }
}
