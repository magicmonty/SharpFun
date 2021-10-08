namespace Pagansoft.Functional
{
    /// <summary>
    /// Represents a void return value without functionality
    /// </summary>
    public sealed class Unit
    {
        /// <summary>
        /// Returns the (one and only) unit instance
        /// </summary>
        public static readonly Unit Instance = new ();

        private Unit() { }
    }
}