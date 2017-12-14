using System;

namespace Workflows.Sorting
{
    /// <inheritdoc />
    /// <summary>
    /// This exception is thrown, whenever a circular dependency 
    /// between nodes detected
    /// </summary>
    public class CircularDependencyException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new instance of CircularDependencyException
        /// </summary>
        /// <param name="node">Node that causes circular dependency</param>
        public CircularDependencyException(object node)
            : base("Found circular dependency in: " + node)
        {
            
        }
    }
}