#region Copyright
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
    [DataContract(Name = "M")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class MemberExpressionNode<TMemberInfo> : ExpressionNode<MemberExpression> where TMemberInfo : MemberInfo
    {
        public MemberExpressionNode() { }

        public MemberExpressionNode(INodeFactory factory, MemberExpression expression)
            : base(factory, expression) { }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "E")]
#endif
        #endregion
        public ExpressionNode Expression { get; set; }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberNode<TMemberInfo> Member { get; set; }

        protected override void Initialize(MemberExpression expression)
        {
            this.Expression = this.Factory.Create(expression.Expression);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            var member = this.Member.ToParameter(context);
            return System.Linq.Expressions.Expression.MakeMemberAccess(Expression?.ToExpression(context), member);
        }
    }
}
