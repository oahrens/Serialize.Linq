﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Nodes;

namespace Serialize.Linq
{
    public abstract class ExpressionContextBase : IExpressionContext
    {
        private readonly IDictionary<ParameterExpressionNode, ParameterExpression> _parameterExpressions =
            new Dictionary<ParameterExpressionNode, ParameterExpression>(new ParameterExpressionNodeComparer());

        private readonly IDictionary<string, Type> _typeCache = new Dictionary<string, Type>();

        private readonly IDictionary<int, LabelTarget> _labelTargets = new Dictionary<int, LabelTarget>();

        protected ExpressionContextBase() { }

        protected ExpressionContextBase(bool allowPrivateFieldAccess)
        {
            AllowPrivateFieldAccess = allowPrivateFieldAccess;
        }

        public bool AllowPrivateFieldAccess { get; set; }

        public virtual BindingFlags BindingFlags => AllowPrivateFieldAccess ? Constants.ALSO_NON_PUBLIC_BINDING : Constants.PUBLIC_ONLY_BINDING;

        [Obsolete("This function is just for compatibility. Please use ExpressionContext.BindingFlags instead.", false)]
        public virtual BindingFlags? GetBindingFlags()
        {
            if (!this.AllowPrivateFieldAccess)
                return null;

            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }

        public virtual ParameterExpression GetParameterExpression(ParameterExpressionNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!_parameterExpressions.TryGetValue(node, out var nodeExpression))
            {
                nodeExpression = Expression.Parameter(node.Type.ToType(this), node.Name);
                _parameterExpressions.Add(node, nodeExpression);
            }

            return nodeExpression;
        }

        public LabelTarget GetLabelTarget(TypeNode type, string name, int id)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!_labelTargets.TryGetValue(id, out var target))
            {
                target = Expression.Label(type.ToType(this), name);
                _labelTargets.Add(id, target);
            }
            return target;
        }

        public virtual Type ResolveType(TypeNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (String.IsNullOrWhiteSpace(node.Name))
                return null;
            else
            {
                if (!_typeCache.TryGetValue(node.Name, out var type))
                {
                    if ((type = Type.GetType(node.Name)) == null)
                    {
                        using (IEnumerator<Assembly> enumerator = this.GetAssemblies().GetEnumerator())
                        {
                            while (enumerator.MoveNext() && type == null)
                                type = enumerator.Current.GetType(node.Name);
                        }
                    }
                    _typeCache.Add(node.Name, type);
                }

                return type;
            }
        }

        protected abstract IEnumerable<Assembly> GetAssemblies();
    }
}
