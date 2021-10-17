using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    [TestClass]
    public class BlockExpressionTestsGeneric
    {
        [TestMethod]
        public void SerializeDeserializeBlockExpression()
        {
            SerializeDeserializeBlockExpressionInternal(new BinarySerializer());
            SerializeDeserializeBlockExpressionInternal(new XmlSerializer());
            SerializeDeserializeBlockExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeBlockExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            ParameterExpression ex;
            Type traceFormatType;
            MethodInfo toStringFunction;
            ConstantExpression traceFormatNormal;
            MethodCallExpression stackTraceCall;

            ex = Expression.Parameter(typeof(StackTrace));
            traceFormatType = typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.Public | BindingFlags.NonPublic);
            toStringFunction = typeof(StackTrace).GetMethod("ToString", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { traceFormatType }, null);
            traceFormatNormal = Expression.Constant((Enum)Enum.GetValues(traceFormatType).GetValue(0), traceFormatType);
            stackTraceCall = Expression.Call(ex, toStringFunction, traceFormatNormal);

            var expression = Expression.Lambda<Func<StackTrace, string>>(Expression.Block(stackTraceCall), ex);

            serializer.AddKnownTypes(new[] { typeof(StackTrace), typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.Public | BindingFlags.NonPublic) });
            var value = serializer.SerializeGeneric(expression);
            var actualExpression = (Expression<Func<StackTrace, string>>)serializer.DeserializeGeneric(value, new ExpressionContext(true));

            Assert.AreEqual(expression.ToString(), actualExpression.ToString());

            var stack = expression.Compile().Invoke(new StackTrace());
            var actualStack = actualExpression.Compile().Invoke(new StackTrace());

            Assert.AreEqual(stack, actualStack);
        }
    }
}
