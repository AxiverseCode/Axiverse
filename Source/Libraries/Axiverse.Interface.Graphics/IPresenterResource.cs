namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Interface for resources which need to be disposed and recreated when the presenter is
    /// resized.
    /// </summary>
    public interface IPresenterResource
    {
        /// <summary>
        /// Recreate resources bound to the presenter.
        /// </summary>
        void Recreate();

        /// <summary>
        /// Dispose resources bound to the presenter.
        /// </summary>
        void Dispose();
    }
}
