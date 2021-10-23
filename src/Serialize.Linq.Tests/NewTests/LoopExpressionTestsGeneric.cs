using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;
using Serialize.Linq.Tests.Internals;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.loopexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class LoopExpressionTestsGeneric
    {
        [TestMethod]
        public void SerializeDeserializeLoopExpression()
        {
            SerializeDeserializeLoopExpressionInternal(new BinarySerializer());
            SerializeDeserializeLoopExpressionInternal(new XmlSerializer());
            SerializeDeserializeLoopExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeLoopExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            ParameterExpression valueParameter = Expression.Parameter(typeof(int), "value");
            ParameterExpression resultParameter = Expression.Parameter(typeof(int), "result");
            LabelTarget label = Expression.Label(typeof(int));

            BlockExpression blockExpression = Expression.Block(new[] { resultParameter },
                                                               Expression.Assign(resultParameter, Expression.Constant(1)),
                                                               Expression.Loop(Expression.IfThenElse(Expression.GreaterThan(valueParameter, Expression.Constant(1)),
                                                                                                     Expression.MultiplyAssign(resultParameter,
                                                                                                                               Expression.PostDecrementAssign(valueParameter)),
                                                                                                     Expression.Break(label, resultParameter)),
                                                                               label));

            var expression = Expression.Lambda<Func<int, int>>(blockExpression, valueParameter);

            var value = serializer.SerializeGeneric(expression);
            var actualExpression = (Expression<Func<int, int>>)serializer.DeserializeGeneric(value);

            Assert.AreEqual(expression.GetDebugView(), actualExpression.GetDebugView());
            var comparer = new ExpressionComparer();
            Assert.IsTrue(comparer.AreEqual(expression, actualExpression));

            var func = expression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(func.Invoke(5), actualFunc.Invoke(5));
            Assert.AreEqual(actualFunc.Invoke(5), 120);
        }
    }
}
