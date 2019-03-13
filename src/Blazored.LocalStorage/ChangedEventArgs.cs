namespace Blazored.LocalStorage
{
    public class ChangedEventArgs
    {
        public string Key { get; set; }
        
        // NOTE: can't easily make event handlers generic

        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}