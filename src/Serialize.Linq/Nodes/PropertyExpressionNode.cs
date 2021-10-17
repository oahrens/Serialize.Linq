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
    [DataContract]
#else
    [DataContract(Name = "PE")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
#endregion
    public sealed class PropertyExpressionNode : MemberExpressionNode<PropertyInfo>
    {
        public PropertyExpressionNode(INodeFactory factory, MemberExpression expression) 
            : base(factory, expression)
        {
            Member = Factory.Create((PropertyInfo)expression.Member);
        }
    }
}
