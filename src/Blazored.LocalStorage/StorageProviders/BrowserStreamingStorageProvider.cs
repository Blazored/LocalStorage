using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage.Exceptions;
using Microsoft.JSInterop;

namespace Blazored.LocalStorage;
internal class BrowserStreamingStorageProvider : BrowserStorageProviderBase, IStorageProvider
{
    public BrowserStreamingStorageProvider(IJSRuntime jSRuntime) : base(jSRuntime) { }

    public async ValueTask<string?> GetItemAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!(await ContainKeyAsync(key, cancellationToken)))
            {
                return null;
            }

            var streamRef = await JSRuntime.InvokeAsync<IJSStreamReference>("getLocalStorageValue", cancellationToken, key);
            using var stream = await streamRef.OpenReadStreamAsync(cancellationToken: cancellationToken);
            using var streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }
        catch (Exception exception)
        {
            if (IsStorageDisabledException(exception))
            {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    public async ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        try
        {
            var dnStream = new DotNetStreamReference(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            await JSRuntime.InvokeVoidAsync("setLocalStorageValue", cancellationToken, key, dnStream);
        }
        catch (Exception exception)
        {
            if (IsStorageDisabledException(exception))
            {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }
}
