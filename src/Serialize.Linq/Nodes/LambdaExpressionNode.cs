#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

#if !WINDOWS_PHONE7 && !UAP10_0
using System;
#endif
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "L")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class LambdaExpressionNode : ExpressionNode<LambdaExpression>
    {
        public LambdaExpressionNode() { }

        public LambdaExpressionNode(INodeFactory factory, LambdaExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "B")]
#endif
        #endregion
        public ExpressionNode Body { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "P")]
#endif
        #endregion
        public ExpressionParameterNodeList<Expression, ExpressionNode> Parameters { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "TC")]
#endif
        #endregion
        public bool TailCall { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "N")]
#endif
        #endregion
        public string Name { get; set; }

        protected override void Initialize(LambdaExpression expression)
        {
#if !WINDOWS_PHONE7
            this.Parameters = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Parameters);
#else
            this.Parameters = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Parameters.Select(p => (Expression)p));
#endif
            this.Body = this.Factory.Create(expression.Body);
            this.TailCall = expression.TailCall;
            this.Name = expression.Name;
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Lambda(this.Type.ToType(context),
                                     this.Body.ToExpression(context),
                                     this.Name,
                                     this.TailCall,
                                     this.Parameters.ToParameters(context).Cast<ParameterExpression>());
        }
    }
}
