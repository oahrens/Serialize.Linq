#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serialize.Linq.Extensions;

namespace Serialize.Linq.Internals
{
    internal abstract class MemberTypeEnumerator
    {
        private readonly Type _type;
        private readonly BindingFlags _bindingFlags;
        private readonly ISet<Type> _seenTypes;
        private ISet<Type> _referencedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeEnumerator"/> class.
        /// </summary>
        /// <param name="seenTypes">The seen types.</param>
        /// <param name="type">The type.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <exception cref="System.ArgumentNullException">
        /// seenTypes
        /// or
        /// type
        /// </exception>
        public MemberTypeEnumerator(ISet<Type> seenTypes, Type type, BindingFlags bindingFlags)
        {
            _seenTypes = seenTypes ?? throw new ArgumentNullException(nameof(seenTypes));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _bindingFlags = bindingFlags;
        }

        /// <summary>
        /// Determines whether [is considered type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is considered type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsConsideredType(Type type);

        /// <summary>
        /// Determines whether [is considered member] [the specified member].
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>
        ///   <c>true</c> if [is considered member] [the specified member]; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool IsConsideredMember(MemberInfo member);

        public IEnumerable<Type> GetReferencedTypes()
        {
            if (_referencedTypes == null)
            {
                _referencedTypes = new HashSet<Type>();
                foreach (var propertyType in from memberInfo in _type.GetMembers(_bindingFlags)
                                             where IsConsideredMember(memberInfo)
                                             select memberInfo.GetReturnType())
                {
                    if (!_seenTypes.Contains(propertyType) && IsConsideredType(propertyType))
                    {
                        _referencedTypes.Add(propertyType);
                    }
                }
            }
            return _referencedTypes;
        }
    }
}