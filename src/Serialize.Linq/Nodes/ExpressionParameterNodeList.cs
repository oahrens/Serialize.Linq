using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region CollectionDataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [CollectionDataContract]
#else
    [CollectionDataContract(Name = "CBL")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public sealed class ExpressionParameterNodeList<TParameter, TParameterNode>
        : List<TParameterNode> where TParameter : class where TParameterNode : IExpressionParameterNode<TParameter>
    {
        public ExpressionParameterNodeList() { }

        public ExpressionParameterNodeList(INodeFactory factory, IEnumerable<TParameter> items)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (items != null)
                this.AddRange(items.Select(p => factory.CreateParameterNode<TParameter>(p)).Cast<TParameterNode>());
        }

        public IEnumerable<TParameter> ToParameters(IExpressionContext context)
        {
            return this.Select(n => n.ToParameter(context));
        }
    }
}
