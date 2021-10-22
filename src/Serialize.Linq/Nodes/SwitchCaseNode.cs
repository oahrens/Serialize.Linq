using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "SC")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class SwitchCaseNode : Node, IExpressionParameterNode<SwitchCase>
    {
        public SwitchCaseNode() { }

        public SwitchCaseNode(INodeFactory factory, SwitchCase switchCase)
            : base(factory)
        {
            if (switchCase == null)
                throw new ArgumentNullException(nameof(switchCase));

            TestValues = new ExpressionParameterNodeList<Expression, ExpressionNode>(Factory, switchCase.TestValues);
            Body = Factory.Create(switchCase.Body);
        }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionParameterNodeList<Expression, ExpressionNode> TestValues { get; set; }

        public SwitchCase ToParameter(IExpressionContext context)
        {
            return Expression.SwitchCase(Body.ToExpression(context), TestValues.ToParameters(context));
        }
    }
}
