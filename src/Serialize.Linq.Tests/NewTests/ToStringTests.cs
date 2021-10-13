using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Test and fix provided by https://github.com/oahrens
    /// </summary>
    [TestClass]
    public class ToStringTests
    {
        [TestMethod]
        public void SerializeToStringBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, string>> expression = x => x.ToString();
            // the next line throws a InvalidOperationException in version 2.0.0.0
            var serialized = serializer.SerializeBinary(expression);
            var actualExpression = (Expression<Func<object, string>>)serializer.DeserializeBinary(serialized);

            Assert.AreEqual(expression.ToString(), actualExpression.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1), actualExpression.Compile().Invoke(1));
        }

        [TestMethod]
        public void SerializeToStringXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, string>> expression = x => x.ToString();
            // the next line throws a InvalidOperationException in version 2.0.0.0
            var serialized = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<object, string>>)serializer.DeserializeText(serialized);

            Assert.AreEqual(expression.ToString(), actualExpression.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1), actualExpression.Compile().Invoke(1));
        }

        [TestMethod]
        public void SerializeToStringJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            Expression<Func<object, string>> expression = x => x.ToString();
            // the next line throws a InvalidOperationException in version 2.0.0.0
            var serialized = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<object, string>>)serializer.DeserializeText(serialized);

            Assert.AreEqual(expression.ToString(), actualExpression.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1), actualExpression.Compile().Invoke(1));
        }
    }
}
