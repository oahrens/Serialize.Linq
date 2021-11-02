#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Nodes;

namespace Serialize.Linq.Factories
{
    public class NodeFactory
        : INodeFactory
    {
        private readonly IDictionary<LabelTarget, LabelTargetNode> _labelTargets = new Dictionary<LabelTarget, LabelTargetNode>();
        private int _labelTargetCtr;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFactory"/> class.
        /// </summary>
        public NodeFactory()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFactory"/> class.
        /// </summary>
        /// <param name="factorySettings">The factory settings to use.</param>
        public NodeFactory(FactorySettings factorySettings)
        {
            this.Settings = factorySettings ?? new FactorySettings();
        }

        public FactorySettings Settings { get; }

        /// <summary>
        /// Creates an expression node from an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown expression of type  + expression.GetType()</exception>
        public virtual ExpressionNode Create(Expression expression)
        {
            if (expression == null)
                return null;

            if (expression is BinaryExpression binaryExpression) return new BinaryExpressionNode(this, binaryExpression);
            if (expression is BlockExpression blockExpression) return new BlockExpressionNode(this, blockExpression);
            if (expression is ConditionalExpression conditionalExpression) return new ConditionalExpressionNode(this, conditionalExpression);
            if (expression is ConstantExpression constantExpression) return new ConstantExpressionNode(this, constantExpression);
            if (expression is GotoExpression gotoExpression) return new GotoExpressionNode(this, gotoExpression);
            if (expression is InvocationExpression invocationExpression) return new InvocationExpressionNode(this, invocationExpression);
            if (expression is IndexExpression indexExpression) return new IndexExpressionNode(this, indexExpression);
            if (expression is LabelExpression labelExpression) return new LabelExpressionNode(this, labelExpression);
            if (expression is LambdaExpression lambdaExpression) return new LambdaExpressionNode(this, lambdaExpression);
            if (expression is ListInitExpression listInitExpression) return new ListInitExpressionNode(this, listInitExpression);
            if (expression is LoopExpression loopExpression) return new LoopExpressionNode(this, loopExpression);
            if (expression is MemberExpression memberExpression) return new MemberExpressionNode(this, memberExpression);
            if (expression is MemberInitExpression memberInitExpression) return new MemberInitExpressionNode(this, memberInitExpression);
            if (expression is MethodCallExpression methodCallExpression) return new MethodCallExpressionNode(this, methodCallExpression);
            if (expression is NewArrayExpression newArrayExpression) return new NewArrayExpressionNode(this, newArrayExpression);
            if (expression is NewExpression newExpression) return new NewExpressionNode(this, newExpression);
            if (expression is ParameterExpression parameterExpression) return new ParameterExpressionNode(this, parameterExpression);
            if (expression is SwitchExpression switchExpression) return new SwitchExpressionNode(this, switchExpression);
            if (expression is TryExpression tryExpression) return new TryExpressionNode(this, tryExpression);
            if (expression is TypeBinaryExpression typeBinaryExpression) return new TypeBinaryExpressionNode(this, typeBinaryExpression);
            if (expression is UnaryExpression unaryExpression) return new UnaryExpressionNode(this, unaryExpression);

            throw new ArgumentException("Unknown expression of type " + expression.GetType());
        }

        /// <summary>
        /// Creates an type node from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public TypeNode Create(Type type)
        {
            return new TypeNode(this, type);
        }

        public virtual LabelTargetNode Create(LabelTarget target)
        {

            if (target == null)
                return null;
            else
            {
                if (!_labelTargets.TryGetValue(target, out var targetNode))
                {
                    targetNode = new LabelTargetNode(this, target, this.GetLabelTargetName(target.Name));
                    _labelTargets.Add(target, targetNode);
                    _labelTargetCtr += 1;
                }
                return targetNode;
            }
        }

        public Node CreateParameterNode<TParameter>(TParameter parameter) where TParameter : class
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            if (parameter is CatchBlock catchBlock) return new CatchBlockNode(this, catchBlock);
            if (parameter is SwitchCase switchCase) return new SwitchCaseNode(this, switchCase);
            if (parameter is ElementInit elementInit) return new ElementInitNode(this, elementInit);
            if (parameter is FieldInfo fieldInfo) return new FieldInfoNode(this, fieldInfo);
            if (parameter is MemberAssignment assignment) return new MemberAssignmentNode(this, assignment);
            if (parameter is MemberListBinding listBinding) return new MemberListBindingNode(this, listBinding);
            if (parameter is MemberMemberBinding memberMemberBinding) return new MemberMemberBindingNode(this, memberMemberBinding);
            if (parameter is PropertyInfo propertyInfo) return new PropertyInfoNode(this, propertyInfo);
            if (parameter is Expression expression) return this.Create(expression);

            throw new ArgumentException($"Unknown expression of type {parameter.GetType()}");
        }

        /// <summary>
        /// Gets binding flags to be used when accessing type members.
        /// </summary>
        [Obsolete("This function is just for compatibility. Please use NodeFactory.Settings.BindingFlags instead.", false)]
        public BindingFlags? GetBindingFlags()
        {
            if (!this.Settings.AllowPrivateFieldAccess)
                return null;

            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }

        private string GetLabelTargetName(string labelTargetName)
        {
            return (labelTargetName ?? String.Empty) + "#" + _labelTargetCtr.ToString();
        }
    }
}