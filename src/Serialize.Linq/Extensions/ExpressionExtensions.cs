#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Serialize.Linq.Factories;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Nodes;
using Serialize.Linq.Serializers;

namespace Serialize.Linq.Extensions
{
    /// <summary>
    /// Expression extension methods.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Converts an expression to an expression node.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factorySettings">The factory settings to use.</param>
        /// <returns></returns>
        public static ExpressionNode ToExpressionNode(this Expression expression, FactorySettings factorySettings = null)
        {
            var converter = new ExpressionConverter();
            return converter.Convert(expression, factorySettings);
        }

        /// <summary>
        /// Converts an expression to an json encoded string.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factorySettings">The factory settings to use.</param>
        /// <returns></returns>
        public static string ToJson(this Expression expression, FactorySettings factorySettings = null)
        {
            return expression.ToJson(expression.GetDefaultFactory(factorySettings));
        }

        /// <summary>
        /// Converts an expression to an json encoded string using the given factory.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        public static string ToJson(this Expression expression, INodeFactory factory)
        {
            return expression.ToJson(factory, new JsonSerializer());
        }

        /// <summary>
        /// Converts an expression to an json encoded string using the given factory and serializer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        public static string ToJson(this Expression expression, INodeFactory factory, IJsonSerializer serializer)
        {
            return expression.ToText(factory, serializer);
        }

        /// <summary>
        /// Converts an expression to an xml encoded string.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factorySettings">The factory settings to use.</param>
        /// <returns></returns>
        public static string ToXml(this Expression expression, FactorySettings factorySettings = null)
        {
            return expression.ToXml(expression.GetDefaultFactory(factorySettings));
        }

        /// <summary>
        /// Converts an expression to an xml encoded string using the given factory.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        public static string ToXml(this Expression expression, INodeFactory factory)
        {
            return expression.ToXml(factory, new XmlSerializer());
        }

        /// <summary>
        /// Converts an expression to an xml encoded string using the given factory and serializer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        public static string ToXml(this Expression expression, INodeFactory factory, IXmlSerializer serializer)
        {
            return expression.ToText(factory, serializer);
        }

        /// <summary>
        /// Converts an expression to an encoded string using the given factory and serializer.
        /// The encoding is decided by the serializer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// factory
        /// or
        /// serializer
        /// </exception>
        public static string ToText(this Expression expression, INodeFactory factory, ITextSerializer serializer)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(factory.Create(expression));
        }

        /// <summary>
        /// Converts an expression to a byte array.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factorySettings">The factory settings to use.</param>
        /// <returns>The byte array representing the serialized <paramref name="expression"/>.</returns>
        public static byte[] ToBinary(this Expression expression, FactorySettings factorySettings = null)
        {
            return expression.ToBinary(expression.GetDefaultFactory(factorySettings));
        }

        /// <summary>
        /// Converts an expression to a byte array using the given factory.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The byte array representing the serialized <paramref name="expression"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="factory"/>
        /// </exception>
        public static byte[] ToBinary(this Expression expression, INodeFactory factory)
        {
            return expression.ToBinary(factory, new BinarySerializer());
        }

        /// <summary>
        /// Converts an expression to a byte array using the given factory and serializer.
        /// The encoding is decided by the serializer.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The byte array representing the serialized <paramref name="expression"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="factory"/>
        /// or
        /// <paramref name="serializer"/>
        /// </exception>
        public static byte[] ToBinary(this Expression expression, INodeFactory factory, IBinarySerializer serializer)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            return serializer.Serialize(factory.Create(expression));
        }

        /// <summary>
        /// Gets the default factory.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="factorySettings">The factory settings to use.</param>
        /// <returns></returns>
        internal static INodeFactory GetDefaultFactory(this Expression expression, FactorySettings factorySettings)
        {
            if (expression is LambdaExpression lambda)
                return new DefaultNodeFactory(lambda.Parameters.Select(p => p.Type), factorySettings);
            return new NodeFactory(factorySettings);
        }

        /// <summary>
        /// From https://stackoverflow.com/a/31360768/2883733.
        /// </summary>
        public static string GetDebugView(this Expression expression)
        {
            // ToDo: Funktion kompilieren
            PropertyInfo info;

            if (expression == null)
                return null;
            else
            {
                info = typeof(Expression).GetProperty("DebugView", BindingFlags.Instance | BindingFlags.NonPublic);

                return info.GetValue(expression, null/* TODO Change to default(_) if this is not a reference type */) as string;
            }
        }
    }
}