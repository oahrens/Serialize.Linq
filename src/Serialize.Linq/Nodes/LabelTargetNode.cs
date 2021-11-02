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
    [DataContract(Name = "LT")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class LabelTargetNode : Node, IExpressionParameterNode<LabelTarget>
    {
        public LabelTargetNode() { }

        public LabelTargetNode(INodeFactory factory, LabelTarget target, string name)
            : base(factory)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            this.Name = name;
            this.Type = this.Factory.Create(target.Type);
        }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TypeNode Type { get; set; }

        public LabelTarget ToParameter(IExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            return context.GetLabelTarget(this);
        }
    }
}
