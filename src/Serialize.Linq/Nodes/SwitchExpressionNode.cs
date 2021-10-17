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
    [DataContract(Name = "SE")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class SwitchExpressionNode
        : ExpressionNode<SwitchExpression>
    {
        public SwitchExpressionNode(INodeFactory factory, SwitchExpression expression)
            : base(factory, expression) { }

        [DataMember(EmitDefaultValue = false)]
        public SwitchCaseNodeList Cases { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public MethodInfoNode Comparison { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode DefaultBody { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode SwitchValue { get; set; }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.Switch(Type?.ToType(context),
                SwitchValue.ToExpression(context),
                DefaultBody?.ToExpression(context),
                Comparison?.ToMemberInfo(context),
                Cases.GetSwitchCases(context));
        }

        protected override void Initialize(SwitchExpression expression)
        {
            this.Cases = new SwitchCaseNodeList(Factory, expression.Cases);
            this.Comparison = new MethodInfoNode(Factory, expression.Comparison);
            this.DefaultBody = Factory.Create(expression.DefaultBody);
            this.SwitchValue = Factory.Create(expression.SwitchValue);
        }
    }
}
