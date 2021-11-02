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
    [DataContract(Name = "SE")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class SwitchExpressionNode
        : ExpressionNode<SwitchExpression>
    {
        public SwitchExpressionNode() { }

        public SwitchExpressionNode(INodeFactory factory, SwitchExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionParameterNodeList<SwitchCase, SwitchCaseNode> Cases { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MethodInfoNode Comparison { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode DefaultBody { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode SwitchValue { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Switch(this.Type?.ToType(context),
                                     this.SwitchValue.ToExpression(context),
                                     this.DefaultBody?.ToExpression(context),
                                     (MethodInfo)this.Comparison?.ToParameter(context),
                                     this.Cases.ToParameters(context));
        }

        protected override void Initialize(SwitchExpression expression)
        {
            this.Cases = new ExpressionParameterNodeList<SwitchCase, SwitchCaseNode>(this.Factory, expression.Cases);
            this.Comparison = new MethodInfoNode(this.Factory, expression.Comparison);
            this.DefaultBody = this.Factory.Create(expression.DefaultBody);
            this.SwitchValue = this.Factory.Create(expression.SwitchValue);
        }
    }
}
