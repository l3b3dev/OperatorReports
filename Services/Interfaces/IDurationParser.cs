namespace Services.Interfaces
{
    /// <summary>
    /// IDurationParser
    /// </summary>
    public interface IDurationParser
    {
        /// <summary>
        /// Parses this instance.
        /// </summary>
        /// <returns></returns>
        string Parse(string duration);
    }
}