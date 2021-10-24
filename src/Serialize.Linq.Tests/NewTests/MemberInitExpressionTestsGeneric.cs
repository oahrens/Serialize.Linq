using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Extensions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Serializers;
using Serialize.Linq.Tests.Internals;

namespace Serialize.Linq.Tests.NewTests
{
    [TestClass]
    public class MemberInitExpressionTestsGeneric
    {

        /// <summary>
        /// From: https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.memberinitexpression?view=netframework-4.5.
        /// </summary>
        [TestMethod]
        public void SerializeDeserializeMemberInitExpression()
        {
            SerializeDeserializeMemberInitExpressionInternal(new BinarySerializer());
            SerializeDeserializeMemberInitExpressionInternal(new XmlSerializer());
            SerializeDeserializeMemberInitExpressionInternal(new JsonSerializer());
        }

        private static void SerializeDeserializeMemberInitExpressionInternal<T>(IGenericSerializer<T> serializer)
        {
            NewExpression newAnimal = Expression.New(typeof(Animal));
            MemberInfo speciesMember = typeof(Animal).GetMember("Species")[0];
            MemberInfo ageMember = typeof(Animal).GetMember("Age")[0];
            MemberInfo speciesFieldMember = typeof(Animal).GetMember("SpeciesField")[0];
            MemberInfo ageFieldMember = typeof(Animal).GetMember("AgeField")[0];

            MemberBinding speciesMemberBinding = System.Linq.Expressions.Expression.Bind(speciesMember,
                                                                                         Expression.Constant("horse"));
            MemberBinding ageMemberBinding = Expression.Bind(ageMember,
                                                             Expression.Constant(12));
            MemberBinding speciesFieldMemberBinding = System.Linq.Expressions.Expression.Bind(speciesFieldMember,
                                                                                             Expression.Constant("horse"));
            MemberBinding ageFieldMemberBinding = Expression.Bind(ageFieldMember,
                                                                  Expression.Constant(12));
            MemberInitExpression memberInitExpression = Expression.MemberInit(newAnimal,
                                                                              speciesMemberBinding,
                                                                              speciesFieldMemberBinding,
                                                                              ageMemberBinding,
                                                                              ageFieldMemberBinding);

            var value = serializer.SerializeGeneric(memberInitExpression, null);
            var actualExpression = serializer.DeserializeGeneric(value, null);

            Assert.AreEqual(memberInitExpression.GetDebugView(), actualExpression.GetDebugView());
            var comparer = new ExpressionComparer();
            Assert.IsTrue(comparer.AreEqual(memberInitExpression, actualExpression));

            Console.WriteLine(memberInitExpression.ToString());
        }
    }

#pragma warning disable CS0649 // Field 'field' is never assigned to, and will always have its default value 'value'
    internal class Animal
    {
        public string Species { get; set; }
        public string SpeciesField;
        public int Age { get; set; }
        public int AgeField;
    }
#pragma warning disable CS0649 // Field 'field' is never assigned to, and will always have its default value 'value'
}
