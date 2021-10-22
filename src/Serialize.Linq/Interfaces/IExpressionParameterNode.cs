namespace Serialize.Linq.Interfaces
{
    public interface IExpressionParameterNode<TParameter> where TParameter : class
    {
        TParameter ToParameter(IExpressionContext context);
    }
}
