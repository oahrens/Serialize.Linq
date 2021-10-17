using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Tests.NewTests
{
    /// <summary>
    /// Example from: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.tryexpression?view=netframework-4.0
    /// </summary>
    [TestClass]
    public class TryExpressionTests
    {
        [TestMethod]
        public void SerializeDeserializeTryExpressionBinary()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new BinarySerializer());
#pragma warning restore CS0618 // type or member is obsolete
            TryExpression tryCatchExpr = Expression.TryCatch(Expression.Block(Expression.Throw(Expression.Constant(new DivideByZeroException())),
                                                                              Expression.Constant("Try block")),
                                                             Expression.Catch(typeof(DivideByZeroException),
                                                                              Expression.Constant("Catch block")));
            var expression = Expression.Lambda<Func<string>>(tryCatchExpr);

            serializer.AddKnownTypes(new[] { typeof(DivideByZeroException) });
            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeBinary(expression);
            var actualExpression = (Expression<Func<string>>)serializer.DeserializeBinary(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            Debug.WriteLine(action.Invoke());
            Debug.WriteLine(actualAction.Invoke());

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeTryExpressionXml()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new XmlSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            TryExpression tryCatchExpr = Expression.TryCatch(Expression.Block(Expression.Throw(Expression.Constant(new DivideByZeroException())),
                                                                              Expression.Constant("Try block")),
                                                             Expression.Catch(typeof(DivideByZeroException),
                                                                              Expression.Constant("Catch block")));
            var expression = Expression.Lambda<Func<string>>(tryCatchExpr);

            serializer.AddKnownTypes(new[] { typeof(DivideByZeroException) });
            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<string>>)serializer.DeserializeText(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            Debug.WriteLine(action.Invoke());
            Debug.WriteLine(actualAction.Invoke());

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }

        [TestMethod]
        public void SerializeDeserializeTryExpressionJson()
        {
#pragma warning disable CS0618 // type or member is obsolete
            ExpressionSerializer serializer = new ExpressionSerializer(new JsonSerializer());
#pragma warning restore CS0618 // type or member is obsolete
            TryExpression tryCatchExpr = Expression.TryCatch(Expression.Block(Expression.Throw(Expression.Constant(new DivideByZeroException())),
                                                                              Expression.Constant("Try block")),
                                                             Expression.Catch(typeof(DivideByZeroException),
                                                                              Expression.Constant("Catch block")));
            var expression = Expression.Lambda<Func<string>>(tryCatchExpr);

            serializer.AddKnownTypes(new[] { typeof(DivideByZeroException) });
            // the next line throws an ArgumentException in version 2.0.0.0:
            var value = serializer.SerializeText(expression);
            var actualExpression = (Expression<Func<string>>)serializer.DeserializeText(value);

            var action = expression.Compile();
            var actualAction = actualExpression.Compile();
            Debug.WriteLine(action.Invoke());
            Debug.WriteLine(actualAction.Invoke());

            Assert.AreEqual(action.ToString(), actualAction.ToString());
        }
    }
}
