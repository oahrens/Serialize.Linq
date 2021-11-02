#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
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
    [DataContract(Name = "MB")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public abstract class MemberBindingNode : Node, IExpressionParameterNode<MemberBinding>
    {
        protected MemberBindingNode() { }

        protected MemberBindingNode(INodeFactory factory)
            : base(factory) { }

        protected MemberBindingNode(INodeFactory factory, MemberBindingType bindingType, MemberInfo memberInfo)
            : base(factory)
        {
            this.BindingType = bindingType;
            if (memberInfo is FieldInfo field)
                this.Member = new FieldInfoNode(this.Factory, field);
            else if (memberInfo is PropertyInfo property)
                this.Member = new PropertyInfoNode(this.Factory, property);
            else
                throw new ArgumentOutOfRangeException(nameof(memberInfo), "Not supported derived type of type " + nameof(MemberInfo));
        }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "BT")]
#endif
        #endregion
        public MemberBindingType BindingType { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberInfoNode Member { get; set; }

        public abstract MemberBinding ToParameter(IExpressionContext context);
    }
}
