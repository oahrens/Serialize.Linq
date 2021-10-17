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
    [DataContract(Name = "LO")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class LoopExpressionNode
        : ExpressionNode<LoopExpression>
    {
        public LoopExpressionNode(INodeFactory factory, LoopExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public LabelTargetNode BreakLabel { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public LabelTargetNode ContinueLabel { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Loop(Body.ToExpression(context), BreakLabel?.ToLabelTarget(context), ContinueLabel?.ToLabelTarget(context));
        }

        protected override void Initialize(LoopExpression expression)
        {
            this.Body = Factory.Create(expression.Body);
            this.BreakLabel = Factory.Create(expression.BreakLabel);
            this.ContinueLabel = Factory.Create(expression.ContinueLabel);
        }
    }
}
