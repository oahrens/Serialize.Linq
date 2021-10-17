using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.loopexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class LoopExpressionTests
    {
        [TestMethod]
        public void SerializeDeserializeLoopBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
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

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeBinary(expression);
            var actualExpression = (Expression<Func<int, int>>)serializer.DeserializeBinary(value);

            var func = expression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(func.Invoke(5), actualFunc.Invoke(5));
            Assert.AreEqual(actualFunc.Invoke(5), 120);
        }

        [TestMethod]
        public void SerializeDeserializeLoopXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
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

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<int, int>>)serializer.DeserializeText(value);

            var func = expression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(func.Invoke(5), actualFunc.Invoke(5));
            Assert.AreEqual(actualFunc.Invoke(5), 120);
        }

        [TestMethod]
        public void SerializeDeserializeLoopJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
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

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<int, int>>)serializer.DeserializeText(value);

            var func = expression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(func.Invoke(5), actualFunc.Invoke(5));
            Assert.AreEqual(actualFunc.Invoke(5), 120);
        }
    }
}
