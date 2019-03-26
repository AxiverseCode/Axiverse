namespace Axiverse.Injection
{
    /// <summary>
    /// An interface for classes which provides bindings.
    /// </summary>
    public interface IBindingProvider
    {
        /// <summary>
        /// Gets the object bound to the specified key. The object must abide by the type specified
        /// in the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[Key key] { get; }
    }
}
