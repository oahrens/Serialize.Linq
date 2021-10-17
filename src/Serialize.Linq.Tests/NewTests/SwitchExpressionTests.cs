using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.switchexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class SwitchExpressionTests
    {
        [TestMethod]
        public void SerializeDeserializeSwitchExpressionBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
            ConstantExpression switchValue = Expression.Constant(3);

            SwitchExpression switchExpr = Expression.Switch(switchValue,
                                                            Expression.Call(null,
                                                                            typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                            Expression.Constant("Default")),
                                                            new SwitchCase[] { Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("First")),
                                                                                                     Expression.Constant(1)),
                                                                               Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("Second")),
                                                                                                     Expression.Constant(2))});

            var expression = Expression.Lambda<Action>(switchExpr);

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeBinary(expression);
            var actualExpression = (Expression<Action>)serializer.DeserializeBinary(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeSwitchExpressionXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            ConstantExpression switchValue = Expression.Constant(3);

            SwitchExpression switchExpr = Expression.Switch(switchValue,
                                                            Expression.Call(null,
                                                                            typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                            Expression.Constant("Default")),
                                                            new SwitchCase[] { Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("First")),
                                                                                                     Expression.Constant(1)),
                                                                               Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("Second")),
                                                                                                     Expression.Constant(2))});

            var expression = Expression.Lambda<Action>(switchExpr);

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Action>)serializer.DeserializeText(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeSwitchExpressionJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            ConstantExpression switchValue = Expression.Constant(3);

            SwitchExpression switchExpr = Expression.Switch(switchValue,
                                                            Expression.Call(null,
                                                                            typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                            Expression.Constant("Default")),
                                                            new SwitchCase[] { Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("First")),
                                                                                                     Expression.Constant(1)),
                                                                               Expression.SwitchCase(Expression.Call(null,
                                                                                                                     typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }),
                                                                                                                     Expression.Constant("Second")),
                                                                                                     Expression.Constant(2))});

            var expression = Expression.Lambda<Action>(switchExpr);

            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Action>)serializer.DeserializeText(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
