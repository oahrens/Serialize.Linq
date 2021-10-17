using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// https://github.com/esskar/Serialize.Linq/issues/138
    /// </summary>
    [TestClass]
    public class Issue137Generic
    {
        [TestMethod]
        public void SerializeInnerFunction()
        {
            SerializeInnerFunctionInternal(new BinarySerializer());
            SerializeInnerFunctionInternal(new XmlSerializer());
            SerializeInnerFunctionInternal(new JsonSerializer());
        }

        private static void SerializeInnerFunctionInternal<TSerialize>(IGenericSerializer<TSerialize> serializer)
        {
            Expression<Func<object, object>> expr = x => FunctionsGeneric.F(x);

            var value = serializer.SerializeGeneric(expr);
            Expression<Func<object, object>> actualExpression = (Expression<Func<object, object>>)serializer.DeserializeGeneric(value);

            Assert.AreEqual(expr.ToString(), actualExpression.ToString());
            Assert.AreEqual(expr.Compile().Invoke(2), actualExpression.Compile().Invoke(2));
        }
    }

    internal class FunctionsGeneric
    {
        public static object F(object x)
        {
            return x;
        }
    }
}
