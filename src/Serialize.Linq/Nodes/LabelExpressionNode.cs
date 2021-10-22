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
    [DataContract(Name = "LE")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class LabelExpressionNode
        : ExpressionNode<LabelExpression>
    {
        public LabelExpressionNode() { }

        public LabelExpressionNode(INodeFactory factory, LabelExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode DefaultValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public LabelTargetNode Target { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Label(Target.ToLabelTarget(context), DefaultValue?.ToExpression(context));
        }

        protected override void Initialize(LabelExpression expression)
        {
            this.Target = Factory.Create(expression.Target);
            this.DefaultValue = Factory.Create(expression.DefaultValue);
        }
    }
}
