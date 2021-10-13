using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Serializers;
using System;
using System.Linq.Expressions;

namespace Serialize.Linq.Tests.NewTests
{
    [TestClass]
    public class Issue104
    {
        [TestMethod]
        public void SerializeBlockExpressionBinary()
        {
            Expression<Func<int, string>> expression = num => num.ToString();
            var parameter = Expression.Parameter(typeof(int), "num");
            BlockExpression block = Expression.Block(
                Expression.Invoke(expression, parameter));
            Expression<Func<int, string>> combinedExpression =
                Expression.Lambda<Func<int, string>>(
                    block,
                    parameter);

            // In version 2.0.0.0. this line throws an exception: System.ArgumentException: 'Unknown expression of type System.Linq.Expressions.BlockN'
            var value = combinedExpression.ToBinary();
#pragma warning disable CS0618 // type or member is obsolete
            var actualExpression = (Expression<Func<int, string>>)new ExpressionSerializer(new BinarySerializer()).DeserializeBinary(value);
#pragma warning restore CS0618 // type or member is obsolete
            var func = combinedExpression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(combinedExpression.ToString(), actualExpression.ToString());
            Assert.AreEqual(func(42), "42");
            Assert.AreEqual(actualFunc(42), "42");
        }

        [TestMethod]
        public void SerializeBlockExpressionXml()
        {
            Expression<Func<int, string>> expression = num => num.ToString();
            var parameter = Expression.Parameter(typeof(int), "num");
            BlockExpression block = Expression.Block(
                Expression.Invoke(expression, parameter));
            Expression<Func<int, string>> combinedExpression =
                Expression.Lambda<Func<int, string>>(
                    block,
                    parameter);

            // In version 2.0.0.0. this line throws an exception: System.ArgumentException: 'Unknown expression of type System.Linq.Expressions.BlockN'
            var value = combinedExpression.ToXml();
#pragma warning disable CS0618 // type or member is obsolete
            var actualExpression = (Expression<Func<int, string>>)new ExpressionSerializer(new XmlSerializer()).DeserializeText(value);
#pragma warning restore CS0618 // type or member is obsolete
            var func = combinedExpression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(combinedExpression.ToString(), actualExpression.ToString());
            Assert.AreEqual(func(42), "42");
            Assert.AreEqual(actualFunc(42), "42");
        }


        [TestMethod]
        public void SerializeBlockExpressionJson()
        {
            Expression<Func<int, string>> expression = num => num.ToString();
            var parameter = Expression.Parameter(typeof(int), "num");
            BlockExpression block = Expression.Block(
                Expression.Invoke(expression, parameter));
            Expression<Func<int, string>> combinedExpression =
                Expression.Lambda<Func<int, string>>(
                    block,
                    parameter);

            // In version 2.0.0.0. this line throws an exception: System.ArgumentException: 'Unknown expression of type System.Linq.Expressions.BlockN'
            var value = combinedExpression.ToJson();
#pragma warning disable CS0618 // type or member is obsolete
            var actualExpression = (Expression<Func<int, string>>)new ExpressionSerializer(new JsonSerializer()).DeserializeText(value);
#pragma warning restore CS0618 // type or member is obsolete
            var func = combinedExpression.Compile();
            var actualFunc = actualExpression.Compile();

            Assert.AreEqual(combinedExpression.ToString(), actualExpression.ToString());
            Assert.AreEqual(func(42), "42");
            Assert.AreEqual(actualFunc(42), "42");
        }

    }
}
