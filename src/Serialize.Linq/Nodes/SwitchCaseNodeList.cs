using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region CollectionDataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [CollectionDataContract]
#else
    [CollectionDataContract(Name = "SCL")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class SwitchCaseNodeList
        : List<SwitchCaseNode>
    {
        public SwitchCaseNodeList() { }
        public SwitchCaseNodeList(INodeFactory factory, IEnumerable<SwitchCase> items)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (items != null)
                this.AddRange(items.Select(item => new SwitchCaseNode(factory, item)));
        }

        public IEnumerable<SwitchCase> GetSwitchCases(IExpressionContext context)
        {
            return this.Select(memberBindingEntity => memberBindingEntity.ToSwitchCase(context));
        }
    }
}
