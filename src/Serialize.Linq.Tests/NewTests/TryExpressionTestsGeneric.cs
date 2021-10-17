using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.tryexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class TryExpressionTestsGeneric
    {
        [TestMethod]
        public void SerializeDeserializeTryExpression()
        {
            SerializeDeserializeTryExpressionInternal(new BinarySerializer());
            SerializeDeserializeTryExpressionInternal(new XmlSerializer());
            SerializeDeserializeTryExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeTryExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            TryExpression tryCatchExpr = Expression.TryCatch(Expression.Block(Expression.Throw(Expression.Constant(new DivideByZeroException())),
                                                                              Expression.Constant("Try block")),
                                                             Expression.Catch(typeof(DivideByZeroException),
                                                                              Expression.Constant("Catch block")));
            var expression = Expression.Lambda<Func<string>>(tryCatchExpr);

            serializer.AddKnownTypes(new[] { typeof(DivideByZeroException) });
            var value = serializer.SerializeGeneric(expression);
            var actualExpression = (Expression<Func<string>>)serializer.DeserializeGeneric(value);

            Assert.AreEqual(expression.GetDebugView(), actualExpression.GetDebugView());

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            Debug.WriteLine(action.Invoke());
            Debug.WriteLine(actualAction.Invoke());

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
