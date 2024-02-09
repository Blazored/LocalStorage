using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        /// <summary>
        /// Clears all data from local storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask ClearAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the specified data from local storage and deseralise it to the specfied type.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the local storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the specified data from local storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Return the name of the key at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a collection of strings representing the names of the keys in the local storage.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the <paramref name="key"/> exists in local storage, but does not check its value.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// The number of items stored in local storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove the data with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a collection of <paramref name="keys"/>.
        /// </summary>
        /// <param name="keys">A IEnumerable collection of strings specifying the name of the storage slot to remove</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The data to be saved</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns></returns>
        ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
