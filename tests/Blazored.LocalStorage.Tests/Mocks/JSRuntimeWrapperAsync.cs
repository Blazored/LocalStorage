using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage.Tests.Mocks
{
    /// <summary>
    /// This class is just for mocking purposes
    /// </summary>
    public class JSRuntimeWrapperAsync : IJSRuntime
    {
        public virtual ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual  ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual ValueTask InvokeVoidAsync(string identifier, object[] args)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class is just for mocking purposes
    /// </summary>
    public class JSRuntimeWrapper : IJSInProcessRuntime
    {
        public virtual T Invoke<T>(string identifier, params object[] args)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void InvokeVoid(string identifier, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
