using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/de-de/dotnet/api/system.linq.expressions.gotoexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class GotoExpressionTests
    {
        [TestMethod]
        public void SerializeDeserializeGotoExpressionBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete

            LabelTarget returnTarget = Expression.Label();
            LabelTarget fakeTarget0 = Expression.Label("#Label2");
            LabelTarget fakeTarget1 = Expression.Label();
            BlockExpression expression = Expression.Block(Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("GoTo")),
                                                          Expression.Goto(returnTarget),
                                                          Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("Other Work")),
                                                          Expression.Label(fakeTarget0),
                                                          Expression.Label(returnTarget),
                                                          Expression.Label(fakeTarget1));


            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeBinary(expression);
            var actualExpression = (BlockExpression)serializer.DeserializeBinary(value);

            var action = Expression.Lambda<Action>(expression).Compile();
            var actualAction = Expression.Lambda<Action>(actualExpression).Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeGotoExpressionXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete

            LabelTarget returnTarget = Expression.Label();
            LabelTarget fakeTarget0 = Expression.Label("#Label2");
            LabelTarget fakeTarget1 = Expression.Label();
            BlockExpression expression = Expression.Block(Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("GoTo")),
                                                          Expression.Goto(returnTarget),
                                                          Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("Other Work")),
                                                          Expression.Label(fakeTarget0),
                                                          Expression.Label(returnTarget),
                                                          Expression.Label(fakeTarget1));


            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (BlockExpression)serializer.DeserializeText(value);

            var action = Expression.Lambda<Action>(expression).Compile();
            var actualAction = Expression.Lambda<Action>(actualExpression).Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeGotoExpressionJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete

            LabelTarget returnTarget = Expression.Label();
            LabelTarget fakeTarget0 = Expression.Label("#Label2");
            LabelTarget fakeTarget1 = Expression.Label();
            BlockExpression expression = Expression.Block(Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("GoTo")),
                                                          Expression.Goto(returnTarget),
                                                          Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), Expression.Constant("Other Work")),
                                                          Expression.Label(fakeTarget0),
                                                          Expression.Label(returnTarget),
                                                          Expression.Label(fakeTarget1));


            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (BlockExpression)serializer.DeserializeText(value);

            var action = Expression.Lambda<Action>(expression).Compile();
            var actualAction = Expression.Lambda<Action>(actualExpression).Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
