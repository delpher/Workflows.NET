using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflows.DependencyExplorer
{
    internal class DependencyDictionaryExplorer : IDependencyExplorer
    {
        readonly Dictionary<Type, DependencyCollection> _dependencyDictionary = new Dictionary<Type, DependencyCollection>();

        public IDependencyBuilder Step(Type step)
        {
            if (!_dependencyDictionary.ContainsKey(step))
                _dependencyDictionary.Add(step, new DependencyCollection());

            return new DependencyBuilder(_dependencyDictionary[step]);
        }

        public virtual bool Requires(Type dependent, Type required)
        {
            if (_dependencyDictionary.ContainsKey(dependent))
            {
                return _dependencyDictionary[dependent].RequiredSteps.Any(x => x == required) ||
                       _dependencyDictionary[dependent].RequiredAnySteps.Any(x => x == required);
            }

            return false;
        }

        public virtual bool HasRequired(Type dependent)
        {
            return _dependencyDictionary.ContainsKey(dependent);
        }

        private class DependencyBuilder : IDependencyBuilder
        {
            private readonly DependencyCollection _dependencyCollection;

            public DependencyBuilder(DependencyCollection collection)
            {
                _dependencyCollection = collection;
            }

            public IDependencyBuilder Requires(params Type[] types)
            {
                _dependencyCollection.AddRequiredSteps(types);
                return this;
            }

            public IDependencyBuilder RequiresAny(params Type[] types)
            {
                _dependencyCollection.AddRequiredAnySteps(types);
                return this;
            }
        }
    }
}
