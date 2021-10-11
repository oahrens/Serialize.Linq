using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Serialize.Linq.Internals
{
    internal class ComplexPropertyMemberTypeFinder
    {
        private ISet<Type> _referencedTypes;
        private readonly ISet<Type> _seenTypes = new HashSet<Type>();
        private readonly IEnumerable<Type> _baseTypes;

        public ComplexPropertyMemberTypeFinder(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (types.Any(type => type == null))
                throw new ArgumentNullException(nameof(types));
            _baseTypes = types;
        }

        public IEnumerable<Type> GetReferencedTypes()
        {
            if (_referencedTypes == null)
            {
                _referencedTypes = new HashSet<Type>();
                foreach (var type in _baseTypes)
                {
                    AddTypes(type);
                }
            }
            return _referencedTypes;
        }

        private void AddTypes(Type type)
        {
            if (_seenTypes.Add(type))
            {
                AddElementGenericInterfaceAndBaseTypes(type);
                var enumerator = new ComplexPropertyMemberTypeEnumerator(_referencedTypes, type, BindingFlags.Instance | BindingFlags.Public);
                if (enumerator.IsConsideredType(type) && _referencedTypes.Add(type))
                {
                    foreach (var propertyType in new ComplexPropertyMemberTypeEnumerator(_referencedTypes, type, BindingFlags.Instance | BindingFlags.Public).GetReferencedTypes())
                        AddTypes(propertyType);
                }
            }
        }

        private void AddElementGenericInterfaceAndBaseTypes(Type type)
        {
            if (type.HasElementType)
            {
                AddTypes(type.GetElementType());
            }

            if (type.IsGenericType())
            {
                foreach (var genericType in type.GetGenericArguments())
                    AddTypes(genericType);
            }

            foreach (var interfaceType in type.GetInterfaces())
                AddTypes(interfaceType);

            var baseType = type.GetBaseType();
            if (baseType != null && baseType != typeof(object))
                AddTypes(baseType);
        }
    }

}
