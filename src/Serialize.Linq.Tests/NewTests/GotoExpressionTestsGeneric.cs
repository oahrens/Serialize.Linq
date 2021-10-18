using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/de-de/dotnet/api/system.linq.expressions.gotoexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class GotoExpressionTestsGeneric
    {
        [TestMethod]
        public void SerializeDeserializeGotoExpression()
        {
            SerializeDeserializeGotoExpressionInternal(new BinarySerializer());
            SerializeDeserializeGotoExpressionInternal(new XmlSerializer());
            SerializeDeserializeGotoExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeGotoExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            LabelTarget returnTarget = Expression.Label();
            LabelTarget fakeTarget0 = Expression.Label("#Label2");
            LabelTarget fakeTarget1 = Expression.Label();
            BlockExpression expression = Expression.Block(Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("GoTo")),
                                                          Expression.Goto(returnTarget),
                                                          Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("Other Work")),
                                                          Expression.Label(fakeTarget0),
                                                          Expression.Label(returnTarget),
                                                          Expression.Label(fakeTarget1));


            var value = serializer.SerializeGeneric(expression);
            var actualExpression = (BlockExpression)serializer.DeserializeGeneric(value);

            Assert.AreEqual(expression.GetDebugView(), actualExpression.GetDebugView());

            var action = Expression.Lambda<Action>(expression).Compile();
            var actualAction = Expression.Lambda<Action>(actualExpression).Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
