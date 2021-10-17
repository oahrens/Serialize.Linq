using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.switchexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class SwitchExpressionTestsGeneric
    {
        [TestMethod]
        public void SerializeDeserializeSwitchExpression()
        {
            SerializeDeserializeSwitchExpressionInternal(new BinarySerializer());
            SerializeDeserializeSwitchExpressionInternal(new XmlSerializer());
            SerializeDeserializeSwitchExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeSwitchExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
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

            var value = serializer.SerializeGeneric(expression);
            var actualExpression = (Expression<Action>)serializer.DeserializeGeneric(value);

            Assert.AreEqual(expression.GetDebugView(), actualExpression.GetDebugView());

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            action.Invoke();
            actualAction.Invoke();

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
