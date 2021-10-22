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
    [DataContract(Name = "TE")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
#endregion
    public class TryExpressionNode
        : ExpressionNode<TryExpression>
    {
        public TryExpressionNode() { }

        public TryExpressionNode(INodeFactory factory, TryExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Fault { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Finally { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionParameterNodeList<CatchBlock, CatchBlockNode> Handlers { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.MakeTry(Type?.ToType(context),
                Body?.ToExpression(context),
                Finally?.ToExpression(context),
                Fault?.ToExpression(context),
                Handlers?.ToParameters(context));
        }

        protected override void Initialize(TryExpression expression)
        {
            this.Body = Factory.Create(expression.Body);
            this.Fault = Factory.Create(expression.Fault);
            this.Finally = Factory.Create(expression.Finally);
            this.Handlers = new ExpressionParameterNodeList<CatchBlock, CatchBlockNode>(Factory, expression.Handlers);
        }
    }
}
