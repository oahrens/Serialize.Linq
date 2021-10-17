using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    [TestClass]
    public class BlockExpressionTests
    {
        [TestMethod]
        public void SerializeDeserializeBlockExpressionBinary()
        {
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

#pragma warning disable CS0618 // type or member is obsolete
                var serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
                serializer.AddKnownTypes(new[] { typeof(StackTrace), typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.Public | BindingFlags.NonPublic) });
                var value = serializer.SerializeBinary(expression);
                var actualExpression = (Expression<Func<StackTrace, string>>)serializer.DeserializeBinary(value, new ExpressionContext(true));

                Assert.AreEqual(expression.ToString(), actualExpression.ToString());

                var stack = expression.Compile().Invoke(new StackTrace());
                var actualStack = actualExpression.Compile().Invoke(new StackTrace());

                Assert.AreEqual(stack, actualStack);
            }
        }

        [TestMethod]
        public void SerializeDeserializeBlockExpressionXml()
        {
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

#pragma warning disable CS0618 // type or member is obsolete
                var serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
                serializer.AddKnownTypes(new[] { typeof(StackTrace), typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.Public | BindingFlags.NonPublic) });
                var value = serializer.SerializeText(expression);
                var actualExpression = (Expression<Func<StackTrace, string>>)serializer.DeserializeText(value, new ExpressionContext(true));

                Assert.AreEqual(expression.ToString(), actualExpression.ToString());

                var stack = expression.Compile().Invoke(new StackTrace());
                var actualStack = actualExpression.Compile().Invoke(new StackTrace());

                Assert.AreEqual(stack, actualStack);
            }
        }

        [TestMethod]
        public void SerializeDeserializeBlockExpressionJson()
        {
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

#pragma warning disable CS0618 // type or member is obsolete
                var serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
                serializer.AddKnownTypes(new[] { typeof(StackTrace), typeof(StackTrace).GetNestedType("TraceFormat", BindingFlags.Public | BindingFlags.NonPublic) });
                var value = serializer.SerializeText(expression);
                var actualExpression = (Expression<Func<StackTrace, string>>)serializer.DeserializeText(value, new ExpressionContext(true));

                Assert.AreEqual(expression.ToString(), actualExpression.ToString());

                var stack = expression.Compile().Invoke(new StackTrace());
                var actualStack = actualExpression.Compile().Invoke(new StackTrace());

                Assert.AreEqual(stack, actualStack);
            }
        }
    }
}
