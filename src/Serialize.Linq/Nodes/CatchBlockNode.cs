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
    [DataContract(Name = "CB")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class CatchBlockNode 
        : Node
    {
        public CatchBlockNode() { }

        public CatchBlockNode(INodeFactory factory, CatchBlock catchBlock) 
            : base(factory)
        {
            if (catchBlock == null)
                throw new ArgumentNullException(nameof(catchBlock));

            Body = Factory.Create(catchBlock.Body);
            Filter = Factory.Create(catchBlock.Filter);
            Test = Factory.Create(catchBlock.Test);
            Variable = (ParameterExpressionNode)Factory.Create(catchBlock.Variable);
        }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Body { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ExpressionNode Filter { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TypeNode Test { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ParameterExpressionNode Variable { get; set; }

        public CatchBlock ToCatchBlock(IExpressionContext vcontext)
        {
            return Expression.MakeCatchBlock(Test?.ToType(vcontext), 
                                             (ParameterExpression)Variable?.ToExpression(vcontext), 
                                             Body?.ToExpression(vcontext), 
                                             Filter?.ToExpression(vcontext));
        }
    }
}
