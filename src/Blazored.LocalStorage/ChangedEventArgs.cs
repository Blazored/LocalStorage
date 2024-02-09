using System.Diagnostics.CodeAnalysis;

namespace Blazored.LocalStorage
{
    [ExcludeFromCodeCoverage]
    public class ChangedEventArgs
    {
        public string Key { get; set; } = null!; // Since .NET 6 is supported, `required` is not available yet
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
    }
}
