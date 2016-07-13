namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Методы расширения для <see cref="ScResult"/>
    /// </summary>
    public static class ScResultExtensionMethods
    {
        /// <summary>
        /// To the bool.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>True, если ScResult.SC_RESULT_OK и False в противном случае </returns>
        public static bool ToBool(this ScResult result)
        {
            return result == ScResult.SC_RESULT_OK;
        }
    }
}
