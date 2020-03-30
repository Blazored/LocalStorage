using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage.Tests.Mocks
{
    public class JSRuntimeWrapper : IJSRuntime
    {
        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual  ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
