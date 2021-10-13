using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;
using System;
using System.Linq.Expressions;

namespace Serialize.Linq.Tests.NewTests
{
    [TestClass]
    public class Issue104Generic
    {
        [TestMethod]
        public void SerializeBlockExpressionGeneric()
        {
            SerializeBlockExpressionInternal(new BinarySerializer());
            SerializeBlockExpressionInternal(new XmlSerializer());
            SerializeBlockExpressionInternal(new JsonSerializer());
        }

        private static void SerializeBlockExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            Expression<Func<int, string>> expression = num => num.ToString();
            var parameter = Expression.Parameter(typeof(int), "num");
            BlockExpression block = Expression.Block(
                Expression.Invoke(expression, parameter));
            Expression<Func<int, string>> combinedExpression =
                Expression.Lambda<Func<int, string>>(
                    block,
                    parameter);

            var value = serializer.SerializeGeneric(combinedExpression);
            var actualExpression = (Expression<Func<int, string>>)serializer.DeserializeGeneric(value);
            var func = combinedExpression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(combinedExpression.ToString(), actualExpression.ToString());
            Assert.AreEqual(func(42), "42");
            Assert.AreEqual(actualFunc(42), "42");
        }
    }
}
