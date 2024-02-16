namespace Ivankarez.AIFR.Common.Utils
{
    public static class Check
    {
        public static T ArgumentNotNull<T>(T dep, string paramName)
        {
            if (dep == null)
            {
                throw new System.ArgumentNullException(paramName);
            }

            return dep;
        }

        public static void State(bool condition, string message)
        {
            if (!condition)
            {
                throw new System.InvalidOperationException(message);
            }
        }

        public static T[] NotEmpty<T>(T[] array, string parameterName)
        {
            if (array.Length == 0)
            {
                throw new System.ArgumentException("Array must not be empty", parameterName);
            }

            return array;
        }
    }
}
