namespace ScEngineNet.SafeElements
{
    /// <summary>
    /// Методы расширения для <see cref="ElementType"/>
    /// </summary>
    public static class ScResultExtensionMethods
    {
        public static bool ToBool(this ScResult result)
        {
            return result == ScResult.SC_RESULT_OK;
        }
    }
}
