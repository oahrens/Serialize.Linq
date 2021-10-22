#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

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
    [DataContract(Name = "MMB")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
#endregion
    public class MemberMemberBindingNode : MemberBindingNode
    {
        public MemberMemberBindingNode() { }

        public MemberMemberBindingNode(INodeFactory factory, MemberMemberBinding memberMemberBinding)
            : base(factory, memberMemberBinding.BindingType, memberMemberBinding.Member)
        {
            this.Bindings = new ExpressionParameterNodeList<MemberBinding, MemberBindingNode>(factory, memberMemberBinding.Bindings);
        }

#region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "B")]
#endif
#endregion
        public ExpressionParameterNodeList<MemberBinding, MemberBindingNode> Bindings { get; set; }

        public override MemberBinding ToParameter(IExpressionContext context)
        {
            return Expression.MemberBind(this.Member.ToParameter(context), this.Bindings.ToParameters(context));
        }
    }
}
