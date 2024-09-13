using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage.Exceptions;
using Microsoft.JSInterop;

namespace Blazored.LocalStorage;

internal abstract class BrowserStorageProviderBase
{
    protected const string StorageNotAvailableMessage = "Unable to access the browser storage. This is most likely due to the browser settings.";
    protected readonly IJSInProcessRuntime? JSInProcessRuntime;
    protected readonly IJSRuntime JSRuntime;

    public BrowserStorageProviderBase(IJSRuntime jSRuntime)
    {
        JSRuntime = jSRuntime;
        JSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
    }

    protected static bool IsStorageDisabledException(Exception exception)
        => exception.Message.Contains("Failed to read the 'localStorage' property from 'Window'");

    public string GetItem(string key)
    {
        CheckForInProcessRuntime();
        try
        {
            return JSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
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

    public void SetItem(string key, string data)
    {
        CheckForInProcessRuntime();
        try
        {
            JSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
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

    public void Clear()
    {
        CheckForInProcessRuntime();
        try
        {
            JSInProcessRuntime.InvokeVoid("localStorage.clear");
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

    public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);
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

    public bool ContainKey(string key)
    {
        CheckForInProcessRuntime();
        try
        {
            return JSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
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

    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            return await JSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key);
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

    public string Key(int index)
    {
        CheckForInProcessRuntime();
        try
        {
            return JSInProcessRuntime.Invoke<string>("localStorage.key", index);
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

    public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        try
        {
            return await JSRuntime.InvokeAsync<string?>("localStorage.key", cancellationToken, index);
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

    public IEnumerable<string> Keys()
    {
        CheckForInProcessRuntime();
        try
        {
            return JSInProcessRuntime.Invoke<IEnumerable<string>>("eval", "Object.keys(localStorage)");
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

    public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await JSRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken, "Object.keys(localStorage)");
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

    public int Length()
    {
        CheckForInProcessRuntime();
        try
        {
            return JSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
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

    public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await JSRuntime.InvokeAsync<int>("eval", cancellationToken, "localStorage.length");
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

    public void RemoveItem(string key)
    {
        CheckForInProcessRuntime();
        try
        {
            JSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
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

    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
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

    public void RemoveItems(IEnumerable<string> keys)
    {
        CheckForInProcessRuntime();
        try
        {
            foreach (var key in keys)
            {
                JSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
            }
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

    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var key in keys)
            {
                await JSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
            }
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

    [MemberNotNull(nameof(JSInProcessRuntime))]
    protected void CheckForInProcessRuntime()
    {
        if (JSInProcessRuntime == null)
            throw new InvalidOperationException("IJSInProcessRuntime not available");
    }
}
