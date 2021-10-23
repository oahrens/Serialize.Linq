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
    /// Test and fix provided by https://github.com/oahrens
    /// </summary>
    [TestClass]
    public class ToStringTestsGeneric
    {
        [TestMethod]
        public void SerializeToString()
        {
            SerializeToStringInternal(new BinarySerializer());
            SerializeToStringInternal(new XmlSerializer());
            SerializeToStringInternal(new JsonSerializer());
        }

        private static void SerializeToStringInternal<T>(IGenericSerializer<T> serializer)
        {
            Expression<Func<object, string>> expression = x => x.ToString();
            var serialized = serializer.SerializeGeneric(expression);
            var actualExpression = (Expression<Func<object, string>>)serializer.DeserializeGeneric(serialized);

            Assert.AreEqual(expression.GetDebugView(), actualExpression.GetDebugView());
            var comparer = new ExpressionComparer();
            Assert.IsTrue(comparer.AreEqual(expression, actualExpression));

            Assert.AreEqual(expression.ToString(), actualExpression.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1), actualExpression.Compile().Invoke(1));
        }
    }
}
