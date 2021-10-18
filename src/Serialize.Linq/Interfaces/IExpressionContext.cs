using System;
using System.Linq.Expressions;
using System.Reflection;
using Serialize.Linq.Nodes;

namespace Serialize.Linq.Interfaces
{
    public interface IExpressionContext
    {
        BindingFlags BindingFlags { get; }

        BindingFlags? GetBindingFlags();

        ParameterExpression GetParameterExpression(ParameterExpressionNode node);

        LabelTarget GetLabelTarget(TypeNode type, string name, int id);

        Type ResolveType(TypeNode node);
    }
}
