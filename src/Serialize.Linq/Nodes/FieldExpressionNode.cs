#if !WINDOWS_UWP
using System;
#endif
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
#region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
#else
    [DataContract(Name = "F")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
#endregion
    public class FieldExpressionNode : MemberExpressionNode<FieldInfo>
    {
        public FieldExpressionNode() { }

        public FieldExpressionNode(INodeFactory factory, MemberExpression expression) 
            : base(factory, expression)
        {
            base.Member = Factory.Create((FieldInfo)expression.Member);
        }
    }
}
