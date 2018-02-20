using System;
using System.Collections.Generic;
using System.Linq;
using Workflows.DependencyExplorer;

namespace Workflows
{
    internal class DependencyCollection
    {
        private readonly List<Type> _requiredSteps;
        private readonly List<Type> _requiredAnySteps;

        public DependencyCollection()
        {
            _requiredSteps = new List<Type>();
            _requiredAnySteps = new List<Type>();
        }

        public void AddRequiredSteps(Type[] types)
        {
            if (types.Any(RequiredSteps.Contains) ||
                types.GroupBy(x => x).Count() < types.Length)
                    throw new DuplicateDependencyException();

            _requiredSteps.AddRange(types);
        }

        public void AddRequiredAnySteps(Type[] types)
        {
            if (types.Any(RequiredAnySteps.Contains) ||
                types.GroupBy(x => x).Count() < types.Length)
                    throw new DuplicateDependencyException();

            _requiredAnySteps.AddRange(types);
        }


        public IEnumerable<Type> RequiredSteps
        {
            get { return _requiredSteps; }
        }

        public IEnumerable<Type> RequiredAnySteps
        {
            get { return _requiredAnySteps; }
        }
    }
}
