#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    #region DataContract
    [DataContract]
#if !WINDOWS_UWP
    [Serializable]
#endif
    #region KnownTypes
    [KnownType(typeof(BinaryExpressionNode))]
    [KnownType(typeof(BlockExpressionNode))]
    [KnownType(typeof(CatchBlockNode))]
    [KnownType(typeof(ConditionalExpressionNode))]
    [KnownType(typeof(ConstantExpressionNode))]
    [KnownType(typeof(ConstructorInfoNode))]
    [KnownType(typeof(ElementInitNode))]
    [KnownType(typeof(ExpressionNode))]
    [KnownType(typeof(FieldInfoNode))]
    [KnownType(typeof(GotoExpressionNode))]
    [KnownType(typeof(IndexExpressionNode))]
    [KnownType(typeof(InvocationExpressionNode))]
    [KnownType(typeof(LabelExpressionNode))]
    [KnownType(typeof(LabelTargetNode))]
    [KnownType(typeof(LambdaExpressionNode))]
    [KnownType(typeof(ListInitExpressionNode))]
    [KnownType(typeof(LoopExpressionNode))]
    [KnownType(typeof(MemberAssignmentNode))]
    [KnownType(typeof(MemberBindingNode))]
    [KnownType(typeof(MemberExpressionNode))]
    [KnownType(typeof(MemberInfoNode))]
    [KnownType(typeof(MemberInitExpressionNode))]
    [KnownType(typeof(MemberListBindingNode))]
    [KnownType(typeof(MemberMemberBindingNode))]
    [KnownType(typeof(MethodCallExpressionNode))]
    [KnownType(typeof(NewArrayExpressionNode))]
    [KnownType(typeof(NewExpressionNode))]
    [KnownType(typeof(ParameterExpressionNode))]
    [KnownType(typeof(PropertyInfoNode))]
    [KnownType(typeof(SwitchExpressionNode))]
    [KnownType(typeof(SwitchCaseNode))]
    [KnownType(typeof(TryExpressionNode))]
    [KnownType(typeof(TypeBinaryExpressionNode))]
    [KnownType(typeof(TypeNode))]
    [KnownType(typeof(UnaryExpressionNode))]
    #endregion
    #endregion
    public abstract class Node
    {
        [IgnoreDataMember]
#if !WINDOWS_UWP
        [NonSerialized]
#endif
        private readonly INodeFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        protected Node() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        protected Node(INodeFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        public INodeFactory Factory => _factory;
    }
}
