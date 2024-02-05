namespace Ivankarez.AIFR.Common.Utils
{
    public static class Check
    {
        public static T DependencyNotNull<T>(T dep, string message = null)
        {
            message ??= $"Dependency of tpye {typeof(T).Name} cannot be null.";
            if (dep == null)
            {
                throw new System.ArgumentNullException(message);
            }

            return dep;
        }
    }
}
