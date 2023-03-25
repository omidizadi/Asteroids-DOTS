using Unity.Entities;
namespace Modules.Common.Scripts
{
    /// <summary>
    /// Provides a simple way to check for null references.
    /// </summary>
    public static class SanityCheck
    {
        /// <summary>
        /// Throws an exception if the given entity is null.
        /// </summary>
        /// <param name="entity">the entity to be checked</param>
        /// <param name="name">name of the entity variable</param>
        /// <param name="className">name of the class that the exception was thrown</param>
        public static void NullCheck(Entity entity, string name, string className)
        {
            if (entity == Entity.Null)
            {
                throw new System.Exception($"Null reference exception: {name} in {className}");
            }
        }
    }
}
