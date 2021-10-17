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
    public class SwitchCaseNode 
        : Node
    {
        public SwitchCaseNode(INodeFactory factory, SwitchCase switchCase) 
            : base(factory)
        {
            if (switchCase == null)
                throw new ArgumentNullException(nameof(switchCase));

            TestValues = new ExpressionNodeList(Factory, switchCase.TestValues);
            Body = Factory.Create(switchCase.Body);
        }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNodeList TestValues { get; set; }

        public SwitchCase ToSwitchCase(IExpressionContext context)
        {
            return Expression.SwitchCase(Body.ToExpression(context), TestValues.GetExpressions(context));
        }
    }
}
