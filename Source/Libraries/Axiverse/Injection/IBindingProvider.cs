namespace Axiverse.Injection
{
    public interface IBindingProvider
    {
        object this[Key key] { get; }
        bool ContainsKey(Key key);
        bool TryGetValue(Key key, out object value);
    }
}
