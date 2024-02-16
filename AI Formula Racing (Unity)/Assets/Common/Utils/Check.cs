namespace Ivankarez.AIFR.Common.Utils
{
    public static class Check
    {
        public static T ArgumentNotNull<T>(T dep, string message = null)
        {
            message ??= $"Dependency of tpye {typeof(T).Name} cannot be null.";
            if (dep == null)
            {
                throw new System.ArgumentNullException(message);
            }

            return dep;
        }

        public static void State(bool condition, string message = null)
        {
            message ??= "State condition failed.";
            if (!condition)
            {
                throw new System.InvalidOperationException(message);
            }
        }
    }
}
