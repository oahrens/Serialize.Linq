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
using System.Linq;
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
    [DataContract(Name = "N")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class NewExpressionNode : ExpressionNode<NewExpression>
    {
        public NewExpressionNode() { }

        public NewExpressionNode(INodeFactory factory, NewExpression expression)
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
        [DataMember(EmitDefaultValue = false, Name = "C")]
#endif
        #endregion
        public ConstructorInfoNode Constructor { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public ExpressionParameterNodeList<MemberInfo, MemberInfoNode> Members { get; set; }

        protected override void Initialize(NewExpression expression)
        {
            this.Arguments = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Arguments);
            this.Constructor = new ConstructorInfoNode(this.Factory, expression.Constructor);
            this.Members = new ExpressionParameterNodeList<MemberInfo, MemberInfoNode>(this.Factory, expression.Members);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            if (this.Constructor == null)
                return Expression.New(this.Type.ToType(context));

            var constructor = this.Constructor.ToParameter(context);
            if (constructor == null)
                return Expression.New(this.Type.ToType(context));

            var arguments = this.Arguments.ToParameters(context);
            var members = this.Members?.ToParameters(context).ToArray() ?? null;
            return members != null && members.Length > 0 ? Expression.New(constructor, arguments, members) : Expression.New(constructor, arguments);
        }
    }
}
