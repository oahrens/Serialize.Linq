#if !WINDOWS_UWP
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
    [DataContract(Name = "BLO")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class BlockExpressionNode : ExpressionNode<BlockExpression>
    {
        public BlockExpressionNode() { }

        public BlockExpressionNode(INodeFactory factory, BlockExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionParameterNodeList<Expression, ExpressionNode> Expressions { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Result { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionParameterNodeList<Expression, ExpressionNode> Variables { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Block(Variables.ToParameters(context).Cast<ParameterExpression>(),
                                    Expressions.ToParameters(context));
        }

        protected override void Initialize(BlockExpression expression)
        {
            this.Expressions = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Expressions);
            this.Result = Factory.Create(expression.Result);
            this.Variables = new ExpressionParameterNodeList<Expression, ExpressionNode>(this.Factory, expression.Variables);
        }
    }
}
