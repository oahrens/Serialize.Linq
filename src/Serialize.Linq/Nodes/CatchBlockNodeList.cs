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
    [CollectionDataContract(Name = "CBL")]
#endif
#if !WINDOWS_UWP
    [Serializable]
#endif
    #endregion
    public class CatchBlockNodeList
        : List<CatchBlockNode>
    {
        public CatchBlockNodeList() { }

        /// <summary>
        ///     ''' <paramref name="factory"/> ist obligat. <paramref name="items"/> kann Null sein.
        ///     ''' </summary>
        public CatchBlockNodeList(INodeFactory factory, IEnumerable<CatchBlock> items)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (items != null)
                this.AddRange(items.Select(item => new CatchBlockNode(factory, item)));
        }

        public IEnumerable<CatchBlock> GetCatchBlocks(IExpressionContext context)
        {
            return this.Select(item => item.ToCatchBlock(context));
        }
    }
}
