using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// https://github.com/esskar/Serialize.Linq/issues/138
    /// </summary>
    [TestClass]
    public class Issue138
    {
        [TestMethod]
        public void SerializeInnerFunctionBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            var serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, object>> expr = x => Functions.F(x);

            var value = serializer.SerializeBinary(expr);
            Expression<Func<object, object>> actualExpression = (Expression<Func<object, object>>)serializer.DeserializeBinary(value);

            Assert.AreEqual(expr.ToString(), actualExpression.ToString());
            Assert.AreEqual(expr.Compile().Invoke(2), actualExpression.Compile().Invoke(2));
        }

        [TestMethod]
        public void SerializeInnerFunctionXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            var serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, object>> expr = x => Functions.F(x);

            var value = serializer.SerializeText(expr);
            Expression<Func<object, object>> actualExpression = (Expression<Func<object, object>>)serializer.DeserializeText(value);

            Assert.AreEqual(expr.ToString(), actualExpression.ToString());
            Assert.AreEqual(expr.Compile().Invoke(2), actualExpression.Compile().Invoke(2));
        }

        [TestMethod]
        public void SerializeInnerFunctionJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            var serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, object>> expr = x => Functions.F(x);

            var value = serializer.SerializeText(expr);
            Expression<Func<object, object>> actualExpression = (Expression<Func<object, object>>)serializer.DeserializeText(value);

            Assert.AreEqual(expr.ToString(), actualExpression.ToString());
            Assert.AreEqual(expr.Compile().Invoke(2), actualExpression.Compile().Invoke(2));
        }
    }

    internal class Functions
    {
        public static object F(object x)
        {
            return x;
        }
    }
}
