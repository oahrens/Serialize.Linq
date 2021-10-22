#if !WINDOWS_UWP
using System;
#endif
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "GO")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class GotoExpressionNode
        : ExpressionNode<GotoExpression>
    {
        public GotoExpressionNode() { }

        public GotoExpressionNode(INodeFactory factory, GotoExpression expression)
            : base(factory, expression.NodeType, expression.Target.Type)
        {
            this.Kind = expression.Kind;
            this.Target = Factory.Create(expression.Target);
            this.Value = Factory.Create(expression.Value);
        }

        [DataMember(EmitDefaultValue = false)]
        public GotoExpressionKind Kind { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public LabelTargetNode Target { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Value { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.MakeGoto(Kind, Target.ToLabelTarget(context), Value?.ToExpression(context), Type.ToType(context));
        }

        protected override void Initialize(GotoExpression expression)
        {
            this.Kind = expression.Kind;
            this.Target = Factory.Create(expression.Target);
            this.Value = Factory.Create(expression.Value);
        }
    }
}
