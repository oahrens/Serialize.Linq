﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

#if !WINDOWS_UWP
using System;
#endif
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "X")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class IndexExpressionNode : ExpressionNode<IndexExpression>
    {
        public IndexExpressionNode() { }

        public IndexExpressionNode(INodeFactory factory, IndexExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "A")]
#endif
        #endregion
        public ExpressionParameterNodeList<Expression, ExpressionNode> Arguments { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "I")]
#endif
        #endregion
        public PropertyInfoNode Indexer { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "O")]
#endif
        #endregion
        public ExpressionNode Object { get; set; }

        protected override void Initialize(IndexExpression expression)
        {
            this.Arguments = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Arguments);
            this.Indexer = new PropertyInfoNode(this.Factory, expression.Indexer);
            this.Object = this.Factory.Create(expression.Object);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.MakeIndex(this.Object.ToExpression(context),
                                        (PropertyInfo)this.Indexer.ToParameter(context),
                                        this.Arguments.ToParameters(context));
        }
    }
}
